using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardAbilityDialog : MonoBehaviour
{
    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private CardController _card;
    public CardController Card
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
    private TextMeshProUGUI _goldValue;
    [SerializeField]
    private TextMeshProUGUI _abilityText;
    [SerializeField]
    private TextMeshProUGUI _flavorText;
    [Button("Force Render")]
    public void Render()
    {
        Debug.Assert(_card is not null, "Cannot render null card.");
        _image.sprite = _card.Card.UIArt;
        _name.text = _card.Card.Name;
        _physicalDamage.text = (_card.Card.Attack + _card.BonusDamage).ToString();
        _magicDamage.text = (_card.Card.Magic + _card.BonusMagic).ToString();
        _goldValue.text = _card.Card.Gold.ToString();
        _abilityText.text = _card.Card.AbilityText;
        _flavorText.text = _card.Card.FlavorText;
        _physicalDamage.color = Color.black;
        _magicDamage.color = Color.black;
        _goldValue.color = Color.black;
        _useButton.gameObject.SetActive(false);
        UpdateUse();
    }

    public void OnClickCard(CardController clicked)
    {
        CardSelectorManager.OnClick(clicked);
        UpdateUse();
    }

    public void UpdateUse()
    {
        _useButton.gameObject.SetActive(false);
        if (_card is null) { return; }
        if (_card.Card.Ability is null) { return; }
        if (_card.Card.Ability.CheckRequirement(CardSelectorManager.Selected.ToList()) is false) { return; }
        _useButton.gameObject.SetActive(true);
    }
}
