using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Card Audio Database", menuName = "Assembly/Card Audio Database")]
public class CardAudioDatabase : ScriptableObject
{
    public AudioClip Shuffle;
    public AudioClip Draw;
    public AudioClip Click;
}
