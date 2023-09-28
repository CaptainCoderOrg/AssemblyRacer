using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Rage", menuName = "Assembly/Card Abilities/Bulk/Ability - Rage")]
public class RageAbility : CardAbility
{

    public override void OnDefeat(CardController card, MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.AddBooBooToHand(() =>
        {

        });
    }

}
