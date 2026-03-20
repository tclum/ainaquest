using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RoundManager RoundManager;
    public HumanTurnUI HumanTurnUI;
    public RevealUI RevealUI;
    public FieldUI FieldUI;

    public List<PlayerState> Players = new List<PlayerState>();

    public int playerCount = 1;
    private int currentRound = 1;
    private const int maxRounds = 3;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CreatePlayers();
        StartCoroutine(GameLoop());
    }

    private void CreatePlayers()
    {
        Debug.Log("CreatePlayers called. SoloMode=" + GameSettings.SoloMode + ", SelectedPlayerCount=" + GameSettings.SelectedPlayerCount);
        Players.Clear();

        int totalPlayers = GameSettings.SoloMode ? 2 : GameSettings.SelectedPlayerCount;

        for (int i = 0; i < totalPlayers; i++)
        {
            bool isHuman = !(GameSettings.SoloMode && i == 1);
            string playerName = isHuman ? "Player " + (i + 1) : "NPC";

            Players.Add(new PlayerState(i, playerName, isHuman));
            Debug.Log("Created player: " + playerName + ", isHuman=" + isHuman);
        }
    }

    private IEnumerator GameLoop()
    {
        while (currentRound <= maxRounds)
        {
            Debug.Log("=== ROUND " + currentRound + " START ===");

            RoundManager.SetupRound(Players);
            FieldUI.ResetFieldUI();

            while (!RoundManager.IsRoundOver(Players))
            {
                Dictionary<int, List<CardData>> selections = new Dictionary<int, List<CardData>>();

                foreach (var player in Players)
                {
                    if (player.IsHuman)
                    {
                        HumanTurnUI.SetupHand(player);
                        yield return new WaitUntil(() => HumanTurnUI.IsSelectionConfirmed());
                        selections[player.PlayerId] = HumanTurnUI.GetConfirmedSelection();
                    }
                    else
                    {
                        selections[player.PlayerId] = AIPlayerController.ChooseCardsToPlant(player);
                    }
                }

                yield return StartCoroutine(RevealUI.ShowReveal(Players, selections));

                RoundManager.RevealAndResolve(Players, selections);
                FieldUI.AddPlantedCards(Players, selections);

                if (!RoundManager.IsRoundOver(Players))
                {
                    RoundManager.RotateHandsLeft(Players);
                }

                yield return new WaitForSeconds(0.5f);
            }

            RoundManager.ScoreRound(Players);

            Debug.Log("=== ROUND " + currentRound + " END ===");
            currentRound++;

            yield return new WaitForSeconds(2f);
        }

        ScoringManager.ApplyFinalInvasivePenalty(Players);

        foreach (var player in Players)
        {
            Debug.Log(player.PlayerName + " final score: " + player.TotalScore + ", invasives: " + player.PersistentInvasives.Count);
        }

        SceneManager.LoadScene("Results");
    }
}