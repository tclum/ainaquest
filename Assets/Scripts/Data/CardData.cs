using UnityEngine;

public enum CardType
{
    Native,
    Canoe,
    Invasive,
    Resource
}

public enum CardColorCategory
{
    None,
    Red,
    Blue,
    Green,
    Yellow
}

public enum CardEffectType
{
    None,
    Wai,
    Pakukui
}

[CreateAssetMenu(fileName = "NewCard", menuName = "AinaQuest/Card Data")]
public class CardData : ScriptableObject
{
    public string CardName;
    public CardType CardType;
    public CardColorCategory ColorCategory;
    public int BasePoints;
    public CardEffectType EffectType;
    [TextArea] public string Description;
}