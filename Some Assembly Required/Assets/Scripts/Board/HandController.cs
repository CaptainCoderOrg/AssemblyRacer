using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [field: SerializeField]
    public float AnimationDuration {get; private set; } = 0.15f;
    [SerializeField]
    private AnimationCurve _cardHeightCurve;
    [SerializeField]
    private CardController _template;
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

    public IEnumerable<CardController> DiscardHand(Transform discardPile, System.Action onAnimationComplete)
    {
        StopAllCoroutines(); // TODO: This is kinda dangerous
        for (int ix = 0; ix < _cards.Count; ix++) // (CardController card in _cards)
        {
            CardController card = _cards[ix];
            Discarded.Add(card);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
            onAnimationComplete = ix == _cards.Count - 1 ? onAnimationComplete : () => {};
            StartCoroutine(AnimateCardMove(card, discardPile.position, rotation, onAnimationComplete));
            card.GetComponent<PolygonCollider2D>().enabled = false;
            yield return card;
        }
        _cards.Clear();
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

    private void PlaceAllCards(System.Action onAnimationComplete)
    {
        StopAllCoroutines(); // TODO: This is dangerous as it can happen unintentionally.
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
            float percent = ix/ (handSize - 1);
            float heightCurve = _cardHeightCurve.Evaluate(percent);
            position.y += maxY * heightCurve;
            
            Quaternion rotation = Quaternion.Euler(0, 0, startRotation + rotationIncrement * ix);
            onAnimationComplete = ix == 0 ? onAnimationComplete : () => {};
            StartCoroutine(AnimateCardMove(card, position, rotation, onAnimationComplete));
        }
    }

    public IEnumerator AnimateShuffle(CardController card, System.Action onFinished)
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

    

}
