using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI subText;

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
            case GameStates.GameOver:
                mainText.text = "Game Over";
                mainText.gameObject.SetActive(true);
                // TODO: Fix positioning of the sub text
                subText.text = "Press R to Try Again";
                subText.gameObject.SetActive(true);
                break;
            default:
                mainText.gameObject.SetActive(false);
                subText.gameObject.SetActive(false);
                break;
        }
    }
}
