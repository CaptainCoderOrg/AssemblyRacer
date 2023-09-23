using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MonsterDeckManager : MonoBehaviour
{
    public UnityEvent<int> OnDeckSizeChange;
    public UnityEvent<string> OnDeckSizeChangeString;
    [field: SerializeField]
    public List<MonsterSetData> MonsterSets { get; private set; }
    [field: SerializeField]
    public List<MonsterCardData> MonsterDeck { get; private set; }

    void Start()
    {
        InitializeMonsterDeck();
    }
    
    private void InitializeMonsterDeck()
    {
        MonsterDeck = new List<MonsterCardData>();
        foreach (MonsterSetData set in MonsterSets)
        {
            foreach (MonsterSetData.Entry entry in set.Cards)
            {
                for (int i = 0; i < entry.Count; i++)
                {
                    MonsterDeck.Add(entry.Card);
                }
            }
        }
        MonsterDeck.Shuffle();
        OnDeckSizeChange.Invoke(MonsterDeck.Count);
        OnDeckSizeChangeString.Invoke(MonsterDeck.Count.ToString());
    }

    public MonsterCardData DrawMonster()
    {
        MonsterCardData drawn = MonsterDeck[MonsterDeck.Count - 1];
        MonsterDeck.RemoveAt(MonsterDeck.Count - 1);
        OnDeckSizeChange.Invoke(MonsterDeck.Count);
        OnDeckSizeChangeString.Invoke(MonsterDeck.Count.ToString());
        return drawn;
    }
}
