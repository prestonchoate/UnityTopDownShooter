using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField]
    private float minExpValue = 300.0f;
    [SerializeField]
    private float maxExpValue = 10000.0f;
    [SerializeField]
    private float timeToLive = 5.0f;

    private float expValue;
    public static event Action<float> OnExperiencePickup;

    void Start()
    {
        expValue = UnityEngine.Random.Range(minExpValue, maxExpValue);
    }

    void OnEnable()
    {
        GameManager.GameStateChanged += HandleGameStateChange;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= HandleGameStateChange;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnExperiencePickup(expValue);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        timeToLive -= Time.deltaTime;
        // TODO: Implement fade out
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }

    void HandleGameStateChange(GameState newState)
    {
        if (newState != GameState.Playing)
        {
            Destroy(this);
        }
    }
}
