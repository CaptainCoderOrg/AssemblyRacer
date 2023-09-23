using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DeckImageController : MonoBehaviour
{
    [SerializeField]
    private Sprite _fullImage;
    [SerializeField]
    private Sprite _emptyImage;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Render(int count)
    {
        if (count > 0)
        {
            _spriteRenderer.sprite = _fullImage;
        }
        else
        {
            _spriteRenderer.sprite = _emptyImage;
        }
    }
}
