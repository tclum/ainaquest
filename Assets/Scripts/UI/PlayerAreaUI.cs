using TMPro;
using UnityEngine;

public class PlayerAreaUI : MonoBehaviour
{
    public TextMeshProUGUI PlayerNameText;
    public Transform RevealSlot;
    public Transform FieldPanel;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI InvasiveText;

    public void SetPlayerName(string playerName)
    {
        if (PlayerNameText != null)
            PlayerNameText.text = playerName;
        else
            Debug.LogWarning(gameObject.name + ": PlayerNameText is not assigned.");
    }

    public void SetScore(int score)
    {
        if (ScoreText != null)
            ScoreText.text = "Score: " + score;
        else
            Debug.LogWarning(gameObject.name + ": ScoreText is not assigned.");
    }

    public void SetInvasives(int invasiveCount)
    {
        if (InvasiveText != null)
            InvasiveText.text = "Invasives: " + invasiveCount;
        else
            Debug.LogWarning(gameObject.name + ": InvasiveText is not assigned.");
    }
}