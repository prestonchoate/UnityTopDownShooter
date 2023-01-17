using UnityEngine;

public enum GameStates
{
    MainMenu,
    Loading,
    Playing,
    Paused,
    LevelUp,
    GameOver
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public Canvas WorldSpaceCanvas { get; private set; }

    [SerializeField]
    private GameObject lootPrefab;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject enemyManagerPrefab;
    [SerializeField]
    private Canvas worldSpaceCanvas;


    public GameObject Player { get; private set; }
    private GameObject enemyManager;
    private GameStates gameState;
    private Vector3 defaultPlayerPosition = new Vector3(0, 0, 0);


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
        WorldSpaceCanvas = worldSpaceCanvas;
        // TODO: Create an event for changing game states
        gameState = GameStates.Loading;
        Enemy.EnemyKilled += SpawnLoot;
        PlayerController.Died += HandlePlayerDeath;
        SpawnPlayer();
        CreateEnemyManager();
        // TODO: Set a more appropriate state and create transitions
        gameState = GameStates.Playing;
    }

    void OnDisable()
    {
        Enemy.EnemyKilled -= SpawnLoot;
        PlayerController.Died -= HandlePlayerDeath;
    }

    void SpawnLoot(GameObject go)
    {
        // TODO: Make this pull spawnable loot from the gameObject passed into this event observer
        GameObject.Instantiate(lootPrefab, go.transform.position, go.transform.rotation);
    }

    public static int GetRandomInteger(int min = 0, int max = 100)
    {
        return Random.Range(min, max + 1);
    }

    public static float GetRandomFloat(float min = 0.0f, float max = 100.0f)
    {
        return Random.Range(min, max);
    }

    void SpawnPlayer()
    {
        Player = GameObject.Instantiate(playerPrefab, defaultPlayerPosition, new Quaternion(0, 0, 0, 0));
    }

    void CreateEnemyManager()
    {
        enemyManager = GameObject.Instantiate(enemyManagerPrefab, new Vector3(), new Quaternion());
    }

    void HandlePlayerDeath()
    {
        Player.SetActive(false);
        gameState = GameStates.GameOver;
    }
}
