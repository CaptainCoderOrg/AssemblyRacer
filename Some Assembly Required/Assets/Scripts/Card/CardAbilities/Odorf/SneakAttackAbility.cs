using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Sneak Attack", menuName = "Assembly/Card Abilities/Odorf/Sneak Attack")]
public class SneakAttackAbility : CardAbility
{

    public override bool CheckRequirement(CardController parent, List<CardController> selectedCards) => true;

    public override void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {
        List<CardController> toDiscard = new List<CardController>{parent};
        
        
        manager.DiscardCards(toDiscard, () =>
        {
            manager.DrawCards(3, () => {});
        });
    }

    public override void OnDefeat(CardController card, MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.DrawCards(3, () => {});
    }

}
