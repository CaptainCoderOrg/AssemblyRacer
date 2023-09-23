using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    void Awake()
    {
        _playerDeckManager = GetComponent<PlayerDeckManager>();
        _monsterDeckManager = GetComponent<MonsterDeckManager>();
    }


    public void StartTurn()
    {
        _playerDeckManager.StartTurn();
        DrawMonster();
    }

    public void EndTurn()
    {
        _playerDeckManager.DiscardHand();
    }

    public void DrawMonster()
    {
        MonsterCardData drawn = _monsterDeckManager.DrawMonster();
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

    public void DrawCard() => _playerDeckManager.DrawCard();
}
