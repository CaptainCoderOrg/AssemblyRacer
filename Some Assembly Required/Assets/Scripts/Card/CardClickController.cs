using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CardClickController : MonoBehaviour
{
    public UnityEvent OnClick;

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        
        OnClick.Invoke();
        GetComponent<CardController>()?.PlayClickSound();
        GetComponent<MonsterCardController>()?.PlayClickSound();
    }

}