using System.Collections;
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
    private bool selectionConfirmed;

    private List<CardUIButton> cardButtons = new List<CardUIButton>();

    public IEnumerator HandleTurn(PlayerState player, Dictionary<int, List<CardData>> selections)
    {
        SetupHand(player);
        yield return new WaitUntil(() => IsSelectionConfirmed());
        selections[player.PlayerId] = GetConfirmedSelection();
    }

    public void SetupHand(PlayerState player)
    {
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
            GameObject cardObj = Instantiate(CardButtonPrefab, HandPanel);
            cardObj.SetActive(true);

            RectTransform rt = cardObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.localRotation = Quaternion.identity;
                rt.anchoredPosition = Vector2.zero;
            }

            CardUIButton ui = cardObj.GetComponent<CardUIButton>();
            if (ui != null)
            {
                ui.Setup(card, this);
                cardButtons.Add(ui);
            }
        }

        if (LogText != null)
            LogText.text = player.PlayerName + ", choose a card.";
    }

    public void SelectCard(CardData card)
    {
        if (card == null) return;

        selectedCard = card;

        for (int i = cardButtons.Count - 1; i >= 0; i--)
        {
            if (cardButtons[i] == null)
            {
                cardButtons.RemoveAt(i);
                continue;
            }

            bool isSelected = cardButtons[i].GetCardData() == card;
            cardButtons[i].SetSelected(isSelected);
        }

        if (SelectedCardText != null)
            SelectedCardText.text = "Selected: " + card.CardName;

        if (ConfirmButton != null)
            ConfirmButton.interactable = true;
    }

    public void ConfirmSelection()
    {
        if (selectedCard == null)
            return;

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
        List<CardData> result = new List<CardData>();

        if (selectedCard != null)
            result.Add(selectedCard);

        return result;
    }

    public void ClearHandUI()
    {
        cardButtons.Clear();

        for (int i = HandPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(HandPanel.GetChild(i).gameObject);
        }
    }

    public void HideHandUI()
    {
        ClearHandUI();

        if (SelectedCardText != null)
            SelectedCardText.text = "Selected: None";

        if (LogText != null)
            LogText.text = "";

        if (ConfirmButton != null)
            ConfirmButton.interactable = false;
    }

    public void AddLog(string message)
    {
        if (LogText != null)
            LogText.text = message;
    }
}