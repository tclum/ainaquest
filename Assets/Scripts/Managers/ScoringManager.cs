using System.Collections.Generic;
using System.Linq;

public static class ScoringManager
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

        Dictionary<CardColorCategory, int> colorCounts = new Dictionary<CardColorCategory, int>();

        foreach (var card in player.PlantedThisRound)
        {
            if (card.ColorCategory == CardColorCategory.None) continue;

            if (!colorCounts.ContainsKey(card.ColorCategory))
                colorCounts[card.ColorCategory] = 0;

            colorCounts[card.ColorCategory]++;
        }

        foreach (var pair in colorCounts)
        {
            int count = pair.Value;

            if (count >= 5) score += 12;
            else if (count >= 4) score += 8;
            else if (count >= 3) score += 5;
        }

        return score;
    }

    public static void ApplyFinalInvasivePenalty(List<PlayerState> players)
    {
        int maxInvasives = players.Max(p => p.PersistentInvasives.Count);
        if (maxInvasives <= 0) return;

        foreach (var player in players)
        {
            if (player.PersistentInvasives.Count == maxInvasives)
            {
                player.TotalScore -= 20;
            }
        }
    }
}