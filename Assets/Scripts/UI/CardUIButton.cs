using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUIButton : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public Button Button;
    public Image Background;

    private CardData cardData;
    private HumanTurnUI humanTurnUI;

    public CardData GetCardData()
    {
        return cardData;
    }

    public void Setup(CardData card, HumanTurnUI ui)
    {
        cardData = card;
        humanTurnUI = ui;

        if (Label != null)
        {
            Label.text = card.CardName + "\n" + card.BasePoints + " pts";
        }

        SetSelected(false);

        if (Button != null)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(OnClicked);
        }
    }

    public void SetSelected(bool selected)
    {
        if (Background != null)
        {
            Background.color = Color.green;
        }
    }

    private void OnClicked()
    {
        if (humanTurnUI != null && cardData != null)
        {
            humanTurnUI.SelectCard(cardData);
        }
    }
}