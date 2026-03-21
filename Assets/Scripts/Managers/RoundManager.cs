using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public List<CardData> AllCards = new List<CardData>();

    public void SetupRound(List<PlayerState> players)
    {
        foreach (var player in players)
        {
            player.ResetRoundData();
        }

        List<CardData> deck = GenerateDeck();
        Shuffle(deck);
        DealCards(players, deck, 9);
    }

    public bool IsRoundOver(List<PlayerState> players)
    {
        return players.All(p => p.Hand.Count == 0);
    }

    public void HandleAITurns(List<PlayerState> players, Dictionary<int, List<CardData>> selections)
    {
        foreach (var player in players)
        {
            if (player.IsHuman) continue;

            selections[player.PlayerId] = AIPlayerController.ChooseCardsToPlant(player);
        }
    }

    public void RevealAndResolve(List<PlayerState> players, Dictionary<int, List<CardData>> selections)
    {
        foreach (var player in players)
        {
            if (!selections.ContainsKey(player.PlayerId)) continue;
            if (selections[player.PlayerId] == null) continue;

            foreach (var card in selections[player.PlayerId])
            {
                player.Hand.Remove(card);
                if (card.CardType == CardType.Invasive)
                {
                    player.PersistentInvasives.Add(card);
                }
                player.PlantedThisRound.Add(card);

                Debug.Log(player.PlayerName + " planted " + card.CardName);
            }
        }
    }

    public void RotateHandsLeft(List<PlayerState> players)
    {
        List<List<CardData>> oldHands = players.Select(p => new List<CardData>(p.Hand)).ToList();

        for (int i = 0; i < players.Count; i++)
        {
            int fromIndex = (i + 1) % players.Count;
            players[i].Hand = oldHands[fromIndex];
        }
    }

    private List<CardData> GenerateDeck()
    {
        List<CardData> deck = new List<CardData>();

        for (int i = 0; i < 6; i++)
        {
            deck.AddRange(AllCards);
        }

        return deck;
    }

    private void DealCards(List<PlayerState> players, List<CardData> deck, int cardsPerPlayer)
    {
        for (int c = 0; c < cardsPerPlayer; c++)
        {
            foreach (var player in players)
            {
                if (deck.Count == 0) return;

                player.Hand.Add(deck[0]);
                deck.RemoveAt(0);
            }
        }
    }

    private void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            CardData temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}