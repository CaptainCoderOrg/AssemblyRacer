using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrackController : MonoBehaviour
{
    [SerializeField]
    private float _cardSlideTime = 1f;
    [SerializeField]
    private Transform[] _slotPositions;
    [SerializeField]
    private MonsterCardController _template;
    [SerializeField]
    private MonsterCardController[] _monsters;
    [SerializeField]
    private Transform _monstersContainer;

    private void Start()
    {
        _monsters = new MonsterCardController[5];
    }

    public void AddMonster(MonsterCardData toAdd)
    {
        MonsterCardController newMonster = Instantiate(_template, _monstersContainer);
        newMonster.Card = toAdd;
        newMonster.name = toAdd.Name;
        newMonster.gameObject.SetActive(true);
        float rotation = Random.Range(-7f, 7f);
        newMonster.transform.rotation = Quaternion.Euler(0, 0, rotation);
        PlaceMonster(newMonster,  0);
    }

    public void DefeatMonster(int ix)
    {
        if (_monsters[ix] is null) { return; }
        Destroy(_monsters[ix].gameObject);
        _monsters[ix] = null;
    }

    public void PlaceMonster(MonsterCardController card, int ix)
    {
        if (ix >= _monsters.Length)
        {
            MonsterAttacks(card);
            return;
        }
        StartCoroutine(SlideCardTo(card, _slotPositions[ix]));
        if (_monsters[ix] is not null)
        {
            MonsterCardController bumped = _monsters[ix];
            PlaceMonster(bumped, ix + 1);
        }
        _monsters[ix] = card;
    }

    public void MonsterAttacks(MonsterCardController card)
    {
        Destroy(card.gameObject);
    }

    public IEnumerator SlideCardTo(MonsterCardController toMove, Transform target)
    {
        float startTime = Time.time;
        float endTime = Time.time + _cardSlideTime;
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
            float percent = Mathf.Clamp01(timePassed/_cardSlideTime);
            toMove.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percent);
            toMove.transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return new WaitForEndOfFrame();
        }
        toMove.transform.position = endPosition;
    }
}
