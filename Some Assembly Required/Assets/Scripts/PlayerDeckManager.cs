using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeckManager : MonoBehaviour
{
    public UnityEvent<int> OnDeckSizeChange;
    public UnityEvent<string> OnDeckSizeChangeString;
    [SerializeField]
    private HandController _handController;
    [SerializeField]
    private StarterDeckData _starterDeck;
    [field: SerializeField]
    public List<CardData> PlayerDeck { get; private set; }
    [field: SerializeField]
    public List<CardData> DiscardPile { get; private set; }
    [SerializeField]
    private Transform _discardPileLocation;
    


    void Start()
    {
        DiscardPile = new List<CardData>();
        InitializePlayerDeck();
    }

    public void StartTurn() => StartCoroutine(DrawCards(6));

    public IEnumerator DrawCards(int count)
    {
        for (int ix = 0; ix < count; ix++)
        {
            DrawCard();
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void DiscardHand()
    {
        DiscardPile.AddRange(_handController.DiscardHand(_discardPileLocation).Select(card => card.Card));
    }

    private void InitializePlayerDeck()
    {
        PlayerDeck = new List<CardData>();
        foreach (StarterDeckData.Entry entry in _starterDeck.Cards)
        {
            for (int i = 0; i < entry.Count; i++)
            {
                PlayerDeck.Add(entry.Card);
            }
        }
        PlayerDeck.Shuffle();
        OnDeckSizeChange.Invoke(PlayerDeck.Count);
        OnDeckSizeChangeString.Invoke(PlayerDeck.Count.ToString());
    }
    public void DrawCard()
    {
        CardData drawn = PlayerDeck[PlayerDeck.Count - 1];
        PlayerDeck.RemoveAt(PlayerDeck.Count - 1);
        OnDeckSizeChange.Invoke(PlayerDeck.Count);
        OnDeckSizeChangeString.Invoke(PlayerDeck.Count.ToString());
        CardController card = _handController.DrawCard(drawn);
    }
}
