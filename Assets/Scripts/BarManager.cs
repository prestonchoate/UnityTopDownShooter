using UnityEngine;

public class BarManager : MonoBehaviour
{

    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private Vector3 healthPositionOffset = new Vector3(0.0f, 1.0f, 0.0f);
    [SerializeField]
    private GameObject expBarPrefab;
    [SerializeField]
    private Vector3 expPositionOffset = new Vector3(0.0f, 2.0f, 0.0f);

    private GameObject healthBar;
    private GameObject expBar;
    private BarUpdater healthUpdater;
    private BarUpdater expUpdater;

    void Awake()
    {
        healthBar = GameObject.Instantiate(healthBarPrefab, transform.position + healthPositionOffset, gameObject.transform.rotation);
        healthBar.transform.SetParent(GameManager.Instance.WorldSpaceCanvas.transform);
        healthUpdater = healthBar.gameObject.GetComponentInChildren<BarUpdater>();
        expBar = GameObject.Instantiate(expBarPrefab, transform.position + expPositionOffset, transform.rotation);
        expBar.transform.SetParent(GameManager.Instance.WorldSpaceCanvas.transform);
        expUpdater = expBar.gameObject.GetComponentInChildren<BarUpdater>();
        expUpdater.HasText = true;
    }

    void OnEnable()
    {
        PlayerController.PlayerDamaged += UpdateHealth;
        PlayerController.GainedExperience += UpdateExp;
        PlayerController.LevelUp += UpdateLevel;
    }

    void OnDisable()
    {
        PlayerController.PlayerDamaged -= UpdateHealth;
        PlayerController.GainedExperience -= UpdateExp;
        PlayerController.LevelUp -= UpdateLevel;
    }

    void LateUpdate()
    {
        healthBar.transform.position = transform.position + healthPositionOffset;
        expBar.transform.position = transform.position + expPositionOffset;
    }

    void UpdateHealth(float damageTaken, float currentHealth, float maxHealth)
    {
        healthUpdater.UpdateBar(currentHealth / maxHealth);
    }

    void UpdateExp(float expGained, float totalExp, float expToNextLevel)
    {
        expUpdater.UpdateBar(totalExp / expToNextLevel);
    }

    void UpdateLevel(int level)
    {
        Debug.Log("Attempting to update level text");
        expUpdater.UpdateLevel(level);
    }
}
