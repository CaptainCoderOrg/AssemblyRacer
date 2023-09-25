using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerDeckManager), typeof(MonsterDeckManager))]
public class GameManager : MonoBehaviour
{

    [SerializeField]
    private MonsterTrackController _monsterTrack;
    [SerializeField]
    private MonsterDialogController _monsterDialog;
    [field: SerializeField]
    public MonsterCardController SelectedMonster { get; private set; }
    private PlayerDeckManager _playerDeckManager;
    private MonsterDeckManager _monsterDeckManager;
    public UnityEvent<string> OnWoundsChangedString;
    private int _wounds = 10;

    void Awake()
    {
        _playerDeckManager = GetComponent<PlayerDeckManager>();
        _monsterDeckManager = GetComponent<MonsterDeckManager>();
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


    public void StartTurn()
    {
        DrawMonster(() => 
        {
            _playerDeckManager.StartTurn();
        });
    }

    public void EndTurn()
    {
        _playerDeckManager.DiscardHand();
    }

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
    }

    public void DeselectMonster()
    {
        SelectedMonster = null;
        _monsterDialog.gameObject.SetActive(false);
    }

    public void DrawCard() => _playerDeckManager.DrawCard();
}
