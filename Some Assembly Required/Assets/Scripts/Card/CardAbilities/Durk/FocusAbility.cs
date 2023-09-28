using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Focus", menuName = "Assembly/Card Abilities/Durk/Focus")]
public class FocusAbility : CardAbility
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
            manager.DrawCards(2, () =>
            {

            });
        });
    }

}
