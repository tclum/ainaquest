using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public RoundManager RoundManager;
    public HumanTurnUI HumanTurnUI;
    public RevealUI RevealUI;
    public FieldUI FieldUI;
    public ResultsUI ResultsUI;
    public PauseMenuUI PauseMenuUI;
    public TurnTransitionUI TurnTransitionUI;

    public List<PlayerState> Players = new List<PlayerState>();
    public int playerCount = 1;

    private int currentRound = 1;
    private const int maxRounds = 1;

    private GameState currentState = GameState.Playing;

    private void Start()
    {
        currentState = GameState.Playing;
        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.GameOver)
                return;

            if (PauseMenuUI != null)
            {
                PauseMenuUI.TogglePause();
                currentState = PauseMenuUI.IsPaused() ? GameState.Paused : GameState.Playing;
            }
        }
    }

    private void CreatePlayers()
    {
        Players.Clear();

        Debug.Log("CreatePlayers called. SoloMode=" + GameSettings.SoloMode + ", SelectedPlayerCount=" + GameSettings.SelectedPlayerCount);

        int totalPlayers = GameSettings.SoloMode ? 2 : GameSettings.SelectedPlayerCount;

        for (int i = 0; i < totalPlayers; i++)
        {
            bool isHuman = !(GameSettings.SoloMode && i == 1);
            string playerName = isHuman ? "Player " + (i + 1) : "NPC";

            Players.Add(new PlayerState(i, playerName, isHuman));
            Debug.Log("Created player: " + playerName + ", isHuman=" + isHuman);
        }
    }

    private IEnumerator WaitWhilePaused()
    {
        yield return new WaitUntil(() => currentState != GameState.Paused);
    }

    private bool ShouldShowLocalHandoff()
    {
        return !GameSettings.SoloMode && GameSettings.SelectedPlayerCount > 1;
    }

    private IEnumerator ShowTransitionMessage(string message)
    {
        if (TurnTransitionUI == null)
            yield break;

        TurnTransitionUI.ShowMessage(message);
        yield return new WaitUntil(() => TurnTransitionUI.IsContinuePressed());
    }

    private IEnumerator GameLoop()
    {
        CreatePlayers();

        if (FieldUI != null)
            FieldUI.Initialize(Players);

        if (RevealUI != null)
            RevealUI.Initialize(Players);

        yield return StartCoroutine(
        ShowTransitionMessage("Round " + currentRound)
        );

        while (currentRound <= maxRounds)
        {
            Debug.Log("=== ROUND " + currentRound + " START ===");

            RoundManager.SetupRound(Players);

            if (FieldUI != null)
                FieldUI.ResetFieldUI(Players);

            while (!RoundManager.IsRoundOver(Players))
            {
                if (currentState == GameState.Paused)
                    yield return StartCoroutine(WaitWhilePaused());

                Dictionary<int, List<CardData>> selections = new Dictionary<int, List<CardData>>();

                for (int i = 0; i < Players.Count; i++)
                {
                    PlayerState player = Players[i];
                    if (!player.IsHuman) continue;

                    if (ShouldShowLocalHandoff())
                    {
                        yield return StartCoroutine(
                            ShowTransitionMessage("Pass to " + player.PlayerName + "\nPress Continue when ready")
                        );
                    }

                    yield return StartCoroutine(HumanTurnUI.HandleTurn(player, selections));
                    HumanTurnUI.HideHandUI();
                }

                if (currentState == GameState.Paused)
                    yield return StartCoroutine(WaitWhilePaused());

                RoundManager.HandleAITurns(Players, selections);

                yield return StartCoroutine(RevealUI.ShowReveal(Players, selections));

                RoundManager.RevealAndResolve(Players, selections);

                if (FieldUI != null)
                    FieldUI.AddPlantedCards(Players, selections);

                if (!RoundManager.IsRoundOver(Players))
                {
                    RoundManager.RotateHandsLeft(Players);
                }

                yield return new WaitForSeconds(0.5f);
            }

            ApplyEndOfRoundScores();

            if (FieldUI != null)
                FieldUI.ResetFieldUI(Players);

            Debug.Log("=== ROUND " + currentRound + " END ===");
            currentRound++;

            if (currentRound <= maxRounds)
            {
                if (ShouldShowLocalHandoff())
                {
                    yield return StartCoroutine(
                        ShowTransitionMessage("Round complete\nPress Continue for next round")
                    );
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        currentState = GameState.GameOver;

        ScoringSystem.ApplyFinalInvasivePenalty(Players);

        if (ResultsUI != null)
            ResultsUI.ShowResults(Players);

        Debug.Log("Game complete.");
    }

    private void ApplyEndOfRoundScores()
    {

        foreach (var player in Players)
        {
            CardEffectResolver.ApplyEffects(player, Players);

            int roundScore = ScoringSystem.CalculateRoundScore(player);
            player.TotalScore += roundScore;

            Debug.Log(player.PlayerName + " earned " + roundScore + " this round. Total Score = " + player.TotalScore);
        }
    }
}