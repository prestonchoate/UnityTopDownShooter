using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{

    #region SerializedFields
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireForce = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float defaultFireDelay = 1.0f;

    #endregion

    #region PrivateFields

    private float fireDelay = 1.0f;
    private float currentDamage;
    private float currentFireForce;
    private float currentFireDelay;
    private IObjectPool<Projectile> projectiles;
    private GameObject projectileContainer;

    #endregion

    void Awake()
    {
        projectileContainer = new GameObject("ProjectileContainer");
        projectileContainer.transform.SetParent(transform);
        projectiles = new ObjectPool<Projectile>(InstantiateProjectile, GetProjectile, ReleaseProjectile);
        Reset();
    }

    void OnEnable()
    {
        Projectile.Die += HandleDeadProjectile;
    }

    void OnDisable()
    {
        Projectile.Die -= HandleDeadProjectile;
    }

    void Update()
    {
        fireDelay -= Time.deltaTime;
    }

    public void Fire()
    {
        if (canFire())
        {
            Projectile projectile;
            projectiles.Get(out projectile);
            projectile.SetDamage(damage);
            projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        }
    }

    private bool canFire()
    {
        if (fireDelay <= 0)
        {
            fireDelay = defaultFireDelay;
            return true;
        }
        return false;
    }

    void HandleDeadProjectile(Projectile projectile)
    {
        projectiles.Release(projectile);
    }

    Projectile InstantiateProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectile.transform.SetParent(projectileContainer.transform);
        return projectile;
    }

    public void GetProjectile(Projectile p)
    {
        p.gameObject.SetActive(true);
        p.gameObject.transform.position = firePoint.transform.position;
    }

    public void ReleaseProjectile(Projectile p)
    {
        p.gameObject.SetActive(false);
    }

    public void Reset()
    {
        currentFireForce = fireForce;
        currentDamage = damage;
        currentFireDelay = fireDelay;
        projectiles.Clear();
    }
}
