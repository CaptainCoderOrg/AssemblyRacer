using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CardHoverController), typeof(CardClickController), typeof(SortingGroup))]
public class MonsterCardController : MonoBehaviour
{
    private AudioSource _audioSource;
    private PolygonCollider2D _collider;
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
    private bool _isInteractable = true;
    public bool Interactable 
    { 
        get => _isInteractable; 
        set
        {
            _isInteractable = value;
            _collider.enabled = value;
        }
    }
    public CardClickController ClickController { get; private set; }
    public SortingGroup  SortingGroup { get; private set; }
    
    [field: SerializeField]
    public IconDatabase IconDatabase { get; private set; }
    public CardAudioDatabase CardAudioDatabase;

    private void Awake()
    {
        ClickController = GetComponent<CardClickController>();
        SortingGroup = GetComponent<SortingGroup>();
        _collider = GetComponent<PolygonCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayEnterSound() => PlaySound(Card.EnterSound);
    internal void PlayClickSound() => PlaySound(CardAudioDatabase.Click);

    private void PlaySound(AudioClip clip)
    {
        _audioSource.volume = VolumeController.SFXVolume;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void Render()
    {
        Debug.Assert(Card != null);
        _title.text = Card.Name;
        _art.sprite = Card.Art;
        _attackDamage.text = Card.HitPoints.ToString();
        _magicDamage.text = Card.MagicPoints.ToString();
    }
}
