using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Intimidate", menuName = "Assembly/Card Abilities/Bulk/Intimidate Card Ability")]
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
        manager.DiscardCards(toDiscard, () =>
        {
            manager.DrawCards(3, () => { });
        });

    }

    public override void OnDefeat(CardController card, MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.DrawCards(1, () => { });
    }

}
