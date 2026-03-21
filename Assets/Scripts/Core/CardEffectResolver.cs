using System.Collections.Generic;

public static class CardEffectResolver
{
    public static void ApplyEffects(PlayerState player, List<PlayerState> allPlayers)
    {
        foreach (var card in player.PlantedThisRound)
        {
            switch (card.EffectType)
            {
                case CardEffectType.Wai:
                    ApplyWai(player);
                    break;

                case CardEffectType.Pakukui:
                    ApplyPakukui(player);
                    break;
            }
        }
    }

    private static void ApplyWai(PlayerState player)
    {
        // Example: +2 per Native
        int bonus = 0;

        foreach (var card in player.PlantedThisRound)
        {
            if (card.CardType == CardType.Native)
                bonus += 2;
        }

        player.TotalScore += bonus;
    }

    private static void ApplyPakukui(PlayerState player)
    {
        // Placeholder (expand later)
        player.TotalScore += 5;
    }
}