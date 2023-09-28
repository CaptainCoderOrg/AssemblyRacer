using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Card Ability", menuName = "Assembly/Card Ability")]
public class MonsterAbility : ScriptableObject
{
    public virtual void OnDraw(GameManager manager, System.Action onAnimationCompelte)
    {
        onAnimationCompelte.Invoke();
    }

    public virtual void OnDefeat(MonsterCardController enemy, GameManager manager, System.Action onAnimationCompelte)
    {
        onAnimationCompelte.Invoke();
    }

    public virtual void OnEscape(MonsterCardController enemy, GameManager manager, System.Action onAnimationCompelte)
    {
        onAnimationCompelte.Invoke();
    }

}
