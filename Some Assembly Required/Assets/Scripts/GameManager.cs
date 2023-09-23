using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField]
    private MonsterTrackController _monsterTrack;
    [SerializeField]
    private MonsterDialogController _monsterDialog;
    [field: SerializeField]
    public List<MonsterSetData> MonsterSets { get; private set; }
    [field: SerializeField]
    public List<MonsterCardData> MonsterDeck { get; private set; }
    [field: SerializeField]
    public MonsterCardController SelectedMonster { get; private set; }

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
        MonsterCardController card = _monsterTrack.AddMonster(drawn);
        card.ClickController.OnClick.AddListener(() => SelectMonster(card));
    }

    public void SelectMonster(MonsterCardController monsterCard)
    {
        SelectedMonster = monsterCard;
        _monsterDialog.Card = monsterCard.Card;
        _monsterDialog.gameObject.SetActive(true);
    }

    public void DeselectMonster()
    {
        SelectedMonster = null;
        _monsterDialog.gameObject.SetActive(false);
    }
}
