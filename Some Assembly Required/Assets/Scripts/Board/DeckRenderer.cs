using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeckRenderer : MonoBehaviour
{
    private static Action<GameObject> s_Destroy
    {
        get
        {
            return Destroy;
        }
    }
    [SerializeField]
    private int _count;
    [SerializeField]
    private float _offset = 0.125f;
    [SerializeField]
    private SpriteRenderer _cardTemplate;
    [SerializeField]
    private Transform _deckContainer;

    public int Count
    {
        get => _count;
        set
        {
            _count = Mathf.Max(0, value);
            Render();
        }
    }

    void Start()
    {
        Render();
    }

    private void Render()
    {
        if (_deckContainer.childCount < Count)
        {
            for (int ix = 0; ix < Count; ix++)
            {
                SpriteRenderer renderer = Instantiate(_cardTemplate, _deckContainer);
                Vector2 pos = renderer.transform.position;
                pos.x += _offset * -ix;
                pos.y += _offset * ix;
                renderer.transform.position = pos;
            }
        }
        else
        {

            foreach (Transform child in _deckContainer)
            {
                if (_deckContainer.childCount <= Count)
                {
                    break;
                }
                s_Destroy(child.gameObject);
            }
        }
    }

    private void EmptyDeck()
    {
        foreach (Transform child in _deckContainer)
        {
            s_Destroy(child.gameObject);
        }
    }
}
