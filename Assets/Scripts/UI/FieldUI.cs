using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldUI : MonoBehaviour
{
    public List<PlayerAreaUI> PlayerAreas = new List<PlayerAreaUI>();
    public GameObject CardButtonPrefab;

    private readonly Dictionary<int, List<GameObject>> spawnedFieldCards = new Dictionary<int, List<GameObject>>();

    public void Initialize(List<PlayerState> players)
    {
        for (int i = 0; i < PlayerAreas.Count; i++)
        {
            bool active = i < players.Count;
            PlayerAreas[i].gameObject.SetActive(active);

            if (active)
            {
                PlayerAreas[i].SetPlayerName(players[i].PlayerName);
                PlayerAreas[i].SetScore(players[i].TotalScore);
                PlayerAreas[i].SetInvasives(players[i].PersistentInvasives.Count);
            }
        }
    }

    public void ResetFieldUI(List<PlayerState> players)
    {
        foreach (var pair in spawnedFieldCards)
        {
            foreach (var obj in pair.Value)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        spawnedFieldCards.Clear();
        UpdateRunningDisplays(players);
    }

    public void AddPlantedCards(List<PlayerState> players, Dictionary<int, List<CardData>> selections)
    {
        for (int i = 0; i < players.Count && i < PlayerAreas.Count; i++)
        {
            PlayerState player = players[i];

            if (!selections.ContainsKey(player.PlayerId)) continue;
            if (selections[player.PlayerId] == null || selections[player.PlayerId].Count == 0) continue;

            if (!spawnedFieldCards.ContainsKey(player.PlayerId))
                spawnedFieldCards[player.PlayerId] = new List<GameObject>();

            foreach (var card in selections[player.PlayerId])
            {
                SpawnCardInField(card, PlayerAreas[i].FieldPanel, spawnedFieldCards[player.PlayerId]);
            }
        }

        UpdateRunningDisplays(players);
    }

    private void SpawnCardInField(CardData card, Transform parent, List<GameObject> trackingList)
    {
        if (parent == null)
        {
            Debug.LogError("SpawnCardInField failed: parent is null for card " + card.CardName);
            return;
        }

        Debug.Log("Spawning field card " + card.CardName + " into " + parent.name);

        GameObject cardGO = Instantiate(CardButtonPrefab, parent);
        cardGO.SetActive(true);

        RectTransform rt = cardGO.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
            rt.anchoredPosition = Vector2.zero;
        }

        CardUIButton cardUI = cardGO.GetComponent<CardUIButton>();
        if (cardUI != null)
        {
            cardUI.Setup(card, null); // Pass null for HumanTurnUI to indicate non-interactive field card
        }
        else
        {
            Debug.LogWarning("CardUIButton missing on spawned field card prefab.");
        }

        Button button = cardGO.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        trackingList.Add(cardGO);
    }

    private void UpdateRunningDisplays(List<PlayerState> players)
    {
        for (int i = 0; i < players.Count && i < PlayerAreas.Count; i++)
        {
            int visibleScore = players[i].TotalScore + ScoringSystem.CalculateRoundScore(players[i]);

            Debug.Log("Updating score for " + players[i].PlayerName + " to " + visibleScore);
            Debug.Log("Updating invasives for " + players[i].PlayerName + " to " + players[i].PersistentInvasives.Count);

            if (PlayerAreas[i] != null)
            {
                PlayerAreas[i].SetScore(visibleScore);
                PlayerAreas[i].SetInvasives(players[i].PersistentInvasives.Count);
            }
            else
            {
                Debug.LogError("PlayerAreas[" + i + "] is null.");
            }
        }
    }
}
