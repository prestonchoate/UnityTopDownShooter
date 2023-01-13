using UnityEngine;

public class HealthBarManager : MonoBehaviour
{

    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private Vector3 positionOffset = new Vector3(0.0f, 1.0f, 0.0f);

    private GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.Instantiate(healthBarPrefab, gameObject.transform.position + positionOffset, gameObject.transform.rotation);
        healthBar.transform.SetParent(GameManager.Instance.WorldSpaceCanvas.transform);
        PlayerController.PlayerDamaged += UpdateHealth;
    }

    void OnDisable()
    {
        PlayerController.PlayerDamaged -= UpdateHealth;
    }

    void LateUpdate()
    {
        healthBar.transform.position = gameObject.transform.position + positionOffset;
    }

    void UpdateHealth(float damageTaken)
    {

    }
}
