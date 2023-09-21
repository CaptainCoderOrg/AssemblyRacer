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

    public void AddMonster(MonsterCardData toAdd)
    {
        MonsterCardController newMonster = Instantiate(_template, _monstersContainer);
        newMonster.Card = toAdd;
        newMonster.name = toAdd.Name;
        newMonster.gameObject.SetActive(true);
        float rotation = Random.Range(-7f, 7f);
        newMonster.transform.rotation = Quaternion.Euler(0, 0, rotation);
        StartCoroutine(SlideCardTo(newMonster, _slotPositions[4]));
    }

    public IEnumerator SlideCardTo(MonsterCardController toMove, Transform target)
    {
        float startTime = Time.time;
        float endTime = Time.time + _cardSlideTime;
        Vector2 startPosition = toMove.transform.position;
        Vector2 endPosition = target.position;
        while (Time.time < endTime)
        {
            float timePassed = Time.time - startTime;
            float percent = Mathf.Clamp01(timePassed/_cardSlideTime);
            toMove.transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return new WaitForEndOfFrame();
        }
        toMove.transform.position = endPosition;
    }
}
