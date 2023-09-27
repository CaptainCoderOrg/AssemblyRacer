using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitDialogController : MonoBehaviour
{
    
    [SerializeField]
    private CardData _card;
    public CardData Card
    {
        get => _card;
        set
        {
            _card = value;
            Render();
        }
    }
    [SerializeField]
    private IconDatabase _iconDatabase;
    [SerializeField]
    private Image _iconTemplate;
    [SerializeField]
    private Transform _iconContainer;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private TextMeshProUGUI _abilityText;
    [SerializeField]
    private TextMeshProUGUI _flavorText;
    [SerializeField]
    private Button _hireButton;
    [SerializeField]
    private bool _forceValidate;


    [Button("Force Render")]
    public void Render()
    {
        Debug.Assert(_card is not null, "Cannot render null card.");
        CardSelectorManager.Clear();
        _image.sprite = _card.UIArt;
        _name.text = _card.Name;
        _costText.text = _card.Cost.ToString();
        RenderIcons();
        _abilityText.text = _card.AbilityText;
        _flavorText.text = _card.FlavorText;
        _costText.color = Color.black;
        _hireButton.gameObject.SetActive(false);
    }

    private void RenderIcons()
    {
        _iconContainer.DestroyChildren();
        RenderIcon(Card.Attack, _iconDatabase.Attack);
        RenderIcon(Card.Magic, _iconDatabase.Magic);
        RenderIcon(Card.Gold, _iconDatabase.Coin);
        RenderIcon(Card.Wounds, _iconDatabase.Wound);
    }

    private void RenderIcon(int count, Sprite image)
    {
        for (int ix = 0; ix < count; ix++)
        {
            Image icon = Instantiate<Image>(_iconTemplate, _iconContainer);
            icon.sprite = image;
            icon.gameObject.SetActive(true);
        }
    }


    public void OnClickCard(CardController clicked)
    {
        CardSelectorManager.OnClick(clicked);
        UpdateCost();
    }

    private void UpdateCost()
    {
        if (CardSelectorManager.Count == 0)
        {
            Render();
            return;
        }
        int totalValue = 0;
        foreach (CardController card in CardSelectorManager.Selected)
        {
            totalValue += card.Card.Gold;
        }
        int newCost = Mathf.Max(0, Card.Cost - totalValue);
        _costText.text = newCost.ToString();
        if (newCost == 0)
        {
            _costText.color = Color.green;
            _hireButton.gameObject.SetActive(true);
        }
        else
        {
            _costText.color = Color.red;
            _hireButton.gameObject.SetActive(false);
        }
    }

    

    private void OnValidate()
    {
        if (_forceValidate)
        {
            _forceValidate = false;
        }
        if (_card is null) { return; }
        // Render(true);
    }
}
