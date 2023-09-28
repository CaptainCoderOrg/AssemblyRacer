using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - GoatAbility", menuName = "Assembly/Monster Abilities/BarnYardBaddies/Ability - Goat")]
public class GoatAbility : MonsterAbility
{

    public override void OnDefeat(MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.RecruitShiny();
        onAnimationCompelte.Invoke();
    }

}
