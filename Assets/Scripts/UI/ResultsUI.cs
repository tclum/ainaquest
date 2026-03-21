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
        Hide();
    }

    public void ShowResults(List<PlayerState> players)
    {
        if (ResultsPanel != null)
            ResultsPanel.SetActive(true);

        if (players == null || players.Count == 0)
            return;

        List<PlayerState> orderedPlayers = players
            .OrderByDescending(p => p.TotalScore)
            .ToList();

        PlayerState winner = orderedPlayers[0];

        if (TitleText != null)
            TitleText.text = winner.PlayerName + " Wins!";

        if (ScoreSummaryText != null)
        {
            ScoreSummaryText.text = "";


            foreach (var player in orderedPlayers)
            {
                ScoreSummaryText.text += player.PlayerName + ": " + player.TotalScore + " (Invasives: " + player.PersistentInvasives.Count + ")\n";
            }
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
        if (ResultsPanel != null)
            ResultsPanel.SetActive(false);
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