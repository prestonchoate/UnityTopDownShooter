using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarUpdater : MonoBehaviour
{
    public bool HasText { get; set; }

    private Image fillImage;
    private TextMeshProUGUI levelText;

    void Start()
    {
        fillImage = this.GetComponent<Image>();
        if (HasText)
        {
            levelText = this.GetComponentInChildren<TextMeshProUGUI>();
            if (levelText is null)
            {
                Debug.Log("Could not find level text object");

            }
        }
    }

    public void UpdateBar(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }

    public void UpdateLevel(int level)
    {
        if (HasText && levelText is not null)
        {
            levelText.text = level.ToString();
        }
    }
}
