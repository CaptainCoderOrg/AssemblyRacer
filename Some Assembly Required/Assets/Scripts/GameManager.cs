using System.Collections;
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
    [field: SerializeField]
    public MonsterCardController SelectedMonster { get; private set; }
    [field: SerializeField]
    public CardController SelectedRecruit { get; private set; }
    private PlayerDeckManager _playerDeckManager;
    private RecruitDeckManager _recruitDeckManager;
    private MonsterDeckManager _monsterDeckManager;
    public UnityEvent<string> OnWoundsChangedString;
    private int _wounds = 10;

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
        () => {
            StartTurn();
        }
        );
    }

    public void FillRecruitTrack(System.Action onAnimationComplete)
    {
        CardController card = null;
        card  = _recruitsTrack.AddRecruit(_recruitDeckManager, () =>
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
            _playerDeckManager.StartTurn(() => {});
        });
    }

    public void EndTurn()
    {
        _playerDeckManager.DiscardHand(() => {});
    }

    public void DrawMonster() => DrawMonster(() => {});

    public void DrawMonster(System.Action onAnimationFinished)
    {
        MonsterCardData drawn = _monsterDeckManager.DrawMonster();
        MonsterCardController card = _monsterTrack.AddMonster(drawn, onAnimationFinished);
        card.ClickController.OnClick.AddListener(() => SelectMonster(card));
    }

    public void SelectMonster(MonsterCardController monsterCard)
    {
        SelectedMonster = monsterCard;
        _monsterDialog.Card = monsterCard.Card;
        _monsterDialog.gameObject.SetActive(true);
        _recruitDialog.gameObject.SetActive(false);
    }

    public void SelectRecruit(CardController recruitCard)
    {
        SelectedRecruit = recruitCard;
        _recruitDialog.Card = recruitCard.Card;
        _recruitDialog.gameObject.SetActive(true);
        _monsterDialog.gameObject.SetActive(false);
    }

    public void Unselect()
    {
        SelectedMonster = null;
        _monsterDialog.gameObject.SetActive(false);
        SelectedRecruit = null;
        _recruitDialog.gameObject.SetActive(false);
    }

    public void DrawCard()
    {
        _playerDeckManager.DrawCard(() => {});
    }
}
