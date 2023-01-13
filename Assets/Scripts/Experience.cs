using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField]
    private float minExpValue = 3.0f;
    [SerializeField]
    private float maxExpValue = 10.0f;
    private float expValue;
    public delegate void ExperiencePickup(float expValue);
    public static event ExperiencePickup OnExperiencePickup;

    void Start()
    {
        expValue = Random.Range(minExpValue, maxExpValue);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnExperiencePickup(expValue);
            Destroy(gameObject);
        }
    }
}
