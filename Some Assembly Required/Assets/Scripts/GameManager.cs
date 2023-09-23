using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private HandController _handController;
    [SerializeField]
    private StarterDeckData _starterDeck;
    [field: SerializeField]
    public List<CardData> PlayerDeck { get; private set; }
    [field: SerializeField]
    public List<CardData> DiscardPile { get; private set; }
    [SerializeField]
    private Transform _discardPileLocation;


    void Start()
    {
        DiscardPile = new List<CardData>();
        InitializeMonsterDeck();
        InitializePlayerDeck();
    }

    public void StartTurn()
    {
        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
        DrawMonster();
    }

    public void EndTurn()
    {
        DiscardPile.AddRange(_handController.DiscardHand(_discardPileLocation).Select(card => card.Card));
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
    }

    private void InitializePlayerDeck()
    {
        PlayerDeck = new List<CardData>();
        foreach (StarterDeckData.Entry entry in _starterDeck.Cards)
        {
            for (int i = 0; i < entry.Count; i++)
            {
                PlayerDeck.Add(entry.Card);
            }
        }
        PlayerDeck.Shuffle();
    }

    // Update is called once per frame
    public void DrawMonster()
    {
        MonsterCardData drawn = MonsterDeck[MonsterDeck.Count - 1];
        MonsterDeck.RemoveAt(MonsterDeck.Count - 1);
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

    public void DrawCard()
    {
        CardData drawn = PlayerDeck[PlayerDeck.Count - 1];
        PlayerDeck.RemoveAt(PlayerDeck.Count - 1);
        CardController card = _handController.DrawCard(drawn);
    }
}
