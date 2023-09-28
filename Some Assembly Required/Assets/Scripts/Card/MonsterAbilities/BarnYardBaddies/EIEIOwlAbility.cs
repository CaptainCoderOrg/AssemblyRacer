using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - EIEIOwl", menuName = "Assembly/Monster Abilities/BarnYardBaddies/Ability - EIEIOwl")]
public class EIEIOwlAbility : MonsterAbility
{

    public override void OnDraw(GameManager manager, Action onAnimationCompelte)
    {
        manager.DrawMonster(onAnimationCompelte);
    }


}
