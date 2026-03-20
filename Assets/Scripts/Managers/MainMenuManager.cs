using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartSoloGame()
    {
        GameSettings.SoloMode = true;
        GameSettings.SelectedPlayerCount = 1;
        Debug.Log("Starting SOLO game. SoloMode=" + GameSettings.SoloMode + ", SelectedPlayerCount=" + GameSettings.SelectedPlayerCount);
        SceneManager.LoadScene("Gameplay");
    }

    public void StartMultiplayerGame(int playerCount)
    {
        GameSettings.SoloMode = false;
        GameSettings.SelectedPlayerCount = playerCount;
        SceneManager.LoadScene("Gameplay");
    }
}