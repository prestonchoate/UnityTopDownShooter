using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void Deactivate(GameObject enemy);
    public static event Deactivate EnemyKilled;

    public float Damage { get; private set; }

    [SerializeField]
    private float hp = 25.0f;
    [SerializeField]
    private float moveSpeed = 1.5f;

    private Rigidbody2D rb;
    private GameObject target;
    private Vector3 targetPos;
    private Vector3 targetOffset;
    private Vector3 velocity = Vector3.zero;
    private float minDamage = 5.0f;
    private float maxDamage = 15.0f;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");
        if (target is null)
        {
            Debug.Log("Could not find target. Dying");
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        Damage = Random.Range(minDamage, maxDamage);
    }

    // Update is called once per frame
    void Update()
    {
        // targetOffset = GenerateNewOffset();
        // targetPos = Vector3.SmoothDamp(rb.transform.position, target.transform.position + targetOffset, ref velocity, 0.15f, moveSpeed);
        targetPos = target.transform.position - rb.transform.position + targetOffset;
        targetPos.Normalize();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.transform.position + targetPos * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectiles"))
        {
            float damage = other.gameObject.GetComponent<Projectile>().GetDamage();
            TakeDamage(damage);

        }
    }

    void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            EnemyKilled(gameObject);
        }
    }

    Vector3 GenerateNewOffset()
    {
        return new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-8.0f, 8.0f), 0.0f);
    }
}
