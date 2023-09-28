using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Coin Toss", menuName = "Assembly/Card Abilities/Odorf/Coin Toss")]
public class CoinTossAbility : CardAbility
{

    public override bool CheckRequirement(CardController parent, List<CardController> selectedCards)
    {
        if (selectedCards.Count != 1) { return false; }
        if (selectedCards[0] == parent) { return false; }
        if (selectedCards[0].Card.Gold == 0) { return false; }
        return true;
    }

    public override void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {
        List<CardController> toDiscard = new List<CardController>(cards);
        parent.BonusDamage += 2;
        manager.RemoveCardsFromGame(cards, () =>
        {
            CardSelectorManager.Clear();
            manager.SelectHandCard(parent);
        });
    }

}
