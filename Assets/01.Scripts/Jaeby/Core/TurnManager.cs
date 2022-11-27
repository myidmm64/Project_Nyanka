using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class TurnManager : MonoSingleTon<TurnManager>
{
    [SerializeField]
    private TextMeshProUGUI _whoseTurnText = null;
    [SerializeField]
    private TextMeshProUGUI _currentTurnText = null;

    private List<BaseMainModule> _entitys;
    private List<PlayerMainModule> _players;
    public List<PlayerMainModule> Players => _players;
    private List<AIMainModule> _enemys;
    public List<AIMainModule> Enemys => _enemys;

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

    public UnityEvent<PlayerMainModule> OnPlusTurn = null;
    public UnityEvent<PlayerMainModule> OnLoseTurn = null;
    public UnityEvent<int> OnStarted = null;
    public UnityEvent<bool> OnNextPhase = null;
    public UnityEvent<int> OnNextTurn = null;
    public UnityEvent<int> OnBattlePointUp = null;

    private List<TurnAction> _turnActions = new List<TurnAction>();

    private int _playerTurnCount = 0;
    private int PlayerTurnCount
    {
        get => _playerTurnCount;
        set
        {
            _playerTurnCount = value;
        }
    }
    private bool _plusTurn = false;
    private bool _loseTurn = false;

    private int _maxPoint = 0;
    public int MaxPoint => _maxPoint;

    private void Awake()
    {
        _entitys = new List<BaseMainModule>(FindObjectsOfType<BaseMainModule>());
        _players = new List<PlayerMainModule>(FindObjectsOfType<PlayerMainModule>());
        _enemys = new List<AIMainModule>(FindObjectsOfType<AIMainModule>());
    }

    private void Start()
    {
        //for (int i = 0; i < _entitys.Count; i++)
        //    OnNextPhase.AddListener(_entitys[i].PhaseChanged);
        OnNextPhase?.Invoke(true);
        OnStarted?.Invoke(1);
        PlayerTurnCount = GetLiveCount(_players);

        _maxPoint = UIManager.Instance.GetComponentInChildren<AttackPointUI>().MaxAttackPoint;
    }

    private void NewTurnReset()
    {
        _plusTurn = false;
        _loseTurn = false;
    }

    public void UseTurn(int count, PlayerMainModule player)
    {
        ClickManager.Instance.ClickManagerReset();
        PlayerTurnCount -= count;
        if (PlayerTurnCount <= 0)
            EnemyPhase();
        player.MyTurnEnd();
        UIManager.Instance.UIDisable();
    }

    public void EnemyPhase()
    {
        OnNextPhase?.Invoke(false);

        StartCoroutine(EnemysTurn());
    }

    private IEnumerator EnemysTurn()
    {
        //Debug.Log("야몸ㄴ놈너머");
        yield return new WaitForSecondsRealtime(1f);
        UIManager.Instance.TargettingUIEnable(false, false);
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        List<AIMainModule> liveEnemys = _enemys.FindAll(v => v.IsLived);

        for (int i = 0; i < liveEnemys.Count; i++)
        {
            Debug.Log(liveEnemys[i].name);
            yield return StartCoroutine(liveEnemys[i].GetComponent<BehaviorTree.Tree>().StartAI());
        }
        EntityManager.Instance.enemy_TargetLists.Clear();
        NextTurn();
        UIManager.Instance.TargettingUIEnable(true, false);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
    }

    private void NextTurn()
    {
        PlayerTurnCount = GetLiveCount(_players);
        _turn++;
        OnNextTurn?.Invoke(_turn);
        List<PlayerMainModule> livePlayers = _players.FindAll(v => v.IsLived);
        for (int i = 0; i < livePlayers.Count; i++)
            livePlayers[i].PhaseChange(PhaseType.Player);
        OnNextPhase?.Invoke(true);
        TurnActionCheck();
        NewTurnReset();
    }

    public void BattlePointChange(int val)
    {
        _battlePoint = val;
        _battlePoint = Mathf.Clamp(_battlePoint, 0, _maxPoint);
        OnBattlePointUp?.Invoke(_battlePoint);
    }

    public void CurrentTurnTextChange(int val)
    {
        _currentTurnText.SetText($"{val} ROUND");
        _currentTurnText.transform.DOKill();
        _currentTurnText.transform.localScale = Vector3.one * 1.5f;
        _currentTurnText.transform.DOScale(1f, 0.2f);
    }

    public void WhoseTurnTextChange(bool val)
    {
        if (val)
            _whoseTurnText.SetText("플레이어 턴");
        else
            _whoseTurnText.SetText("적 턴");
        _whoseTurnText.transform.DOKill();
        _whoseTurnText.transform.localScale = Vector3.one * 1.5f;
        _whoseTurnText.transform.DOScale(1f, 0.2f);
    }

    private int GetLiveCount(List<PlayerMainModule> entitys)
    {
        return entitys.FindAll(x => x.IsLived).Count;
    }
    private int GetLiveCount(List<AIMainModule> entitys)
    {
        return entitys.FindAll(x => x.IsLived).Count;
    }

    public void PressTurnCheck(PlayerMainModule player)
    {
        if (player.PressTurnChecked)
        {
            UseTurn(1, player);
            return;
        }
        if (_plusTurn)
        {
            player.PressTurnChecked = true;
            player.MoveModule.Moveable = true;
            StartCoroutine(WaitCoroutine(true, player));
            return;
        }
        if (_loseTurn)
        {
            PlayerTurnCount = 0;
            OnLoseTurn?.Invoke(player);
            StartCoroutine(WaitCoroutine(false, player));
            return;
        }
        UseTurn(1, player);
    }

    private IEnumerator WaitCoroutine(bool isPlus, PlayerMainModule player)
    {
        if (isPlus)
            OnPlusTurn?.Invoke(player);
        else
            OnLoseTurn?.Invoke(player);
        yield return new WaitForSecondsRealtime(1f);
        if(isPlus)
            ClickManager.Instance.ForceSelect(player);
        else
            EnemyPhase();
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

    public void TurnActionAdd(TurnAction turnAction, bool locked)
    {
        for (int i = 0; i < _turnActions.Count; i++)
            if (_turnActions[i] == turnAction)
                return;
        _turnActions.Add(turnAction);
        turnAction.Locked = locked;
    }

    private void TurnActionCheck()
    {
        for(int i = 0; i < _turnActions.Count; i++)
            if (_turnActions[i]?.Locked == false)
                _turnActions[i].TryCallback();
    }
}

public class TurnAction
{
    private Action _callback = null;
    private Action _startAction = null;
    private Action<int> _countChangeAction = null;
    private int _maxCount = 0;
    private int _count = 0;
    private bool _locked = true;
    public bool Locked { get => _locked; set => _locked = value; }
    public int Count { get => _count; set => _count = value; }

    public TurnAction(int cnt, Action StartAction, Action Callback, Action<int> CountChangeAction)
    {
        _count = cnt;
        _maxCount = _count;
        _startAction = StartAction;
        _callback = Callback;
        _startAction?.Invoke();
        _countChangeAction = CountChangeAction;
    }

    public void TryCallback()
    {
        _count--;
        _countChangeAction?.Invoke(_count);
        if (_count <= 0)
        {
            _locked = true;
            _callback?.Invoke();
        }
    }

    public void Start()
    {
        _startAction?.Invoke();
        _count = _maxCount;
        _locked = false;
    }
}