using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    public GameObject ResultsPanel;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI ScoreSummaryText;
    public Button PlayAgainButton;
    public Button MainMenuButton;

    private void Awake()
    {
        Debug.Log($"[ResultsUI] Awake on {gameObject.name}");
        Hide();
    }

    public void ShowResults(List<PlayerState> players)
    {
        Debug.Log($"[ResultsUI] ShowResults on {gameObject.name}");

        if (ResultsPanel != null)
        {
            Debug.Log($"[ResultsUI] Activating ResultsPanel: {ResultsPanel.name}");
            ResultsPanel.SetActive(true);
            ResultsPanel.transform.SetAsLastSibling();
            Debug.Log($"[ResultsUI] ResultsPanel activeSelf after SetActive(true): {ResultsPanel.activeSelf}");
        }
        else
        {
            Debug.LogWarning("[ResultsUI] ResultsPanel is not assigned.");
        }

        if (players == null || players.Count == 0)
        {
            Debug.LogWarning("[ResultsUI] ShowResults called with no players.");
            return;
        }

        Debug.Log("[ResultsUI] ShowResults called with " + players.Count + " players.");

        List<PlayerState> orderedPlayers = players
            .OrderByDescending(p => p.TotalScore)
            .ToList();

        PlayerState winner = orderedPlayers[0];

        if (TitleText != null)
            TitleText.text = winner.PlayerName + " Wins!";
        else
            Debug.LogWarning("[ResultsUI] TitleText is not assigned.");

        if (ScoreSummaryText != null)
        {
            ScoreSummaryText.text = "";

            foreach (var player in orderedPlayers)
            {
                ScoreSummaryText.text += player.PlayerName + ": " + player.TotalScore + " (Invasives: " + player.PersistentInvasives.Count + ")\n";
            }

            Debug.Log("[ResultsUI] ScoreSummaryText updated to:\n" + ScoreSummaryText.text);
        }
        else
        {
            Debug.LogWarning("[ResultsUI] ScoreSummaryText is not assigned.");
        }

        if (PlayAgainButton != null)
        {
            PlayAgainButton.onClick.RemoveAllListeners();
            PlayAgainButton.onClick.AddListener(PlayAgain);
        }

        if (MainMenuButton != null)
        {
            MainMenuButton.onClick.RemoveAllListeners();
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void Hide()
    {
        Debug.Log($"[ResultsUI] Hide called on {gameObject.name}");

        if (ResultsPanel != null)
        {
            Debug.Log($"[ResultsUI] Deactivating ResultsPanel: {ResultsPanel.name}");
            ResultsPanel.SetActive(false);
            Debug.Log($"[ResultsUI] ResultsPanel activeSelf after SetActive(false): {ResultsPanel.activeSelf}");
        }
        else
        {
            Debug.LogWarning("[ResultsUI] Hide called but ResultsPanel is not assigned.");
        }
    }

    private void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}