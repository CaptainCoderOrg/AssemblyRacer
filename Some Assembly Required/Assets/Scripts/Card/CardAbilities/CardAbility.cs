using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Ability", menuName = "Assembly/Card Ability")]
public class CardAbility : ScriptableObject
{

    public virtual bool CheckRequirement(List<CardController> cards)
    {
        return false;
    }

    public virtual void ApplyAbility(CardController parent, List<CardController> cards, GameManager manager, CardAbilityDialog cardAbilityDialog)
    {

    }

}
