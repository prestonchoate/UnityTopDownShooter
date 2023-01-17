using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public delegate void TakeDamage(float damageAmount, float currentHP, float maxHP);
    public static event TakeDamage PlayerDamaged;
    public delegate void GainExp(float expGained, float totalExp, float expToNextLevel);
    public static event GainExp GainedExperience;
    public delegate void Level(int level);
    public static event Level LevelUp;
    public delegate void PlayerDeath();
    public static event PlayerDeath Died;

    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private AnimationCurve expCurve;

    private Weapon weapon;
    private BarManager healthBarManager;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private float totalExperience = 0;
    private float expToNextLevel;
    private int currentLevel = 1;
    private float maxHp = 100.0f;
    private float currentHp;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        weapon = this.GetComponentInChildren<Weapon>();
        healthBarManager = this.GetComponent<BarManager>();
        Experience.OnExperiencePickup += AddExperience;

    }

    void OnDisable()
    {
        Experience.OnExperiencePickup -= AddExperience;
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateExpToGain();
    }

    void Update()
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

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
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
}
