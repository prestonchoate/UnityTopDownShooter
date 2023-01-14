using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarUpdater : MonoBehaviour
{
    public bool HasText { get; set; }

    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private GameObject levelTextContainer;
    private TextMeshPro levelText;

    void Start()
    {
        levelText = levelTextContainer.GetComponent<TextMeshPro>();
        // fillImage = gameObject.GetComponent<Image>();
        // if (HasText)
        // {
        //     levelText = gameObject.GetComponentInChildren<TextMeshPro>();
        //     if (levelText is null)
        //     {
        //         Debug.Log("Could not find level text object");
        //     }
        // }
    }

    public void UpdateBar(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }

    public void UpdateLevel(int level)
    {
        if (HasText && levelText is not null)
        {
            Debug.Log($"Level is {level}");
            // Update Text to level
            levelText.text = level.ToString();
        }
    }
}
