using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Starter Deck", menuName = "Assembly/Starter Deck")]
public class StarterDeckData : ScriptableObject
{
    public List<Entry> Cards;

    [Serializable]
    public class Entry
    {
        public CardData Card;
        public int Count;
    }
}


