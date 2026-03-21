using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject PausePanel;

    private bool isPaused = false;

    private void Awake()
    {
        Hide();
    }

    public void TogglePause()
    {
        if (isPaused)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        isPaused = true;

        if (PausePanel != null)
            PausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        isPaused = false;

        if (PausePanel != null)
            PausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void ContinueGame()
    {
        Hide();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}