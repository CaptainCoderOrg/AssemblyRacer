using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterTrackController : MonoBehaviour
{
    public UnityEvent<MonsterCardController> OnMonsterAttack;
    public UnityEvent<(MonsterCardController, CardController)> OnMonsterAttackFinished;
    [SerializeField]
    private float _cardSlideTime = 1f;
    [SerializeField]
    private Transform[] _slotPositions;
    [SerializeField]
    private MonsterCardController _template;
    [SerializeField]
    private CardController _booBooTemplate;
    [SerializeField]
    private Transform _discardPileTransform;
    [SerializeField]
    private Transform _monsterFinalPosition;
    [SerializeField]
    private MonsterCardController[] _monsters;
    [SerializeField]
    private Transform _monstersContainer;

    private void Start()
    {
        _monsters = new MonsterCardController[5];
    }

    public MonsterCardController AddMonster(MonsterCardData toAdd, System.Action onAnimationFinished)
    {
        MonsterCardController newMonster = Instantiate(_template, _monstersContainer);
        newMonster.Card = toAdd;
        newMonster.name = toAdd.Name;
        newMonster.gameObject.SetActive(true);
        float rotation = Random.Range(-7f, 7f);
        newMonster.transform.rotation = Quaternion.Euler(0, 0, rotation);
        PlaceMonster(newMonster,  0, onAnimationFinished);
        return newMonster;
    }

    public void DefeatMonster(int ix)
    {
        if (_monsters[ix] is null) { return; }
        Destroy(_monsters[ix].gameObject);
        _monsters[ix] = null;
    }

    public void PlaceMonster(MonsterCardController card, int ix, System.Action onAnimationFinished)
    {
        if (ix >= _monsters.Length)
        {
            MonsterAttacks(card, onAnimationFinished);
            return;
        }
        if (_monsters[ix] is not null)
        {
            MonsterCardController bumped = _monsters[ix];
            PlaceMonster(bumped, ix + 1, onAnimationFinished);
            onAnimationFinished = null;
        }
        _monsters[ix] = card;
        StartCoroutine(SlideCardTo(card.gameObject, _slotPositions[ix], _cardSlideTime, onAnimationFinished));
        
    }

    public void MonsterAttacks(MonsterCardController card, System.Action onAnimationFinished)
    {
        // TODO: And then animation monad would be great here.
        card.Interactable = false;
        StartCoroutine(SlideCardTo(card.gameObject, _booBooTemplate.transform, _cardSlideTime, () => {
            GameObject grouped = new GameObject("Monster and Wound");
            grouped.transform.position = card.transform.position;
            card.transform.parent = grouped.transform;
            card.SortingGroup.sortingLayerName = "Moving Card";

            CardController woundCard = Instantiate(_booBooTemplate, this.transform);
            woundCard.transform.parent = grouped.transform;
            woundCard.transform.localPosition = new Vector2(.2f, -.2f);
            woundCard.gameObject.SetActive(true);
            woundCard.SortingGroup.sortingLayerName = "Moving Card";
            woundCard.SortingGroup.sortingOrder = 1;
            
            OnMonsterAttack.Invoke(card);

            StartCoroutine(SlideCardTo(grouped, _discardPileTransform, .5f, () => {
                woundCard.transform.localPosition = Vector2.zero;
                woundCard.SortingGroup.sortingLayerName = "Card";
                woundCard.SortingGroup.sortingOrder = 0;
                StartCoroutine(SlideCardTo(card.gameObject, _monsterFinalPosition, .5f, () => {
                    OnMonsterAttackFinished.Invoke((card, woundCard));
                    Destroy(card.gameObject);
                    woundCard.transform.parent = null;
                    Destroy(grouped.gameObject);
                    onAnimationFinished.Invoke();
                }));
            }));
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

    internal bool TrySelectMonster(int ix, out MonsterCardController monsterCard)
    {
        monsterCard = null;
        if (_monsters[ix] == null) { return false; }
        monsterCard = _monsters[ix];
        return true;
    }

    internal bool TryFindMonsterIx(MonsterCardController monsterCard, out int ix)
    {
        for (ix = 0; ix < _monsters.Length; ix++)
        {
            if (_monsters[ix] == monsterCard)
            {
                return true;
            }
        }
        return false;
    }
}
