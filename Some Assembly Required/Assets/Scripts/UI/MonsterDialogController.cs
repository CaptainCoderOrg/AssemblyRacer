using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MonsterDialogController : MonoBehaviour
{
    [SerializeField]
    private Button _defeatButton;
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
        _physicalDamage.color = Color.black;
        _magicDamage.color = Color.black;
        _defeatButton.gameObject.SetActive(false);
        UpdateCost();
    }

    public void OnClickCard(CardController clicked)
    {
        CardSelectorManager.OnClick(clicked);
        UpdateCost();
    }

    public void UpdateCost()
    {
        int totalAttack = 0;
        int totalMagic = 0;
        foreach (CardController card in CardSelectorManager.Selected)
        {
            totalAttack += card.Card.Attack + card.BonusDamage;
            totalMagic += card.Card.Magic + card.BonusMagic;
        }

        int attackRemaining = Mathf.Max(0, Card.HitPoints - totalAttack);
        _physicalDamage.text = attackRemaining.ToString();
        if (attackRemaining == 0)
        {
            _physicalDamage.color = Color.green;
        }
        else
        {
            _physicalDamage.color = Color.red;
        }

        int magicRemaining = Mathf.Max(0, Card.MagicPoints - totalMagic);
        _magicDamage.text = magicRemaining.ToString();
        if (magicRemaining == 0)
        {
            _magicDamage.color = Color.green;
        }
        else
        {
            _magicDamage.color = Color.red;
        }
        if (magicRemaining == 0 && attackRemaining == 0)
        {
            _defeatButton.gameObject.SetActive(true);
        }
        else
        {
            _defeatButton.gameObject.SetActive(false);
        }
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
