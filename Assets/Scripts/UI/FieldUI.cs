using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI : MonoBehaviour
{
    public Transform PlayerFieldPanel;
    public Transform NpcFieldPanel;

    public TextMeshProUGUI PlayerScoreText;
    public TextMeshProUGUI NpcScoreText;

    public GameObject CardButtonPrefab;

    private readonly List<GameObject> playerFieldCards = new List<GameObject>();
    private readonly List<GameObject> npcFieldCards = new List<GameObject>();

    public void ResetFieldUI()
    {
        ClearCardList(playerFieldCards);
        ClearCardList(npcFieldCards);

        UpdateScoreTexts(0, 0);
    }

    public void AddPlantedCards(List<PlayerState> players, Dictionary<int, List<CardData>> selections)
    {
        Debug.Log("AddPlantedCards called");
        foreach (var player in players)
        {
            if (!selections.ContainsKey(player.PlayerId)) continue;
            if (selections[player.PlayerId] == null || selections[player.PlayerId].Count == 0) continue;

            foreach (var card in selections[player.PlayerId])
            {
                if (player.IsHuman)
                {
                    SpawnCardInField(card, PlayerFieldPanel, playerFieldCards);
                }
                else
                {
                    SpawnCardInField(card, NpcFieldPanel, npcFieldCards);
                }
            }
        }

        UpdateRunningScores(players);
    }

    private void SpawnCardInField(CardData card, Transform parent, List<GameObject> trackingList)
    {
        GameObject cardGO = Instantiate(CardButtonPrefab, parent);
        cardGO.SetActive(true);
        Debug.Log("Spawned field card: " + card.CardName + " into " + parent.name);

        RectTransform rt = cardGO.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
            rt.anchoredPosition = Vector2.zero;
        }

        CardUIButton cardUI = cardGO.GetComponent<CardUIButton>();
        if (cardUI != null && cardUI.Label != null)
        {
            cardUI.Label.text = card.CardName + "\n" + card.BasePoints + " pts";
        }

        Button button = cardGO.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        trackingList.Add(cardGO);
    }

    private void UpdateRunningScores(List<PlayerState> players)
    {
        int playerScore = 0;
        int npcScore = 0;

        foreach (var player in players)
        {
            int runningScore = CalculateVisibleRunningScore(player);

            if (player.IsHuman)
            {
                playerScore = runningScore;
            }
            else
            {
                npcScore = runningScore;
            }
        }

        UpdateScoreTexts(playerScore, npcScore);
    }

    private int CalculateVisibleRunningScore(PlayerState player)
    {
        int score = 0;

        foreach (var card in player.PlantedThisRound)
        {
            if (card.CardType != CardType.Invasive)
            {
                score += card.BasePoints;
            }
        }

        return score;
    }

    private void UpdateScoreTexts(int playerScore, int npcScore)
    {
        if (PlayerScoreText != null)
            PlayerScoreText.text = "Score: " + playerScore;

        if (NpcScoreText != null)
            NpcScoreText.text = "Score: " + npcScore;
    }

    private void ClearCardList(List<GameObject> cardList)
    {
        foreach (var obj in cardList)
        {
            if (obj != null)
                Destroy(obj);
        }

        cardList.Clear();
    }
}