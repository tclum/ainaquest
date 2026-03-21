using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnTransitionUI : MonoBehaviour
{
    public GameObject Panel;
    public TextMeshProUGUI MessageText;
    public Button ContinueButton;

    private bool continuePressed = false;

    private void Awake()
    {
        HideImmediate();
    }

    public void ShowMessage(string message)
    {
        continuePressed = false;

        if (Panel != null)
            Panel.SetActive(true);

        if (MessageText != null)
            MessageText.text = message;

        if (ContinueButton != null)
        {
            ContinueButton.onClick.RemoveAllListeners();
            ContinueButton.onClick.AddListener(OnContinuePressed);
        }
    }

    public bool IsContinuePressed()
    {
        return continuePressed;
    }

    public void HideImmediate()
    {
        continuePressed = false;

        if (Panel != null)
            Panel.SetActive(false);
    }

    private void OnContinuePressed()
    {
        continuePressed = true;

        if (Panel != null)
            Panel.SetActive(false);
    }
}