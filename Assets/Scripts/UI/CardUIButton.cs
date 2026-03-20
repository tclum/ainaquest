using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUIButton : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public Button Button;

    private CardData cardData;
    private HumanTurnUI humanTurnUI;

    public void Setup(CardData card, HumanTurnUI ui)
    {
        cardData = card;
        humanTurnUI = ui;

        if (Label != null)
        {
            Label.text = card.CardName + "\n" + card.BasePoints + " pts";
        }

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        humanTurnUI.SelectCard(cardData);
    }
}