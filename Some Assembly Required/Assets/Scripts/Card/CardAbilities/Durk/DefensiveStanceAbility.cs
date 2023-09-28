using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Defensive Stance", menuName = "Assembly/Card Abilities/Durk/Defensive Stance")]
public class DefensiveStanceAbility : CardAbility
{

    public override bool CheckRequirement(CardController card, List<CardController> selectedCards)
    {
        return true;
    }

    public override void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {
        List<CardController> toDiscard = new List<CardController> { parent };
        manager.Unselect();
        manager.DiscardCards(toDiscard, () =>
        {
            manager.DrawCards(1, () =>
            {

            });
        });
    }

}
