using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MonsterDialogController : MonoBehaviour
{
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
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _physicalDamage;
    [SerializeField]
    private TextMeshProUGUI _magicDamage;
    [SerializeField]
    private TextMeshProUGUI _abilityText;
    [SerializeField]
    private TextMeshProUGUI _flavorText;
    [SerializeField]
    private bool _forceValidate;
    public void Render()
    {
        Debug.Assert(_card is not null, "Cannot render null card.");
        _image.sprite = _card.UIArt;
        _name.text = _card.Name;
        _physicalDamage.text = _card.HitPoints.ToString();
        _magicDamage.text = _card.MagicPoints.ToString();
        _abilityText.text = _card.AbilityText;
        _flavorText.text = _card.FlavorText;
    }

    private void OnValidate()
    {
        if (_forceValidate)
        {
            _forceValidate = false;
        }        
        if (_card is null) { return; }
        Render();
    }
}
