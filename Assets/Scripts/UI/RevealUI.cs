using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevealUI : MonoBehaviour
{
    public GameObject RevealPanel;
    public List<PlayerAreaUI> PlayerAreas = new List<PlayerAreaUI>();
    public GameObject CardButtonPrefab;

    private readonly List<GameObject> spawnedCards = new List<GameObject>();
    private readonly List<CardData> spawnedCardData = new List<CardData>();

    public void Initialize(List<PlayerState> players)
    {
        for (int i = 0; i < PlayerAreas.Count; i++)
        {
            bool active = i < players.Count;
            PlayerAreas[i].gameObject.SetActive(active);
        }
    }

    public void ClearReveal()
    {
        foreach (var obj in spawnedCards)
        {
            if (obj != null)
                Destroy(obj);
        }

        spawnedCards.Clear();
        spawnedCardData.Clear();
    }

    public IEnumerator ShowReveal(List<PlayerState> players, Dictionary<int, List<CardData>> selections)
    {
        if (RevealPanel == null)
        {
            Debug.LogError("RevealUI.ShowReveal failed: RevealPanel is not assigned.");
            yield break;
        }

        RevealPanel.SetActive(true);
        ClearReveal();

        for (int i = 0; i < players.Count && i < PlayerAreas.Count; i++)
        {
            PlayerState player = players[i];

            if (!selections.ContainsKey(player.PlayerId)) continue;
            if (selections[player.PlayerId] == null || selections[player.PlayerId].Count == 0) continue;

            CardData card = selections[player.PlayerId][0];
            Transform parent = PlayerAreas[i].RevealSlot;

            GameObject cardGO = Instantiate(CardButtonPrefab, parent);
            cardGO.SetActive(true);

            RectTransform rt = cardGO.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.localRotation = Quaternion.identity;
                rt.anchoredPosition = Vector2.zero;
            }

            CardUIButton cardUI = cardGO.GetComponent<CardUIButton>();
            if (cardUI != null && cardUI.Label != null)
            {
                cardUI.Label.text = "???";
            }

            Button button = cardGO.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = false;
            }

            spawnedCards.Add(cardGO);
            spawnedCardData.Add(card);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < spawnedCards.Count; i++)
        {
            yield return StartCoroutine(FlipCard(spawnedCards[i], spawnedCardData[i]));
        }

        yield return new WaitForSeconds(1f);

        ClearReveal();
        RevealPanel.SetActive(false);
    }

    private IEnumerator FlipCard(GameObject cardGO, CardData card)
    {
        RectTransform rt = cardGO.GetComponent<RectTransform>();
        CardUIButton cardUI = cardGO.GetComponent<CardUIButton>();

        float duration = 0.2f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float scaleX = Mathf.Lerp(1f, 0f, t / duration);
            rt.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }

        if (cardUI != null && cardUI.Label != null)
        {
            cardUI.Label.text = card.CardName + "\n" + card.BasePoints + " pts";
        }

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float scaleX = Mathf.Lerp(0f, 1f, t / duration);
            rt.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }

        rt.localScale = Vector3.one;
    }
}