using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance { get; private set; }

    [SerializeField]
    private GameObject enemyPrefab;
    private int enemiesToPreload = 1000;
    private int minX, minY = -250;
    private int maxX, maxY = 250;
    private float defaultSpawnTime = 0.25f;
    private float spawnTime = 0.25f;
    private int defaultMaxActiveEnemies = 150;
    private int maxActiveEnemies = 150;
    private int activeEnemies = 0;
    private GameObject enemyContainer;
    private IObjectPool<Enemy> enemyPool;


    void Awake()
    {
        // Prevent another instance of this class from being instantiated
        if (Instance is not null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        enemyContainer = new GameObject("EnemyContainer");
        enemyContainer.transform.SetParent(transform);
        enemyPool = new ObjectPool<Enemy>(createFunc: InstantiateEnemy, GetEnemy, ReleaseEnemy, DestroyEnemy);
        Enemy.Killed += DeactivateEnemy;
    }

    Enemy InstantiateEnemy()
    {
        Enemy e = Instantiate(enemyPrefab).GetComponent<Enemy>();
        e.transform.SetParent(enemyContainer.transform);
        return e;
    }

    public void GetEnemy(Enemy e)
    {
        e.transform.position = GetSpawnPoint(GameManager.Instance.Player.transform.position, 30.0f);
        e.gameObject.SetActive(true);
        activeEnemies++;
    }

    public void ReleaseEnemy(Enemy e)
    {
        e.gameObject.SetActive(false);
        activeEnemies--;
    }

    public void DestroyEnemy(Enemy e)
    {
        Destroy(e);
    }

    void OnDisable()
    {
        Enemy.Killed -= DeactivateEnemy;
    }

    void SpawnEnemies(int count = 1)
    {
        if (activeEnemies < maxActiveEnemies)
        {
            int i = 0;
            while (i < count)
            {
                Enemy e;
                enemyPool.Get(out e);
                i++;
            }
        }
    }

    Vector3 GetSpawnPoint()
    {
        int xPos = Random.Range(minX, maxX + 1);
        int yPos = Random.Range(minY, maxY + 1);
        Vector3 spawnPoint = new Vector3(xPos, yPos, 0);
        return spawnPoint;
    }

    Vector3 GetSpawnPoint(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0.0f && GameManager.Instance.CurrentState == GameState.Playing)
        {
            spawnTime = defaultSpawnTime;
            SpawnEnemies();
        }
    }

    void DeactivateEnemy(Enemy enemy)
    {
        enemyPool.Release(enemy);
    }

    public void DeactivateAllEnemies()
    {
        enemyPool.Clear();
        activeEnemies = 0;
    }
}
