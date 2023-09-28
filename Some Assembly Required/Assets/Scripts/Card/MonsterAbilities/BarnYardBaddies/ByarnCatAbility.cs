using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - ByarnCat", menuName = "Assembly/Monster Abilities/BarnYardBaddies/Ability - ByarnCar")]
public class ByarnCatAbility : MonsterAbility
{

    public override void OnDefeat(MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.AddBooBoo(onAnimationCompelte);
    }

}
