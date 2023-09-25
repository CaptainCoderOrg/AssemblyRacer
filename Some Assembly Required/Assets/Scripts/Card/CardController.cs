using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CardClickController), typeof(SortingGroup))]
public class CardController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] _icons;
    [SerializeField]
    private TextMeshPro _title;
    [SerializeField]
    private SpriteRenderer _art;
    [field: SerializeField]
    private CardData _card;
    public CardData Card { 
        get => _card; 
        set
        {
            _card = value;
            Render();
        } 
    }
    [field: SerializeField]
    public IconDatabase IconDatabase { get; private set; }
    public SortingGroup SortingGroup { get; private set; }

    public CardClickController ClickController { get; private set; }

    private void Awake()
    {
        ClickController = GetComponent<CardClickController>();
        SortingGroup = GetComponent<SortingGroup>();
    }

    public void Render()
    {
        Debug.Assert(Card != null);
        _title.text = Card.Name;
        _art.sprite = Card.Art;
        int attack = Card.Attack;
        int magic = Card.Magic;
        int coin = Card.Gold;
        int wounds = Card.Wounds;
        int abilityText = 0;
        if (Card.AbilityText is not null && Card.AbilityText.Trim() != string.Empty)
        {
            abilityText = 1;
        }
        foreach(SpriteRenderer icon in _icons)
        {
            if (attack > 0)
            {
                icon.sprite = IconDatabase.Attack;
                attack--;
            }
            else if (magic > 0)
            {
                icon.sprite = IconDatabase.Magic;
                magic--;
            }
            else if (coin > 0)
            {
                icon.sprite = IconDatabase.Coin;
                coin--;
            }
            else if (wounds > 0)
            {
                icon.sprite = IconDatabase.Wound;
                wounds--;
            }
            else if (abilityText > 0)
            {
                icon.sprite = IconDatabase.Special;
                abilityText--;
            }
            else
            {
                icon.sprite = null;    
            }
        }
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
