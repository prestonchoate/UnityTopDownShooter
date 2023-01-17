using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action<float, float, float> PlayerDamaged;
    public static event Action<float, float, float> GainedExperience;
    public static event Action<int> LevelUp;
    public static event Action Died;

    [SerializeField] private float defaultMoveSpeed = 5.0f;
    [SerializeField] private AnimationCurve expCurve;

    private Weapon weapon;
    private BarManager healthBarManager;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private float totalExperience = 0;
    private float expToNextLevel;
    private int currentLevel = 1;
    private float maxHp = 100.0f;
    private float defaultHp = 100.0f;
    private float moveSpeed;
    private float currentHp;
    private bool paused;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        weapon = this.GetComponentInChildren<Weapon>();
        healthBarManager = this.GetComponent<BarManager>();
        moveSpeed = defaultMoveSpeed;

    }

    void OnEnable()
    {
        Experience.OnExperiencePickup += AddExperience;
        GameManager.GameStateChanged += CheckState;
    }

    void OnDisable()
    {
        Experience.OnExperiencePickup -= AddExperience;
        GameManager.GameStateChanged -= CheckState;
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateExpToGain();
        totalExperience = expCurve.Evaluate((float)currentLevel);
    }

    void Update()
    {
        if (!paused)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
            {
                weapon.Fire();
            }

            moveDirection = new Vector2(moveX, moveY).normalized;
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckExperience();
        }
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
            Vector2 aimDirection = mousePosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = aimAngle;
        }
        else
        {
            // Immediately stop if the game is paused
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;
        }
    }

    void AddExperience(float expValue)
    {
        totalExperience += expValue;
        GainedExperience?.Invoke(expValue, totalExperience, expToNextLevel);
    }

    void CheckExperience()
    {
        if (totalExperience >= expToNextLevel)
        {
            currentLevel++;
            LevelUp?.Invoke(currentLevel);
            Debug.Log("Player leveled up! They are now level " + currentLevel);
            UpdateExpToGain();
        }
    }

    void UpdateExpToGain()
    {
        expToNextLevel = expCurve.Evaluate((float)currentLevel + 1);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            float damageToTake = other.gameObject.GetComponent<Enemy>().Damage;
            UpdateHealth(-damageToTake);

        }
    }

    void UpdateHealth(float adjustment)
    {
        currentHp += adjustment;
        PlayerDamaged?.Invoke(adjustment, currentHp, maxHp);
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        if (currentHp <= 0.0f)
        {
            Died?.Invoke();

        }
    }

    void CheckState(GameStates newState)
    {
        paused = newState == GameStates.Paused;
    }

    public void Reset(Vector2 position)
    {
        transform.position = position;
        currentHp = defaultHp;
        maxHp = defaultHp;
        currentLevel = 1;
        totalExperience = expCurve.Evaluate((float)currentLevel);
        UpdateExpToGain();
        moveSpeed = defaultMoveSpeed;
        weapon.Reset();
        // TODO: Update bar positions
    }
}
