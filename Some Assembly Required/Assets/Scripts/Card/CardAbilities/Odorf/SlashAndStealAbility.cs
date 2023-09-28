using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Slash N Steal", menuName = "Assembly/Card Abilities/Odorf/Slash N Steal")]
public class SlashAndStealAbility : CardAbility
{

    public override void OnDefeat(CardController card, MonsterCardController enemy, GameManager manager, Action onAnimationCompelte)
    {
        manager.RecruitShiny(() => {});
    }

}
