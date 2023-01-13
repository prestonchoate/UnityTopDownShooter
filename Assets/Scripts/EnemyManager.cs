using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance { get; private set; }

    [SerializeField]
    private GameObject enemyPrefab;
    private int enemiesToPreload = 1000;
    private static List<GameObject> enemies = new List<GameObject>();
    private int minX, minY = -250;
    private int maxX, maxY = 250;
    private float defaultSpawnTime = 0.25f;
    private float spawnTime = 0.25f;
    private int maxActiveEnemies = 150;
    private int activeEnemies = 0;
    private GameObject enemyContainer;


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
        LoadObjectPool(enemiesToPreload);
        Enemy.EnemyKilled += DeactivateEnemy;
    }

    void OnDisable()
    {
        Enemy.EnemyKilled -= DeactivateEnemy;
    }

    // Function to add additional enemies to the pool
    void LoadObjectPool(int objToPool)
    {
        for (int i = 0; i <= objToPool; i++)
        {
            GameObject enemy = GameObject.Instantiate(enemyPrefab);
            enemy.transform.SetParent(enemyContainer.transform);
            enemy.SetActive(false);
            enemies.Add(enemy);

        }
    }

    void SpawnEnemies(int count = 1)
    {
        if (activeEnemies < maxActiveEnemies)
        {
            IEnumerable<GameObject> enemiesToActivate = enemies.Where(e => e.activeInHierarchy == false).Take(count);
            // TODO: If enemiesToActivate is empty add more enemies to the pool
            foreach (GameObject enemyToActivate in enemiesToActivate)
            {

                enemyToActivate.transform.position = GetSpawnPoint(GameManager.Instance.Player.transform.position, 30.0f);
                //TODO: Before spawning show a spawn warning sprite
                enemyToActivate.SetActive(true);
                activeEnemies++;
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

    // Update is called once per frame
    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0.0f)
        {
            spawnTime = defaultSpawnTime;
            SpawnEnemies();
        }
    }

    void DeactivateEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        activeEnemies--;
    }
}
