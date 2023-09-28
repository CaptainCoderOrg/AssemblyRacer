using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Unstoppable", menuName = "Assembly/Card Abilities/Durk/Unstoppable")]
public class UnstoppableAbility : CardAbility
{

    public override bool CheckRequirement(CardController parent, List<CardController> selectedCards) => true;

    public override void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {
        manager.Unselect();
        manager.DiscardHand(() => {
            manager.DrawCards(6, () => {});
        });
    }

    public override void OnDefeat(CardController card, MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.ReturnCardFromDiscardToHand(card, () => {
            // manager.RegisterAbilityCards();
        });
    }

}
