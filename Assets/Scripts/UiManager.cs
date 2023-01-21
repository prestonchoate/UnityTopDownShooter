using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI subText;
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image expBarFillImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void OnEnable()
    {
        GameManager.GameStateChanged += UpdateState;
        GameManager.ScoreUpdated += UpdateScore;
        PlayerController.GainedExperience += UpdateExpBar;
        PlayerController.HealthChanged += UpdateHealthBar;
        PlayerController.LevelUp += UpdateLevelText;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= UpdateState;
        GameManager.ScoreUpdated -= UpdateScore;
        PlayerController.GainedExperience -= UpdateExpBar;
        PlayerController.HealthChanged -= UpdateHealthBar;
        PlayerController.LevelUp -= UpdateLevelText;
    }

    void UpdateState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Paused:
                mainText.text = "Paused";
                mainText.gameObject.SetActive(true);
                subText.text = "Press Q to Quit";
                subText.gameObject.SetActive(true);
                break;
            case GameStates.GameOver:
                mainText.text = "Game Over";
                mainText.gameObject.SetActive(true);
                subText.text = "Press R to Try Again";
                subText.gameObject.SetActive(true);
                break;
            default:
                mainText.gameObject.SetActive(false);
                subText.gameObject.SetActive(false);
                break;
        }
    }

    void UpdateHealthBar(float adjustment, float currentHp, float maxHp)
    {
        healthBarFillImage.fillAmount = currentHp / maxHp;
    }

    void UpdateExpBar(float fillAmount)
    {
        expBarFillImage.fillAmount = fillAmount;
    }

    void UpdateLevelText(int level)
    {
        levelText.text = level.ToString();
    }

    void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
