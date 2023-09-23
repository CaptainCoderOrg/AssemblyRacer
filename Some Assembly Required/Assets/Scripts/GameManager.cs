using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private MonsterTrackController _monsterTrack;

    [field: SerializeField]
    public List<MonsterSetData> MonsterSets { get; private set; }

    [field: SerializeField]
    public List<MonsterCardData> MonsterDeck { get; private set; }

    void Start()
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
    }

    // Update is called once per frame
    public void DrawMonster()
    {
        MonsterCardData drawn = MonsterDeck[MonsterDeck.Count-1];
        MonsterDeck.RemoveAt(MonsterDeck.Count-1);
        _monsterTrack.AddMonster(drawn);
    }
}
