using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HandController : MonoBehaviour
{
    [field: SerializeField]
    public float AnimationDuration { get; private set; } = 0.15f;
    [SerializeField]
    private AnimationCurve _cardHeightCurve;
    [SerializeField]
    private CardController _template;
    [SerializeField]
    private CardController _woundTemplate;
    [SerializeField]
    private Transform _leftPivot;
    [SerializeField]
    private Transform _rightPivot;
    [SerializeField]
    private Transform _cardContainer;
    [field: SerializeField]
    public List<CardController> Discarded { get; private set; }
    private List<CardController> _cards;

    private void Awake()
    {
        Discarded = new List<CardController>();
        _cards = new List<CardController>();
    }

    public CardController AddWoundToHand(System.Action onAnimationComplete)
    {
        CardController newCard = Instantiate(_template, _cardContainer);
        newCard.transform.position = _woundTemplate.transform.position;
        newCard.Card = _woundTemplate.Card;
        newCard.name = "Wound";
        newCard.gameObject.SetActive(true);
        _cards.Add(newCard);
        PlaceAllCards(onAnimationComplete);
        return newCard;
    }

    public void EnableSelectionMode(System.Action<CardController> OnClick)
    {
        foreach (CardController card in _cards)
        {
            card.ClickController.OnClick.RemoveAllListeners();
            card.Selected = false;
            card.ClickController.OnClick.AddListener(() => OnClick.Invoke(card));
        }
    }

    public void DisableSelectMode()
    {
        foreach (CardController card in _cards)
        {
            card.ClickController.OnClick.RemoveAllListeners();
            card.Selected = false;
        }
    }

    public IEnumerable<CardController> DiscardHand(Transform discardPile, System.Action onAnimationComplete)
    {
        if (_cards.Count == 0)
        {
            onAnimationComplete.Invoke();
        }
        // StopAllCoroutines(); // TODO: This is kinda dangerous
        for (int ix = 0; ix < _cards.Count; ix++) // (CardController card in _cards)
        {
            CardController card = _cards[ix];
            Discarded.Add(card);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
            System.Action action = ix == _cards.Count - 1 ? onAnimationComplete : () => { };
            StartCoroutine(AnimateCardMove(card, discardPile.position, rotation, action));
            card.GetComponent<PolygonCollider2D>().enabled = false;
            card.Selected = false;
            yield return card;
        }
        _cards.Clear();
    }

    internal void DiscardCards(List<CardController> cards, Transform discardPile, System.Action onAnimationComplete)
    {
        for (int ix = 0; ix < cards.Count; ix++) //(CardController card in _cards)
        {
            CardController card = cards[ix];
            card.Selected = false;
            Discarded.Add(card);
            _cards.Remove(card);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
            System.Action action = ix == 0 ? onAnimationComplete : () => { };
            StartCoroutine(AnimateCardMove(card, discardPile.position, rotation, () =>
            {
                PlaceAllCards(action);
            }));
            card.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    public CardController DrawCard(CardData data, System.Action onAnimationComplete)
    {
        CardController newCard = Instantiate(_template, _cardContainer);
        newCard.Card = data;
        newCard.name = data.Name;
        newCard.gameObject.SetActive(true);
        _cards.Add(newCard);
        PlaceAllCards(onAnimationComplete);
        return newCard;
    }

    public void AddCardToHand(CardController cardToAdd, System.Action onAnimationComplete)
    {
        cardToAdd.Interactable = true;
        _cards.Add(cardToAdd);
        PlaceAllCards(onAnimationComplete);
    }

    private void PlaceAllCards(System.Action onAnimationComplete)
    {
        if (_cards.Count == 0)
        {
            onAnimationComplete.Invoke();
            return;
        }
        // StopAllCoroutines(); // TODO: This is dangerous as it can happen unintentionally.
        float handSize = Mathf.Max(6, _cards.Count);
        float space = _rightPivot.position.x - _leftPivot.position.x;
        float increment = space / (handSize - 1);
        // -10 => 10
        float maxY = 1;
        float startRotation = 15f;
        float rotationIncrement = -30f / (handSize - 1);
        for (int ix = 0; ix < _cards.Count; ix++)
        {
            CardController card = _cards[ix];
            Vector2 position = _leftPivot.position;
            position.x += increment * ix;
            float percent = ix / (handSize - 1);
            float heightCurve = _cardHeightCurve.Evaluate(percent);
            position.y += maxY * heightCurve;

            Quaternion rotation = Quaternion.Euler(0, 0, startRotation + rotationIncrement * ix);
            System.Action action = ix == 0 ? onAnimationComplete : () => { };
            StartCoroutine(AnimateCardMove(card, position, rotation, action));
        }

    }

    public IEnumerator AnimateShuffle(CardController card, System.Action onFinished)
    {
        if (!card.IsDestroyed())
        {
            float startTime = Time.time;
            float duration = AnimationDuration;
            float endTime = startTime + duration;
            Vector2 startPosition = card.transform.position;
            Vector2 endPosition = _template.transform.position;
            while (Time.time < endTime)
            {
                float percent = Mathf.Clamp01((Time.time - startTime) / duration);
                card.transform.position = Vector2.Lerp(startPosition, endPosition, percent);
                yield return new WaitForEndOfFrame();
            }
            card.transform.position = endPosition;
            onFinished.Invoke();
            Destroy(card.gameObject);
        }
        else
        {
            onFinished.Invoke();
        }
    }

    private IEnumerator AnimateCardMove(CardController card, Vector2 endPosition, Quaternion endRotation, System.Action onAnimationComplete)
    {
        float startTime = Time.time;
        float endTime = startTime + AnimationDuration;
        Vector2 startPosition = card.transform.position;
        Quaternion startRotation = card.transform.rotation;
        while (Time.time < endTime)
        {
            float percent = Mathf.Clamp01((Time.time - startTime) / AnimationDuration);
            card.transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            card.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percent);
            yield return new WaitForEndOfFrame();
        }
        card.transform.position = endPosition;
        card.transform.rotation = endRotation;
        onAnimationComplete.Invoke();
    }

    internal void RemoveCardsFromGame(List<CardController> cards, System.Action onAnimationComplete)
    {
        int ix = 0;
        foreach (CardController card in cards)
        {
            Vector2 endPosition = card.transform.position;
            endPosition.y = 10;
            card.Selected = false;
            _cards.Remove(card);
            StartCoroutine(AnimateCardMove(card, endPosition, card.transform.rotation, () =>
            {

                Destroy(card.gameObject);
                if (ix++ == 0)
                {
                    onAnimationComplete.Invoke();
                }
            }));
        }
    }
}
