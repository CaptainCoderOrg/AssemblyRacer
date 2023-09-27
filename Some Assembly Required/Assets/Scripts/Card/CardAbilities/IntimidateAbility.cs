using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Intimidate Card Ability", menuName = "Assembly/Card Abilities/Intimidate Card Ability")]
public class IntimidateCardAbility : CardAbility
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
        List<CardController> toDiscard = new List<CardController> { parent };
        foreach (CardController card in cards)
        {
            if (card.name == "Boo-boo")
            {
                toDiscard.Add(card);
                break;
            }
        }
        
        manager.Unselect();
        manager.DiscardCards(toDiscard);
        manager.DrawCards(3, () => {});
        
    }

}
