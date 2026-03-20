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

    public Dictionary<int, List<CardData>> GetSelections(List<PlayerState> players)
    {
        Dictionary<int, List<CardData>> selections = new Dictionary<int, List<CardData>>();

        foreach (var player in players)
        {
            List<CardData> chosen;

            if (player.IsHuman)
            {
                chosen = ChooseHumanCards(player);
            }
            else
            {
                chosen = AIPlayerController.ChooseCardsToPlant(player);
            }

            selections[player.PlayerId] = chosen;
        }

        return selections;
    }

    public void RevealAndResolve(List<PlayerState> players, Dictionary<int, List<CardData>> selections)
    {
        foreach (var player in players)
        {
            List<CardData> chosen = selections[player.PlayerId];

            foreach (var card in chosen)
            {
                player.Hand.Remove(card);

                if (card.CardType == CardType.Invasive)
                {
                    player.PersistentInvasives.Add(card);
                }

                player.PlantedThisRound.Add(card);

                if (card.EffectType == CardEffectType.Wai)
                {
                    player.PendingExtraPlants = 1;
                }

                if (card.EffectType == CardEffectType.Pakukui)
                {
                    player.HasPakukui = true;
                }

                Debug.Log(player.PlayerName + " planted " + card.CardName);
            }
        }

        foreach (var player in players)
        {
            player.ConsumeTurnBonuses();
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

    public void ScoreRound(List<PlayerState> players)
    {
        foreach (var player in players)
        {
            int roundScore = ScoringManager.CalculateRoundScore(player);
            player.TotalScore += roundScore;
            Debug.Log(player.PlayerName + " scored " + roundScore + " this round. Total: " + player.TotalScore);
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

    private List<CardData> ChooseHumanCards(PlayerState player)
    {
        int count = Mathf.Min(player.CardsToPlantThisTurn(), player.Hand.Count);
        return player.Hand.Take(count).ToList();
    }
}