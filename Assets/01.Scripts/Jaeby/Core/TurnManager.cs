using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoSingleTon<TurnManager>
{
    [SerializeField]
    private TextMeshProUGUI _playerTurnText = null;
    [SerializeField]
    private TextMeshProUGUI _whoseTurnText = null;
    [SerializeField]
    private TextMeshProUGUI _currentTurnText = null;
    [SerializeField]
    private TextMeshProUGUI _battlePointText = null;

    private List<Entity> _entitys;
    private List<Player> _players;
    public List<Player> Players => _players;
    private List<Enemy> _enemys;
    public List<Enemy> Enemys => _enemys;

    [SerializeField]
    private int _turn = 1;
    [SerializeField]
    private int _battlePoint = 0;
    public int BattlePoint
    {
        get => _battlePoint;
        set => _battlePoint = value;
    }

    private bool _isTransed = false;

    public UnityEvent OnPlusTurn = null;
    public UnityEvent OnLoseTurn = null;
    public UnityEvent<int> OnStarted = null;
    public UnityEvent<bool> OnNextPhase = null;
    public UnityEvent<int> OnNextTurn = null;
    public UnityEvent<int> OnBattlePointUp = null;


    private int _playerTurnCount = 0;
    private int PlayerTurnCount
    {
        get => _playerTurnCount;
        set
        {
            _playerTurnCount = value;
            _playerTurnText?.SetText($"useable Turn : {_playerTurnCount}");
        }
    }
    private bool _plusTurn = false;
    private bool _loseTurn = false;

    private void Awake()
    {
        _entitys = new List<Entity>(FindObjectsOfType<Entity>());
        _players = new List<Player>(FindObjectsOfType<Player>());
        _enemys = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    private void Start()
    {
        for (int i = 0; i < _entitys.Count; i++)
            OnNextPhase.AddListener(_entitys[i].PhaseChanged);
        OnNextPhase?.Invoke(true);
        OnStarted?.Invoke(1);
        PlayerTurnCount = GetLiveCount(_players);
    }

    private void NewTurnReset()
    {
        _plusTurn = false;
        _loseTurn = false;
    }

    public void UseTurn(int count, Player player)
    {
        PlayerTurnCount -= count;
        if (PlayerTurnCount <= 0)
            EnemyPhase();
        player.MyTurnEnd();
        UIManager.Instance.UIDisable();
        ClickManager.Instance.ClickManagerReset();
    }

    public void EnemyPhase()
    {
        OnNextPhase?.Invoke(false);

        StartCoroutine(EnemysTurn());
    }

    private IEnumerator EnemysTurn()
    {
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        List<Enemy> liveEnemys = _enemys.FindAll(v => v.IsLived);

        for (int i = 0; i < liveEnemys.Count; i++)
        {
            yield return StartCoroutine(liveEnemys[i].GetComponent<WarriorAIBT>().StartAI());
        }
        NextTurn();
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
    }

    private void NextTurn()
    {
        PlayerTurnCount = GetLiveCount(_players);
        _turn++;
        OnNextTurn?.Invoke(_turn);
        List<Player> livePlayers = _players.FindAll(v => v.IsLived);
        for (int i = 0; i < livePlayers.Count; i++)
            livePlayers[i].PlayerTurnStart();
        OnNextPhase?.Invoke(true);
        NewTurnReset();
    }

    public void BattlePointChange(int val)
    {
        _battlePoint = val;
        _battlePoint = Mathf.Clamp(_battlePoint, 0, 8);
        OnBattlePointUp?.Invoke(_battlePoint);
    }

    public void CurrentTurnTextChange(int val)
    {
        _currentTurnText.SetText($"Current Turn : {val}");
    }

    public void BattlePointTextChange(int val)
    {
        _battlePointText.SetText($"Battle Point : {val}");
    }

    public void WhoseTurnTextChange(bool val)
    {
        if (val)
            _whoseTurnText.SetText("player's Turn");
        else
            _whoseTurnText.SetText("enemy's Turn");
    }

    private int GetLiveCount(List<Player> entitys)
    {
        return entitys.FindAll(x => x.IsLived).Count;
    }
    private int GetLiveCount(List<Enemy> entitys)
    {
        return entitys.FindAll(x => x.IsLived).Count;
    }

    public void PressTurnCheck(Player player)
    {
        if (player.PressTurnChecked)
        {
            UseTurn(1, player);
            return;
        }
        if (_plusTurn)
        {
            player.PressTurnChecked = true;
            player.Moveable = true;
            OnPlusTurn?.Invoke();
            ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, true);
            ClickManager.Instance.ForceSelect(player);
            return;
        }
        if (_loseTurn)
        {
            PlayerTurnCount = 0;
            OnLoseTurn?.Invoke();
            EnemyPhase();
            return;
        }
        UseTurn(1, player);
    }

    public void LoseTurnCheck()
    {
        _loseTurn = true;
    }
    public void PlusTurnCheck()
    {
        _plusTurn = true;
    }

    public void TurnCheckReset()
    {
        _loseTurn = false;
        _plusTurn = false;
    }
}