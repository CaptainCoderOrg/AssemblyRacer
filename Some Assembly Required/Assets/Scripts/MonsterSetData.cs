using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Set", menuName = "Assembly/Monster Set")]
public class MonsterSetData : ScriptableObject
{
    public List<Entry> Cards;

    [Serializable]
    public class Entry
    {
        public MonsterCardData Card;
        public int Count;
    }
}


