using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Clobber", menuName = "Assembly/Card Abilities/Clobber Card Ability")]
public class ClobberAbility : CardAbility
{

    public override bool CheckRequirement(List<CardController> selectedCards)
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
        
        manager.DiscardCards(toDiscard);
        parent.BonusDamage += 5;
        manager.SelectHandCard(parent);
        
    }

}
