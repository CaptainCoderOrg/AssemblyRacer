using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Invest", menuName = "Assembly/Card Abilities/Odorf/Invest")]
public class InvestAbility : CardAbility
{

    public override bool CheckRequirement(CardController parent, List<CardController> selectedCards)
    {
        if (selectedCards.Count == 0) { return false; }
        foreach (CardController card in selectedCards)
        {
            if (card == parent) { return false; }
            if (card.Card.Gold == 0) { return false; }
        }
        return true;
    }

    public override void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {
        List<CardController> toDiscard = new List<CardController>(cards);
        foreach (CardController card in cards)
        {
            parent.BonusGold += card.Card.Gold*2;
        }
        
        
        manager.DiscardCards(toDiscard, () =>
        {
            CardSelectorManager.Clear();
            manager.SelectHandCard(parent);
        });
    }

}
