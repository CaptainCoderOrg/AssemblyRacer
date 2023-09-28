using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Porkington", menuName = "Assembly/Monster Abilities/BarnYardBaddies/Ability - Porkington")]
public class PorkingtonAbility : MonsterAbility
{

    public override void OnDefeat(MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.DrawCards(1, onAnimationCompelte);
    }

}
