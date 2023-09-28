using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Smash", menuName = "Assembly/Card Abilities/Bulk/Smash")]
public class SmashAbility : CardAbility
{

    public override bool CheckRequirement(CardController parent, List<CardController> selectedCards)
    {
        foreach (CardController card in selectedCards)
        {
            if (card.name == "Boo-boo")
            {
                return true;
            }
        }
        return false;
    }

    public override void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {
        List<CardController> toDiscard = new List<CardController> { };
        foreach (CardController card in cards)
        {
            if (card.name == "Boo-boo")
            {
                toDiscard.Add(card);
                break;
            }
        }

        parent.BonusDamage += 2;
        manager.DiscardCards(toDiscard, () =>
        {
            CardSelectorManager.Clear();
            manager.SelectHandCard(parent);
        });

    }


}
