using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecruitsTrackController : MonoBehaviour
{
    [SerializeField]
    private float _cardSlideTime = 0.15f;
    [SerializeField]
    private Transform[] _slotPositions;
    [SerializeField]
    private CardController _template;
    [SerializeField]
    private Transform _discardPileTransform;
    [SerializeField]
    private CardController[] _recruits;
    public CardController[] Recruits => _recruits;
    [SerializeField]
    private Transform _recruitsContainer;

    private void Start()
    {
        _recruits = new CardController[5];
    }

    public CardController AddRecruit(RecruitDeckManager deckManager, System.Action onAnimationFinished)
    {
        if (TryFindEmptyRecruitSlot(out int ix) == false) 
        { 
            onAnimationFinished.Invoke();
            return null; 
        }
        // TODO: This should be game over?
        if (deckManager.TryDrawRecruit(out CardData toAdd) == false) 
        { 
            onAnimationFinished.Invoke();
            return null; 
        }
        CardController newCard = Instantiate(_template, _recruitsContainer);
        newCard.Card = toAdd;
        newCard.name = toAdd.Name;
        newCard.gameObject.SetActive(true);
        float rotation = Random.Range(-7f, 7f);
        newCard.transform.rotation = Quaternion.Euler(0, 0, rotation);
        _recruits[ix] = newCard;
        StartCoroutine(SlideCardTo(newCard.gameObject, _slotPositions[ix], _cardSlideTime, onAnimationFinished));
        return newCard;
    }

    private bool TryFindEmptyRecruitSlot(out int ix)
    {
        for (ix = 0; ix < _recruits.Length; ix++)
        {
            if (_recruits[ix] is null)
            {
                return true;
            }
        }
        return false;
    }

    public void AddRecruitToDiscard(int ix, PlayerDeckManager playerDeckManager, System.Action onAnimationComplete)
    {
        CardController card = _recruits[ix];
        StartCoroutine(SlideCardTo(card.gameObject, _discardPileTransform, _cardSlideTime, () =>
        {
            playerDeckManager.AddCardToDiscard(card);
            card.Interactable = false;
            _recruits[ix] = null;
            onAnimationComplete.Invoke();
        }));
    }

    public IEnumerator SlideCardTo(GameObject toMove, Transform target, float? duration = null, System.Action OnComplete = null)
    {
        float slideTime = duration ?? _cardSlideTime;
        float startTime = Time.time;
        float endTime = Time.time + slideTime;
        Vector2 startPosition = toMove.transform.position;
        Vector2 endPosition = target.position;
        float randomY = Random.Range(-.1f, .1f);
        endPosition.y += randomY;
        Quaternion startRotation = toMove.transform.rotation;
        
        float rotation = Random.Range(-10f, 10f);
        Quaternion endRotation = Quaternion.Euler(0, 0, rotation);
        while (Time.time < endTime)
        {
            float timePassed = Time.time - startTime;
            float percent = Mathf.Clamp01(timePassed/slideTime);
            toMove.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percent);
            toMove.transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return new WaitForEndOfFrame();
        }
        toMove.transform.position = endPosition;
        OnComplete?.Invoke();
    }

    internal bool TryFindRecruitIx(CardController card, out int ix)
    {
        for (ix = 0; ix < _recruits.Length; ix++)
        {
            if (_recruits[ix] == card)
            {
                return true;
            }
        }
        return false;
    }
}
