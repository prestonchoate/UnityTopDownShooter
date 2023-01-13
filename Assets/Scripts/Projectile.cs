using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage = 0.0f;

    private float lifetime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float damageVal)
    {
        damage = damageVal;
    }
}
