using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitDialogController : MonoBehaviour
{
    private HashSet<CardController> _selected = new HashSet<CardController>();
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
    private bool _forceValidate;

    [Button("Force Render")]
    public void Render()
    {
        Debug.Assert(_card is not null, "Cannot render null card.");
        _selected.Clear();
        _image.sprite = _card.UIArt;
        _name.text = _card.Name;
        _costText.text = _card.Cost.ToString();
        RenderIcons();
        _abilityText.text = _card.AbilityText;
        _flavorText.text = _card.FlavorText;
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
        if (_selected.Contains(clicked))
        {
            _selected.Remove(clicked);
            clicked.Selected = false;

        }
        else
        {
            _selected.Add(clicked);
            clicked.Selected = true;
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
