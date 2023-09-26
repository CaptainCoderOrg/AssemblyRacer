using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recruit Set", menuName = "Assembly/Recruit Set")]
public class RecruitSetData : ScriptableObject
{
    public List<Entry> Cards;

    [Serializable]
    public class Entry
    {
        public CardData Card;
        public int Count;
    }
}


