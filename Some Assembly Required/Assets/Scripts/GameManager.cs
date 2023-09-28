using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerDeckManager), typeof(MonsterDeckManager), typeof(RecruitDeckManager))]
public class GameManager : MonoBehaviour
{

    [SerializeField]
    private MonsterTrackController _monsterTrack;
    [SerializeField]
    private RecruitsTrackController _recruitsTrack;
    [SerializeField]
    private MonsterDialogController _monsterDialog;
    [SerializeField]
    private RecruitDialogController _recruitDialog;
    [SerializeField]
    private CardAbilityDialog _cardAbilityDialog;
    [field: SerializeField]
    public MonsterCardController SelectedMonster { get; private set; }
    private int _selectedMonsturIx = -1;
    [field: SerializeField]
    public CardController SelectedRecruit { get; private set; }
    private int _selectedRecruitIx = -1;
    private PlayerDeckManager _playerDeckManager;
    private RecruitDeckManager _recruitDeckManager;
    private MonsterDeckManager _monsterDeckManager;
    public UnityEvent<string> OnWoundsChangedString;
    public UnityEvent<int> OnWoundsChanged;
    private int _wounds = 10;
    [SerializeField]
    private CardController _selectedCard;

    void Awake()
    {
        _playerDeckManager = GetComponent<PlayerDeckManager>();
        _monsterDeckManager = GetComponent<MonsterDeckManager>();
        _recruitDeckManager = GetComponent<RecruitDeckManager>();
    }

    void Start()
    {
        _monsterTrack.OnMonsterAttack.AddListener(HandleMonsterAttack);
        _monsterTrack.OnMonsterAttackFinished.AddListener(HandleMonsterAttackFinished);
        CardSelectorManager.OnCleared += RegisterAbilityCards;
        
    }

    public void AddBooBoo(System.Action onAnimationFinished)
    {
        _wounds--;
        OnWoundsChangedString.Invoke(_wounds.ToString());
        OnWoundsChanged.Invoke(_wounds);
        CardController wound = _monsterTrack.AddBooBoo(onAnimationFinished);
        _playerDeckManager.AddCardToDiscard(wound);
        // TODO: Check if game over
        

    }

    public void AddBooBoos(int count)
    {
        if (count == 0)
        {
            return;
        }
        AddBooBoo(() => AddBooBoos(count - 1));
    }

    public void RegisterAbilityCards()
    {
        _playerDeckManager.EnableSelectionMode(SelectHandCard);
    }

    public void SelectHandCard(CardController card)
    {
        _selectedCard = card;
        _cardAbilityDialog.Card = card;
        _cardAbilityDialog.gameObject.SetActive(true);
        if (card.Card.Ability is not null)
        {
            _playerDeckManager.EnableSelectionMode(_cardAbilityDialog.OnClickCard);
        }
    }

    public void ActivateAbility()
    {
        CardController card = _selectedCard;
        List<CardController> selectedCards = CardSelectorManager.Selected.ToList();
        if (card is null) { return; }
        if (card.Card.Ability is null) { return; }
        if (card.Card.Ability.CheckRequirement(card, selectedCards) is false) { return; }
        card.Card.Ability.ApplyAbility(card, selectedCards, this, _cardAbilityDialog);
    }

    private void HandleMonsterAttack(MonsterCardController attacker)
    {
        _wounds--;
        OnWoundsChangedString.Invoke(_wounds.ToString());
        OnWoundsChanged.Invoke(_wounds);
    }

    private void HandleMonsterAttackFinished((MonsterCardController attacker, CardController wound) evt)
    {
        _playerDeckManager.AddCardToDiscard(evt.wound);
        // TODO: If wounds are 0, end the game.
    }

    public void SetupBoard()
    {
        FillRecruitTrack(
        () =>
        {
            StartTurn();
        }
        );
    }

    public void FillRecruitTrack(System.Action onAnimationComplete)
    {
        CardController card = null;
        card = _recruitsTrack.AddRecruit(_recruitDeckManager, () =>
        {
            if (card is not null)
            {
                FillRecruitTrack(onAnimationComplete);
            }
            else
            {
                onAnimationComplete.Invoke();
            }
        });
        card?.ClickController.OnClick.AddListener(() => SelectRecruit(card));
    }


    public void StartTurn()
    {
        DrawMonster(() =>
        {
            _playerDeckManager.StartTurn(() =>
            {
                RegisterAbilityCards();
            });
        });
    }

    public void EndTurn()
    {
        _playerDeckManager.DiscardHand(() => { 
            StartTurn();
        });
    }

    public void DrawMonster() => DrawMonster(() => { });

    public void DrawMonster(System.Action onAnimationFinished)
    {
        MonsterCardData drawn = _monsterDeckManager.DrawMonster();
        MonsterCardController card = _monsterTrack.AddMonster(drawn, 
            () => 
            {
                if(drawn.Ability is not null)
                {
                    drawn.Ability.OnDraw(this, onAnimationFinished);
                }
                else
                {
                    onAnimationFinished.Invoke();
                }
            }
            );
        card.ClickController.OnClick.AddListener(() => SelectMonster(card));
    }

    public void SelectMonster(MonsterCardController monsterCard)
    {
        if (_monsterTrack.TryFindMonsterIx(monsterCard, out int ix))
        {
            SelectMonster(ix);
        }
    }


    public void SelectMonster(int ix)
    {
        if (_monsterTrack.TrySelectMonster(ix, out MonsterCardController monsterCard))
        {
            SelectedMonster = monsterCard;
            _monsterDialog.Card = monsterCard.Card;
            _monsterDialog.gameObject.SetActive(true);
            _recruitDialog.gameObject.SetActive(false);
            _selectedMonsturIx = ix;
            _playerDeckManager.EnableSelectionMode(_monsterDialog.OnClickCard);
        }
    }

    public void SelectRecruit(CardController card)
    {
        if (_recruitsTrack.TryFindRecruitIx(card, out int ix))
        {
            SelectRecruit(ix);
        }
    }

    public void SelectRecruit(int ix)
    {
        CardController recruitCard = _recruitsTrack.Recruits[ix];
        SelectedRecruit = recruitCard;
        _selectedRecruitIx = ix;
        _recruitDialog.Card = recruitCard.Card;
        _recruitDialog.gameObject.SetActive(true);
        _monsterDialog.gameObject.SetActive(false);
        _playerDeckManager.EnableSelectionMode(_recruitDialog.OnClickCard);

    }

    public void Unselect()
    {
        SelectedMonster = null;
        _monsterDialog.gameObject.SetActive(false);
        _selectedMonsturIx = -1;
        SelectedRecruit = null;
        _recruitDialog.gameObject.SetActive(false);
        _selectedRecruitIx = -1;
        _playerDeckManager.DisableSelectMode();
        _cardAbilityDialog.gameObject.SetActive(false);
        _selectedCard = null;
        CardSelectorManager.Clear();
    }

    public void HireRecruit()
    {
        int ix = _selectedRecruitIx;
        List<CardController> selected = CardSelectorManager.Selected.ToList();
        Unselect();
        if (ix < 0) { return; }
        _playerDeckManager.DiscardCards(selected, () =>
        {
            Recruit(ix);
        });
    }

    public void DefeatMonster()
    {
        int ix = _selectedMonsturIx;
        MonsterCardController monster = SelectedMonster;
        List<CardController> selected = CardSelectorManager.Selected.ToList();
        List<Action<Action>> onDefeatEffects = new();
        int woundsGained = 0;
        foreach (CardController card in selected)
        {
            woundsGained += card.Card.Wounds;
            if (card.Card.Ability is not null)
            {
                onDefeatEffects.Add((onAnimationFinished) =>
                {
                    card.Card.Ability.OnDefeat(card, monster, this, onAnimationFinished);
                });
            }
        }
        Unselect();
        if (ix < 0) { return; }


        _playerDeckManager.DiscardCards(selected, () =>
        {
            _monsterTrack.DefeatMonster(ix, () =>
            {
                Action action = () => {
                    ApplyDefeatEffects(onDefeatEffects, () =>
                    {
                        AddBooBoos(woundsGained);
                    });
                };
                if (monster.Card.Ability is not null)
                {
                    monster.Card.Ability.OnDefeat(monster, this, action);
                }
                else
                {
                    action.Invoke();
                }
                
            });

        });
    }

    private void ApplyDefeatEffects(List<Action<Action>> effects, System.Action onAnimationComplete)
    {
        if (effects.Count == 0)
        {
            onAnimationComplete.Invoke();
            return;
        }
        var action = effects[effects.Count - 1];
        effects.RemoveAt(effects.Count - 1);
        action.Invoke(() => ApplyDefeatEffects(effects, onAnimationComplete));
    }

    public void DiscardCards(List<CardController> cards, System.Action onAnimationComplete)
    {
        _playerDeckManager.DiscardCards(cards, () =>
        {
            CardSelectorManager.Clear();
            onAnimationComplete.Invoke();
        });
    }

    public void DrawCards(int count, System.Action onAnimationFinished)
    {
        _playerDeckManager.DrawCards(count, () =>
        {
            RegisterAbilityCards();
            onAnimationFinished.Invoke();
        });
    }

    public void Recruit(int ix)
    {
        _recruitsTrack.AddRecruitToDiscard(ix, _playerDeckManager,
        () =>
            {
                FillRecruitTrack(() => { });
            }
        );
    }

    public void RejectRecruits()
    {
        Unselect();
        _recruitsTrack.RejectRecruits(() =>
        {
            FillRecruitTrack(() => {});
        });

    }

    public void RecruitShiny() => RecruitShiny(() => { });

    public void RecruitShiny(System.Action onAnimationComplete)
    {
        _recruitsTrack.AddShiniesToDiscard(_playerDeckManager, () =>
        {
            onAnimationComplete.Invoke();
            RegisterAbilityCards();
        });

    }

    internal void AddBooBooToHand(Action onAnimationComplete)
    {
        _wounds--;
        OnWoundsChangedString.Invoke(_wounds.ToString());
        OnWoundsChanged.Invoke(_wounds);
        
        _playerDeckManager.AddWoundToHand(() =>
        {
            onAnimationComplete.Invoke();
            RegisterAbilityCards();
        });
    }

    internal void RemoveCardsFromGame(List<CardController> cards, Action onAnimationComplete)
    {
        _playerDeckManager.RemoveCardsFromGame(cards, onAnimationComplete);
    }

    internal void DiscardHand(Action onAnimationComplete)
    {
        _playerDeckManager.DiscardHand(onAnimationComplete);
    }

    internal void ReturnCardFromDiscardToHand(CardController card, System.Action onAnimationComplete)
    {
        _playerDeckManager.ReturnCardFromDiscardToHand(card, onAnimationComplete);
    }

    
}
