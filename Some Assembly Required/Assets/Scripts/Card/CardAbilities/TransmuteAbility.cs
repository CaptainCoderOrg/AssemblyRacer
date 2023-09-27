using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability - Transmute", menuName = "Assembly/Card Abilities/Gandork/Transmute")]
public class TransmuteAbility : CardAbility
{

    public override void OnDefeat(CardController card, MonsterCardController enemy, GameManager manager, System.Action onAnimationComplete)
    {
        manager.RecruitShiny(onAnimationComplete);
    }

}
