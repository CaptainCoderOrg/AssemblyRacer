using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Card Data", menuName = "Assembly/Monster Card Data")]
public class MonsterCardData : ScriptableObject
{
    public Sprite Art;
    public Sprite UIArt;
    public string Name;
    public int HitPoints;
    public int MagicPoints;
    [TextArea]
    public string AbilityText;
    [TextArea]
    public string FlavorText;

}
