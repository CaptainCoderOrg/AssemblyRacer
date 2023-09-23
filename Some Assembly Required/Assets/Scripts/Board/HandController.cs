using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    private float _animationDuration = 0.15f;
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
    private List<CardController> _cards;

    private void Awake()
    {
        _cards = new List<CardController>();
    }

    public IEnumerable<CardController> DiscardHand(Transform discardPile)
    {
        StopAllCoroutines();
        foreach (CardController card in _cards)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
            StartCoroutine(AnimateCardMove(card, discardPile.position, rotation));
            card.GetComponent<PolygonCollider2D>().enabled = false;
            yield return card;
        }
        _cards.Clear();
    }

    public CardController DrawCard(CardData data)
    {
        CardController newCard = Instantiate(_template, _cardContainer);
        newCard.Card = data;
        newCard.name = data.Name;
        newCard.gameObject.SetActive(true);
        // float rotation = Random.Range(-7f, 7f);
        // newCard.transform.rotation = Quaternion.Euler(0, 0, rotation);
        _cards.Add(newCard);
        PlaceAllCards();
        return newCard;
    }

    private void PlaceAllCards()
    {
        StopAllCoroutines();
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
            StartCoroutine(AnimateCardMove(card, position, rotation));
        }
    }

    private IEnumerator AnimateCardMove(CardController card, Vector2 endPosition, Quaternion endRotation)
    {
        float startTime = Time.time;
        float endTime = startTime + _animationDuration;
        Vector2 startPosition = card.transform.position;
        Quaternion startRotation = card.transform.rotation;
        while (Time.time < endTime)
        {
            float percent = Mathf.Clamp01((Time.time - startTime) / _animationDuration);
            card.transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            card.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percent);
            yield return new WaitForEndOfFrame();
        }
        card.transform.position = endPosition;
        card.transform.rotation = endRotation;
    }

    

}
