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
    [SerializeField] private RectTransform loadingPanel;
    [SerializeField] private GameObject playerDataContainer;
    [SerializeField] private GameObject scoreContainer;

    void OnEnable()
    {
        GameManager.GameStateChanged += UpdateState;
        GameManager.ScoreUpdated += UpdateScore;
        GameManager.DoneLoading += ShowLoadingSubtext;
        PlayerController.GainedExperience += UpdateExpBar;
        PlayerController.HealthChanged += UpdateHealthBar;
        PlayerController.LevelUp += UpdateLevelText;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= UpdateState;
        GameManager.ScoreUpdated -= UpdateScore;
        GameManager.DoneLoading -= ShowLoadingSubtext;
        PlayerController.GainedExperience -= UpdateExpBar;
        PlayerController.HealthChanged -= UpdateHealthBar;
        PlayerController.LevelUp -= UpdateLevelText;
    }

    void UpdateState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Paused:
                ChangeText(mainText, "Paused", true);
                ChangeText(subText, "Press Q to Quit", true);
                break;
            case GameState.GameOver:
                ChangeText(mainText, "Game Over", true);
                ChangeText(subText, "Press R to Try Again", true);
                break;
            case GameState.Loading:
                ChangeText(mainText, "Loading", true);
                ChangeText(subText);
                loadingPanel.gameObject.SetActive(true);
                playerDataContainer.SetActive(value: false);
                scoreContainer.SetActive(false);
                break;
            default:
                ChangeText(mainText);
                ChangeText(subText);
                loadingPanel.gameObject.SetActive(false);
                playerDataContainer.SetActive(true);
                scoreContainer.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Change a text object's status and text. If no parameters are sent the text object will be disabled and blanked
    /// </summary>
    /// <param name="textObject">The object to change</param>
    /// <param name="message">The message to be displayed</param>
    /// <param name="enabled">Status of the object</param>
    void ChangeText(TextMeshProUGUI textObject, string message = "", bool enabled = false)
    {
        textObject.text = message;
        textObject.gameObject.SetActive(enabled);
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

    void ShowLoadingSubtext()
    {
        ChangeText(subText, "Press Enter to play", true);
    }
}
