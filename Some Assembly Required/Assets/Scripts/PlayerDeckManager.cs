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

    public void StartTurn(System.Action onAnimationComplete) => DrawCards(6, onAnimationComplete);

    public void EnableSelectionMode(System.Action<CardController> OnClick) => _handController.EnableSelectionMode(OnClick);

    public void DisableSelectMode() => _handController.DisableSelectMode();

    public void DrawCards(int count, System.Action onAnimationComplete)
    {
        if (count == 0) 
        { 
            onAnimationComplete.Invoke(); 
            return;
        }
        DrawCard(() =>
        {
            DrawCards(count - 1, onAnimationComplete);
        });
    }

    public void AddCardToDiscard(CardController toAdd)
    {
        DiscardPile.Add(toAdd.Card);
        _handController.Discarded.Add(toAdd);
    }

    public void DiscardCards(List<CardController> cards, System.Action onAnimationComplete)
    {
        if (cards.Count == 0)
        {
            onAnimationComplete.Invoke();
            return;
        }
        DiscardPile.AddRange(cards.Select(card => card.Card));
        _handController.DiscardCards(cards, _discardPileLocation, onAnimationComplete);
    }

    public void DiscardHand(System.Action onAnimationComplete)
    {
        DiscardPile.AddRange(_handController.DiscardHand(_discardPileLocation, onAnimationComplete).Select(card => card.Card));
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
    public void AddCardToHand(CardController card, System.Action onAnimationComplete) => _handController.AddCardToHand(card, onAnimationComplete);

    public void DrawCard(System.Action onAnimationComplete)
    {
        // TODO: Display out of cards message?
        if (PlayerDeck.Count == 0 && DiscardPile.Count == 0) { return; }
        if (PlayerDeck.Count == 0)
        {
            ShuffleDiscard(() =>
            {
                DrawCard(onAnimationComplete);
            });
            return;
        }
        CardData drawn = PlayerDeck[PlayerDeck.Count - 1];
        PlayerDeck.RemoveAt(PlayerDeck.Count - 1);
        OnDeckSizeChange.Invoke(PlayerDeck.Count);
        OnDeckSizeChangeString.Invoke(PlayerDeck.Count.ToString());
        CardController card = _handController.DrawCard(drawn, onAnimationComplete);
    }

    public void ShuffleDiscard(System.Action onAnimationComplete)
    {
        
        if (_handController.Discarded.Count == 0)
        {
            DiscardPile.Clear();
            PlayerDeck.Shuffle();
            onAnimationComplete.Invoke();
            return;
        }

        CardController card = _handController.Discarded[_handController.Discarded.Count - 1];
        _handController.Discarded.RemoveAt(_handController.Discarded.Count - 1);
        CardData data = card.Card;
        PlayerDeck.Add(data);
        int count = PlayerDeck.Count;
        StartCoroutine(_handController.AnimateShuffle(card, () =>
        {
            OnDeckSizeChange.Invoke(count);
            OnDeckSizeChangeString.Invoke(count.ToString());            
            ShuffleDiscard(onAnimationComplete);
        }));
    }

    internal CardController AddWoundToHand(System.Action onAnimationComplete) => _handController.AddWoundToHand(onAnimationComplete);
}
