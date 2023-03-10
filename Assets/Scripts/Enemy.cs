using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> Killed;

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
    private bool paused = false;
    private float defaultHp = 25.0f;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player");
        if (target is null)
        {
            Debug.Log("Could not find target. Dying");
            gameObject.SetActive(false);
        }
        GameManager.GameStateChanged += CheckState;
    }

    void OnEnable()
    {
        hp = defaultHp;
        Damage = UnityEngine.Random.Range(minDamage, maxDamage);
    }

    void OnDestroy()
    {
        GameManager.GameStateChanged -= CheckState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            targetPos = target.transform.position - rb.transform.position + targetOffset;
            targetPos.Normalize();
        }
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            rb.MovePosition(rb.transform.position + targetPos * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Immediately stop if the game is paused
            // TODO: Convert this to a reusable "Pausable" script
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;
        }
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
            Killed?.Invoke(this);
        }
    }

    void CheckState(GameState newState)
    {
        paused = (newState != GameState.Playing);
        if (newState == GameState.Loading)
        {
            Killed?.Invoke(this);
        }
    }
}
