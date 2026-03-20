using System.Collections.Generic;

[System.Serializable]
public class PlayerState
{
    public int PlayerId;
    public string PlayerName;
    public bool IsHuman;

    public List<CardData> Hand = new List<CardData>();
    public List<CardData> PlantedThisRound = new List<CardData>();
    public List<CardData> PersistentInvasives = new List<CardData>();

    public int TotalScore = 0;
    public int PendingExtraPlants = 0;
    public bool HasPakukui = false;

    public PlayerState(int id, string name, bool isHuman)
    {
        PlayerId = id;
        PlayerName = name;
        IsHuman = isHuman;
    }

    public int CardsToPlantThisTurn()
    {
        return 1 + PendingExtraPlants;
    }

    public void ConsumeTurnBonuses()
    {
        PendingExtraPlants = 0;
    }

    public void ResetRoundData()
    {
        Hand.Clear();
        PlantedThisRound.Clear();
        HasPakukui = false;
        PendingExtraPlants = 0;
    }
}