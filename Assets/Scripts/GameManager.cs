using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public enum GameState
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
    public static event Action<GameState> GameStateChanged;
    public static event Action<int> ScoreUpdated;
    public static event Action DoneLoading;

    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public int Score { get; private set; }

    public GameState CurrentState
    {
        get
        {
            return gameState;
        }
        private set { }
    }

    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyManagerPrefab;
    [SerializeField] private Canvas worldSpaceCanvas;

    private GameObject enemyManager;
    private GameState gameState;
    private GameState previousState;
    private Vector3 defaultPlayerPosition = new Vector3(0, 0, 0);
    private bool finishedLoading = false;


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
        SpawnPlayer();
    }

    void Start()
    {
        ChangeGameState(GameState.Loading);
        Enemy.Killed += SpawnLoot;
        PlayerController.Died += HandlePlayerDeath;
        Score = 0;
        ScoreUpdated?.Invoke(Score);
        CreateEnemyManager();
        finishedLoading = true;
        DoneLoading?.Invoke();
    }

    void OnDisable()
    {
        Enemy.Killed -= SpawnLoot;
        PlayerController.Died -= HandlePlayerDeath;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (gameState)
            {
                case GameState.Paused:
                    ChangeGameState(previousState);
                    break;
                default:
                    previousState = gameState;
                    ChangeGameState(GameState.Paused);
                    break;
            }
        }

        if (gameState == GameState.Paused && Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
#if (UNITY_EDITOR)
            EditorApplication.ExitPlaymode();
#endif
        }

        if (gameState == GameState.GameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (gameState == GameState.Loading && finishedLoading && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.Playing);
            finishedLoading = false;
        }
    }

    void SpawnLoot(Enemy go)
    {
        if (gameState == GameState.Playing)
        {
            Score += 100;
            ScoreUpdated.Invoke(Score);
            // TODO: Make this pull spawnable loot from the gameObject passed into this event observer
            GameObject.Instantiate(lootPrefab, go.transform.position, go.transform.rotation);
        }
    }

    public static int GetRandomInteger(int min = 0, int max = 100)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    public static float GetRandomFloat(float min = 0.0f, float max = 100.0f)
    {
        return UnityEngine.Random.Range(min, max);
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
        ChangeGameState(GameState.GameOver);
    }

    void ChangeGameState(GameState newState = GameState.Playing)
    {
        gameState = newState;
        GameStateChanged?.Invoke(gameState);
    }

    void RestartGame()
    {
        ChangeGameState(GameState.Loading);
        Player.gameObject.SetActive(false);
        Player.GetComponent<PlayerController>()?.Reset(defaultPlayerPosition);
        EnemyManager.Instance.DeactivateAllEnemies();
        Score = 0;
        ScoreUpdated.Invoke(Score);
        Player.SetActive(true);
        finishedLoading = true;
        DoneLoading?.Invoke();
        // ChangeGameState(GameState.Playing);
    }
}
