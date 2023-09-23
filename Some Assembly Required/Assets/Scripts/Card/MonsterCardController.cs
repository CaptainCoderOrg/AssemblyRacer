using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterCardController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _attackDamage;
    [SerializeField]
    private TextMeshPro _magicDamage;
    [SerializeField]
    private TextMeshPro _title;
    [SerializeField]
    private SpriteRenderer _art;
    

    [SerializeField]
    private MonsterCardData _card;
    
    public MonsterCardData Card 
    { 
        get => _card; 
        set
        {
            _card = value;
            Render();
        }
    }
    
    [field: SerializeField]
    public IconDatabase IconDatabase { get; private set; }

    public void Render()
    {
        Debug.Assert(Card != null);
        _title.text = Card.Name;
        _art.sprite = Card.Art;
        _attackDamage.text = Card.HitPoints.ToString();
        _magicDamage.text = Card.MagicPoints.ToString();
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
