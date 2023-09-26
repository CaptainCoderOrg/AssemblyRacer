using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RecruitDeckManager : MonoBehaviour
{
    public UnityEvent<int> OnDeckSizeChange;
    public UnityEvent<string> OnDeckSizeChangeString;
    [field: SerializeField]
    public List<RecruitSetData> CardSets { get; private set; }
    [field: SerializeField]
    public List<CardData> RecruitDeck { get; private set; }

    void Start()
    {
        InitializeRecruitDeck();
    }
    
    private void InitializeRecruitDeck()
    {
        RecruitDeck = new List<CardData>();
        foreach (RecruitSetData set in CardSets)
        {
            foreach (RecruitSetData.Entry entry in set.Cards)
            {
                for (int i = 0; i < entry.Count; i++)
                {
                    RecruitDeck.Add(entry.Card);
                }
            }
        }
        RecruitDeck.Shuffle();
        OnDeckSizeChange.Invoke(RecruitDeck.Count);
        OnDeckSizeChangeString.Invoke(RecruitDeck.Count.ToString());
    }

    public CardData DrawRecruit()
    {
        CardData drawn = RecruitDeck[RecruitDeck.Count - 1];
        RecruitDeck.RemoveAt(RecruitDeck.Count - 1);
        OnDeckSizeChange.Invoke(RecruitDeck.Count);
        OnDeckSizeChangeString.Invoke(RecruitDeck.Count.ToString());
        return drawn;
    }
}
