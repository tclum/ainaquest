using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevealUI : MonoBehaviour
{
    public GameObject RevealPanel;
    public Transform PlayerRevealSlot;
    public Transform NpcRevealSlot;
    public GameObject CardButtonPrefab;

    private readonly List<GameObject> spawnedCards = new List<GameObject>();

    public void ClearReveal()
    {
        foreach (var obj in spawnedCards)
        {
            if (obj != null)
                Destroy(obj);
        }

        spawnedCards.Clear();
    }

    public IEnumerator ShowReveal(List<PlayerState> players, Dictionary<int, List<CardData>> selections)
    {
        RevealPanel.SetActive(true);
        ClearReveal();

        foreach (var player in players)
        {
            if (!selections.ContainsKey(player.PlayerId)) continue;
            if (selections[player.PlayerId] == null || selections[player.PlayerId].Count == 0) continue;

            CardData card = selections[player.PlayerId][0];
            Transform parent = player.IsHuman ? PlayerRevealSlot : NpcRevealSlot;

            GameObject cardGO = Instantiate(CardButtonPrefab, parent);
            cardGO.SetActive(true);

            // Reset transform so UI layout is predictable
            RectTransform rt = cardGO.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                rt.anchoredPosition = Vector2.zero;
                rt.localRotation = Quaternion.identity;
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

            Debug.Log("Reveal card spawned for " + player.PlayerName + ": " + card.CardName);
        }

        // Wait before flipping (suspense)
        yield return new WaitForSeconds(0.5f);

        // Flip each card
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            GameObject obj = spawnedCards[i];
            PlayerState player = players[i];
            CardData card = selections[player.PlayerId][0];

            yield return StartCoroutine(FlipCard(obj, card));
        }

        // Wait after reveal
        yield return new WaitForSeconds(1f);

        // Clear
        ClearReveal();
        RevealPanel.SetActive(false);
    }

    private IEnumerator FlipCard(GameObject cardGO, CardData card)
    {
        RectTransform rt = cardGO.GetComponent<RectTransform>();
        CardUIButton cardUI = cardGO.GetComponent<CardUIButton>();

        float duration = 0.2f;

        // shrink
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float scaleX = Mathf.Lerp(1f, 0f, t / duration);
            rt.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }

        // swap content
        if (cardUI != null && cardUI.Label != null)
        {
            cardUI.Label.text = card.CardName + "\n" + card.BasePoints + " pts";
        }

        // expand
        t = 0;
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