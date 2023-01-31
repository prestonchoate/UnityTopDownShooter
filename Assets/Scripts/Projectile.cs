using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static event Action<Projectile> Die;
    [SerializeField] private float damage = 0.0f;

    private float lifetime = 5.0f;
    private bool paused = false;
    private Rigidbody2D rb;
    private Vector3 velocity;
    private float angularVelocity;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        GameManager.GameStateChanged += CheckState;
        lifetime = 5.0f;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= CheckState;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                // Destroy(gameObject);
                //gameObject.SetActive(false);
                Die?.Invoke(this);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") != false)
        {
            // gameObject.SetActive(false);
            Die?.Invoke(this);
        }

    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float damageVal)
    {
        damage = damageVal;
    }

    void CheckState(GameState newState)
    {
        paused = (newState != GameState.Playing);
        if (paused)
        {
            velocity = rb.velocity;
            angularVelocity = rb.angularVelocity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;
        }
        else
        {
            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
    }


}
