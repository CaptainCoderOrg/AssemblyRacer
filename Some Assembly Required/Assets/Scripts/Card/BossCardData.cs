using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Card Data", menuName = "Assembly/Boss Card Data")]
public class BossCardData : ScriptableObject
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
