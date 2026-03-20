using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanTurnUI : MonoBehaviour
{
    public Transform HandPanel;
    public GameObject CardButtonPrefab;
    public TextMeshProUGUI SelectedCardText;
    public Button ConfirmButton;
    public TextMeshProUGUI LogText;

    private PlayerState currentPlayer;
    private CardData selectedCard;
    private bool selectionConfirmed = false;

    public void SetupHand(PlayerState player)
    {
        Debug.Log("Setting up hand for " + player.PlayerName + " with " + player.Hand.Count + " cards.");
        currentPlayer = player;
        selectedCard = null;
        selectionConfirmed = false;

        if (SelectedCardText != null)
            SelectedCardText.text = "Selected: None";

        if (ConfirmButton != null)
        {
            ConfirmButton.interactable = false;
            ConfirmButton.onClick.RemoveAllListeners();
            ConfirmButton.onClick.AddListener(ConfirmSelection);
        }

        ClearHandUI();

        foreach (var card in player.Hand)
        {
            Debug.Log("Creating UI card for: " + card.CardName);
            GameObject cardObj = Instantiate(CardButtonPrefab, HandPanel);
            cardObj.SetActive(true);

            CardUIButton ui = cardObj.GetComponent<CardUIButton>();
            ui.Setup(card, this);
        }

        if (LogText != null)
            LogText.text = player.PlayerName + ", choose a card.";
    }

    public void SelectCard(CardData card)
    {
        Debug.Log("Clicked card: " + card.CardName);

        selectedCard = card;

        if (SelectedCardText != null)
            SelectedCardText.text = "Selected: " + card.CardName;

        if (ConfirmButton != null)
            ConfirmButton.interactable = true;
    }

    public void ConfirmSelection()
    {
        if (selectedCard == null) return;

        Debug.Log("Confirmed card: " + selectedCard.CardName);

        selectionConfirmed = true;

        if (LogText != null)
            LogText.text = currentPlayer.PlayerName + " locked in " + selectedCard.CardName;
    }

    public bool IsSelectionConfirmed()
    {
        return selectionConfirmed;
    }

    public List<CardData> GetConfirmedSelection()
    {
        if (selectedCard == null)
            return new List<CardData>();

        return new List<CardData> { selectedCard };
    }

    public void ClearHandUI()
    {
        for (int i = HandPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(HandPanel.GetChild(i).gameObject);
        }
    }

    public void AddLog(string message)
    {
        if (LogText != null)
            LogText.text = message;
    }
}