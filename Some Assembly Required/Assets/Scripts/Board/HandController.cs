using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
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
        float handSize = Mathf.Max(6, _cards.Count);
        float space = _rightPivot.position.x - _leftPivot.position.x;
        float increment = space / handSize;
        // -10 => 10
        float maxY = 1;
        float startRotation = 25f;
        float rotationIncrement = -50f / handSize;
        for (int ix = 0; ix < _cards.Count; ix++)
        {
            CardController card = _cards[ix];
            Vector2 position = _leftPivot.position;
            position.x += increment * ix;
            float percent = ix/ handSize;
            float heightCurve = _cardHeightCurve.Evaluate(percent);
            position.y += maxY * heightCurve;
            card.transform.position = position;
            card.transform.rotation = Quaternion.Euler(0, 0, startRotation + rotationIncrement * ix);
        }
    }

    

}
