using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Tuna", menuName = "Assembly/Monster Abilities/BarnYardBaddies/Ability - Tuna")]
public class TunaAbility : MonsterAbility
{

    public override void OnDefeat(MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.DiscardHand(onAnimationCompelte);
    }

}
