using System;
using System.Collections.Generic;
using System.Linq;

public static class CardSelectorManager
{
    private static HashSet<CardController> _selected = new HashSet<CardController>();
    public static  IEnumerable<CardController> Selected => _selected.ToArray();

    public static void Clear() 
    {
        foreach (CardController card in _selected)
        {
            card.ClickController.OnClick.RemoveAllListeners();
        }
        _selected.Clear();
    }
    public static int Count => _selected.Count;

    public static void OnClick(CardController clicked)
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
}