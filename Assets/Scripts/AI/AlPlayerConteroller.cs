using System.Collections.Generic;
using System.Linq;

public static class AIPlayerController
{
    public static List<CardData> ChooseCardsToPlant(PlayerState player)
    {
        int plantCount = player.CardsToPlantThisTurn();

        var ordered = player.Hand
            .OrderBy(c => c.CardType == CardType.Invasive ? 1 : 0)
            .ThenByDescending(c => c.BasePoints)
            .ToList();

        return ordered.Take(plantCount).ToList();
    }
}