using System.Collections.Generic;

[System.Serializable]
public class PlayerState
{
    public int PlayerId;
    public string PlayerName;
    public bool IsHuman;

    public int TotalScore;

    public int PendingExtraPlants = 0;
    public bool HasPakukui = false;

    public List<CardData> Hand = new List<CardData>();
    public List<CardData> PlantedThisRound = new List<CardData>();
    public List<CardData> PersistentInvasives = new List<CardData>();

    public PlayerState(int id, string name, bool isHuman)
    {
        PlayerId = id;
        PlayerName = name;
        IsHuman = isHuman;
        TotalScore = 0;
    }

    public void ResetRoundData()
    {
        Hand.Clear();
        PlantedThisRound.Clear();
    }

    // Syncs the score calculation with ScoringSystem
    public void SyncScoreFromPlantedThisRound()
    {
        int score = 0;
        foreach (var card in PlantedThisRound)
        {
            if (card.CardType != CardType.Invasive)
            {
                score += card.BasePoints;
            }
        }
        TotalScore = score;
    }
}
