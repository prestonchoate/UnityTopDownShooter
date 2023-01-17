using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI mainText;

    void OnEnable()
    {
        GameManager.GameStateChanged += UpdateState;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= UpdateState;
    }

    void UpdateState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Paused:
                mainText.text = "Paused";
                mainText.gameObject.SetActive(true);
                break;
            case GameStates.Playing:
                mainText.gameObject.SetActive(false);
                break;
            case GameStates.GameOver:
                mainText.text = "Game Over";
                mainText.gameObject.SetActive(true);
                break;
            default:
                mainText.gameObject.SetActive(false);
                break;
        }
    }
}
