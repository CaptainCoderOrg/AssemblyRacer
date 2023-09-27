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

    private void RegisterAbilityCards()
    {
        _playerDeckManager.EnableSelectionMode((card) =>
        {
            _selectedCard = card;
            _cardAbilityDialog.Card = card;
            _cardAbilityDialog.gameObject.SetActive(true);
            if (card.Card.Ability is not null)
            {
                _playerDeckManager.EnableSelectionMode(_cardAbilityDialog.OnClickCard);
            }
        });
    }

    public void ActivateAbility()
    {
        CardController card = _selectedCard;
        List<CardController> selectedCards = CardSelectorManager.Selected.ToList();
        if (card is null) { return; }
        if (card.Card.Ability is null) { return; }
        if (card.Card.Ability.CheckRequirement(selectedCards) is false) { return; }
        card.Card.Ability.ApplyAbility(card, selectedCards, this, _cardAbilityDialog);
    }

    private void HandleMonsterAttack(MonsterCardController attacker)
    {
        _wounds--;
        OnWoundsChangedString.Invoke(_wounds.ToString());
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
        _playerDeckManager.DiscardHand(() => { });
    }

    public void DrawMonster() => DrawMonster(() => { });

    public void DrawMonster(System.Action onAnimationFinished)
    {
        MonsterCardData drawn = _monsterDeckManager.DrawMonster();
        MonsterCardController card = _monsterTrack.AddMonster(drawn, onAnimationFinished);
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
        List<CardController> selected = CardSelectorManager.Selected.ToList();
        Unselect();
        if (ix < 0) { return; }
        _playerDeckManager.DiscardCards(selected, () =>
        {
            _monsterTrack.DefeatMonster(ix);
            // TODO: Run Monster OnDefeat
        });
    }

    public void DiscardCards(List<CardController> cards)
    {
        _playerDeckManager.DiscardCards(cards, () =>
        {
            CardSelectorManager.Clear();
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

    public void RecruitShiny()
    {

    }
}
