using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CardHoverController : MonoBehaviour
{
    // [SerializeField]
    // public SortingLayer _onHoverLayer;
    [SerializeField]
    private float _growDuration = .25f;
    [SerializeField]
    private float _onHoverScale = 1.4f;
    private Vector3 _initialScale;
    private Coroutine _growing = null;
    private int _initialSortingLayer;
    private SortingGroup _sortingGroup;
    private void Awake()
    {
        _initialScale = transform.localScale;
        _sortingGroup = GetComponent<SortingGroup>();
        _initialSortingLayer = _sortingGroup.sortingLayerID;
    }
    private void OnMouseOver()
    {   
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        TryAnimate();
        
    }

    private void OnMouseExit()
    {
        StopAnimation();
    }

    private void StopAnimation()
    {
        if (_growing is not null) 
        { 
            StopCoroutine(_growing);
            _growing = null;    
        }
        
        transform.localScale = _initialScale;
        _sortingGroup.sortingLayerID = _initialSortingLayer;
    }

    private void TryAnimate()
    {
        if (_growing is not null) { return ; }
        _growing = StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        // _sortingGroup.sortingLayerID = _onHoverLayer.id;
        Vector3 targetScale = _initialScale * _onHoverScale;
        float startTime = Time.time;
        float endTime = Time.time + _growDuration;
        while (Time.time < endTime)
        {
            float timeElapsed = Time.time - startTime;
            float percent = Mathf.Clamp01(timeElapsed / _growDuration);
            transform.localScale = Vector3.Lerp(_initialScale, targetScale, percent);
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = targetScale;
    }


}