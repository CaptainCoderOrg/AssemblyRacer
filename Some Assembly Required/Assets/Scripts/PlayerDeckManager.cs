using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            yield return new WaitForSeconds(DrawCard());
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
    public float DrawCard()
    {
        if (PlayerDeck.Count == 0)
        {
            float delay = ShuffleDiscard();
            StartCoroutine(DelayDraw(delay));
            return delay;
        }
        // TODO: Display empty deck message?
        if (PlayerDeck.Count == 0) { return 0.15f; }
        CardData drawn = PlayerDeck[PlayerDeck.Count - 1];
        PlayerDeck.RemoveAt(PlayerDeck.Count - 1);
        OnDeckSizeChange.Invoke(PlayerDeck.Count);
        OnDeckSizeChangeString.Invoke(PlayerDeck.Count.ToString());
        CardController card = _handController.DrawCard(drawn);
        return 0.15f;
    }

    public float ShuffleDiscard()
    {
        DiscardPile.Clear();
        int delay = 0;

        foreach (CardController card in _handController.Discarded)
        {
            CardData data = card.Card;
            PlayerDeck.Add(data);
            int count = PlayerDeck.Count;
            StartCoroutine(_handController.AnimateShuffle(card, delay++, () =>
            {
                OnDeckSizeChange.Invoke(count);
                OnDeckSizeChangeString.Invoke(count.ToString());
            }));
        }
        _handController.Discarded.Clear();
        return delay * _handController.AnimationDuration  + 0.25f;
    }

    private IEnumerator DelayDraw(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerDeck.Shuffle();
        DrawCard();
    }
}
