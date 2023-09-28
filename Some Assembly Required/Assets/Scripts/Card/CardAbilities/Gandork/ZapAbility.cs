using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Zap", menuName = "Assembly/Card Abilities/Gandork/Zap")]
public class ZapAbility : CardAbility
{

    public override bool CheckRequirement(CardController parent, List<CardController> selectedCards)
    {
        if (parent.TimesUsed > 0) { return false; }
        if (selectedCards.Count != 1) { return false; }
        if (selectedCards[0] == parent) { return false; }        
        return selectedCards[0].Card.Magic > 0;
    }

    public override void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {
        
        manager.RemoveCardsFromGame(cards, () => {
            parent.TimesUsed++;
            parent.BonusMagic += 2;
            CardSelectorManager.Clear();
            manager.SelectHandCard(parent);
        });
        
    }

}
