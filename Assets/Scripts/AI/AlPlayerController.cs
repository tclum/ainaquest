using System.Collections.Generic;
using UnityEngine;

public static class AIPlayerController
{
    public static List<CardData> ChooseCardsToPlant(PlayerState player)
    {
        List<CardData> chosen = new List<CardData>();

        if (player.Hand == null || player.Hand.Count == 0)
            return chosen;

        int randomIndex = Random.Range(0, player.Hand.Count);
        chosen.Add(player.Hand[randomIndex]);

        return chosen;
    }
}