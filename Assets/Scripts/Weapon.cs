using UnityEngine;

public class Weapon : MonoBehaviour
{

    #region SerializedFields
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private float fireForce = 20f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float defaultFireDelay = 1.0f;

    #endregion

    #region PrivateFields

    private float fireDelay = 1.0f;

    #endregion

    void Update()
    {
        fireDelay -= Time.deltaTime;

    }

    public void Fire()
    {
        if (canFire())
        {
            // TODO: utilize object pooling instead of straight instantiation
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Projectile>().SetDamage(damage);
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
}
