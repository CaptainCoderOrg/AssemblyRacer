using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CardClickController), typeof(SortingGroup))]
public class CardController : MonoBehaviour
{
    [field: SerializeField]
    public int TimesUsed = 0;
    [field: SerializeField]
    public int BonusDamage { get; set; }
    [field: SerializeField]
    public int BonusMagic { get; set; }
    [field: SerializeField]
    public int BonusGold { get; set; }
    [SerializeField]
    private GameObject _selectedObject;
    private PolygonCollider2D _collider;
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
    private bool _interactable;
    public bool Interactable { 
        get => _interactable;
        set 
        {
            _interactable = value;
            _collider.enabled = value;
        }
    }

    private bool _selected;
    public bool Selected {
        get => _selected;
        set
        {
            _selected = value;
            if (_selectedObject.IsDestroyed()) { return; }
            _selectedObject?.SetActive(value);
        }
    }

    [field: SerializeField]
    public IconDatabase IconDatabase { get; private set; }
    public SortingGroup SortingGroup { get; private set; }

    public CardClickController ClickController { get; private set; }

    private AudioSource _audioSource;
    public CardAudioDatabase CardAudioDatabase;

    private void Awake()
    {
        ClickController = GetComponent<CardClickController>();
        SortingGroup = GetComponent<SortingGroup>();
        _collider = GetComponent<PolygonCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    
    [Button("Force Render")]
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
    internal void PlayShuffleSound() => PlaySound(CardAudioDatabase.Shuffle);
    internal void PlayClickSound() => PlaySound(CardAudioDatabase.Click);
    internal void PlayDrawSound() => PlaySound(CardAudioDatabase.Draw);
    public void PlayRecruitSound() => PlaySound(Card.HireSound);
    


    private void PlaySound(AudioClip clip)
    {
        _audioSource.volume = VolumeController.SFXVolume;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    
}
