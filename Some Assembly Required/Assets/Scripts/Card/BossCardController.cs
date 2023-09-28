using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossCardController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _attackDamage;
    [SerializeField]
    private TextMeshPro _magicDamage;
    [SerializeField]
    private TextMeshPro _title;
    [SerializeField]
    private SpriteRenderer _art;
    

    [field: SerializeField]
    public BossCardData Card { get; private set; }
    [field: SerializeField]
    public IconDatabase IconDatabase { get; private set; }
    public CardClickController ClickController { get; private set; }

    private void Awake()
    {
        ClickController = GetComponent<CardClickController>();
        Debug.Assert(ClickController is not null);
    }

    public void Render()
    {
        Debug.Assert(Card != null);
        _title.text = Card.Name;
        _art.sprite = Card.Art;
        _attackDamage.text = Card.HitPoints.ToString();
        _magicDamage.text = Card.MagicPoints.ToString();
        // TODO: Display hitpoints / magic points
    }
    

    #if UNITY_EDITOR
    public bool ForceUpdate;
    public void OnValidate()
    {
        ForceUpdate = false;
        if (Card == null) { return; }
        Render();
    }
    #endif
}
