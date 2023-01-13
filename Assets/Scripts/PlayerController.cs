using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public delegate void TakeDamage(float damageAmount);
    public static event TakeDamage PlayerDamaged;

    private Weapon weapon;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private float totalExperience = 0;
    private float expToNextLevel;
    private int currentLevel = 1;
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private AnimationCurve expCurve;
    private float maxHp = 100.0f;
    private float currentHp;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        weapon = this.GetComponentInChildren<Weapon>();
        Experience.OnExperiencePickup += AddExperience;

    }

    void OnDisable()
    {
        Experience.OnExperiencePickup -= AddExperience;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        UpdateExpToGain();
    }

    // Update is called once per frame
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
    }

    void CheckExperience()
    {
        if (totalExperience >= expToNextLevel)
        {
            // TODO: Send level up event
            currentLevel++;
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
        if (other.gameObject.CompareTag("enemy"))
        {
            float damageToTake = other.gameObject.GetComponent<Enemy>().Damage;
        }
    }
}
