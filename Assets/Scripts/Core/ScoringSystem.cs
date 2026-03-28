using System.Collections.Generic;
using UnityEngine;

public static class ScoringSystem
{
    public static int CalculateRoundScore(PlayerState player)
    {
        int score = 0;

        foreach (var card in player.PlantedThisRound)
        {
            if (card.CardType != CardType.Invasive)
            {
                score += card.BasePoints;
            }
        }

        player.TotalScore += score;
        return score;
    }

    public static void ApplyFinalInvasivePenalty(List<PlayerState> players)
    {
        int maxInvasives = 0;

        foreach (var player in players)
        {
            if (player.PersistentInvasives.Count > maxInvasives)
            {
                maxInvasives = player.PersistentInvasives.Count;
            }
        }

        Debug.Log("Max invasives found: " + maxInvasives);

        if (maxInvasives <= 0) return;

        foreach (var player in players)
        {
            Debug.Log(player.PlayerName + " invasives: " + player.PersistentInvasives.Count);

            if (player.PersistentInvasives.Count == maxInvasives)
            {
                player.TotalScore -= 20;
                Debug.Log(player.PlayerName + " received invasive penalty. New score: " + player.TotalScore);
            }
        }
    }
}