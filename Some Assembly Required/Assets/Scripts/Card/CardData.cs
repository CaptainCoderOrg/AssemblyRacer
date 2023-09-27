using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "Assembly/Card Data")]
public class CardData : ScriptableObject
{
    public Sprite Art;
    public Sprite UIArt;
    public string Name;
    public int Attack;
    public int Gold;
    public int Magic;
    public int Cost;
    public int Wounds;
    [TextArea]
    public string AbilityText;
    [TextArea]
    public string FlavorText;
    public CardAbility Ability;

}
