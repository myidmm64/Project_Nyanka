using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;

public class TurnManager : MonoSingleTon<TurnManager>
{
    [SerializeField]
    private TextMeshProUGUI _whoseTurnText = null;
    [SerializeField]
    private TextMeshProUGUI _currentTurnText = null;


    public Dictionary<Vector3Int, int> enemy_TargetLists = new Dictionary<Vector3Int, int>();

    [SerializeField]
    private int _turn = 1;
    [SerializeField]
    private int _battlePoint = 0;
    public int BattlePoint
    {
        get => _battlePoint;
        set => _battlePoint = value;
    }

    [ContextMenu("살아있는 플레이어")]
    public void TextMonster()
    {
        foreach(PlayerMainModule player in GameManager.Instance.LivePlayers)
        {
            Debug.Log(player.name);
        }
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
    private bool _gameEnded = false;

    private int _maxPoint = 0;
    public int MaxPoint
    {
        get => _maxPoint;
        set => _maxPoint = value;
    }

    private void Start()
    {
        OnNextPhase?.Invoke(true);
        OnStarted?.Invoke(1);
        PlayerTurnCount = GetLiveCount(GameManager.Instance.Players);
    }

    public void GameEnd()
    {
        _gameEnded = true;
        if (GameManager.Instance.LiveEnemys.Count == 0)
            DialogSystem.Instance.ClearDialog();
        else if (GameManager.Instance.LivePlayers.Count == 0)
            DialogSystem.Instance.FailDialog();
    }

    public void GameRestart()
    {
        _turn = 0;
        _gameEnded = false;
        NextTurn();
    }

    public bool GameEndCheck(EntityType entityType)
    {
        if (entityType == EntityType.Enemy)
            return GameManager.Instance.LiveEnemys.Count == 0;
        else if (entityType == EntityType.Player)
            return GameManager.Instance.LivePlayers.Count == 0;
        return false;
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
        player.MyTurnEnd();
        int selectableCount = GameManager.Instance.LivePlayers.FindAll(x => x.Selectable).Count;
        if (selectableCount <= 0)
            EnemyPhase();
        UIManager.Instance.UIDisable();
    }

    public void EnemyPhase()
    {
        if (_gameEnded)
            return;

        OnNextPhase?.Invoke(false);
        StartCoroutine(EnemysTurn());
    }

    private IEnumerator EnemysTurn()
    {
        //Debug.Log("야몸ㄴ놈너머");
        yield return new WaitForSecondsRealtime(1f);
        UIManager.Instance.TargettingUIEnable(false, true);
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        List<AIMainModule> liveEnemys = GameManager.Instance.Enemys.FindAll(v => v.IsLived);
        List<PlayerMainModule> livePlayers = GameManager.Instance.Players.FindAll(v => v.IsLived);
        for (int i = 0; i < livePlayers.Count; i++)
        {
            if(livePlayers[i] != null)
                livePlayers[i].PhaseChange(PhaseType.Enemy);
        }
        //플레이어와 거리가 짧은 ai 먼저 우선 실행되게 바꾸기
        //일단 이중포문으로 각 ai마다 가장 짧은 플레이어와의 거리를 계산하고 리스트에 넣기
        //그리고 실행
        Dictionary<AIMainModule, int> enemys = new Dictionary<AIMainModule, int>();
        foreach(var enemy in liveEnemys)
        {
            int m_dis = 100000;
            foreach(var player in livePlayers)
            {
                int tempX = Mathf.Abs(enemy.CellIndex.x - player.CellIndex.x) / enemy.Int_MoveRange;
                int tempZ = Mathf.Abs(enemy.CellIndex.z - player.CellIndex.z) / enemy.Int_MoveRange;
                int dis = (tempX > tempZ) ? tempX : tempZ;
                if (m_dis > dis)
                    m_dis = dis;
            }
            enemys.Add(enemy, m_dis);
        }
        enemys = enemys.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        foreach(var enemy in enemys)
        {
            Debug.Log(enemy.Key.name + " " + enemy.Value);
            yield return StartCoroutine(enemy.Key.GetComponent<BehaviorTree.Tree>().StartAI());
            yield return new WaitForSeconds(1f);
        }
        //for (int i = 0; i < liveEnemys.Count; i++)
        //{
        //    Debug.Log(liveEnemys[i].name);
        //    yield return StartCoroutine(liveEnemys[i].GetComponent<BehaviorTree.Tree>().StartAI());
        //}
        UIManager.Instance.TargettingUIPlayerTurnStart();
        NextTurn();
        enemy_TargetLists.Clear();
        UIManager.Instance.TargettingUIEnable(true, false);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
    }

    private void NextTurn()
    {
        if (_gameEnded)
            return;

        PlayerTurnCount = GetLiveCount(GameManager.Instance.Players);
        _turn++;
        OnNextTurn?.Invoke(_turn);
        List<PlayerMainModule> livePlayers = GameManager.Instance.Players.FindAll(v => v.IsLived);
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
            if (_loseTurn && (_plusTurn == false))
            {
                PlayerTurnCount = 0;
                OnLoseTurn?.Invoke(player);
                StartCoroutine(WaitCoroutine(false, player));
                return;
            }

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
        TurnCheckReset();

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

    public void TurnActionDelete(TurnAction turnAction)
    {
        _turnActions.Remove(turnAction);
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

    public void Start(int maxCount = -1)
    {
        if (maxCount == -1)
            maxCount = _maxCount;

        _startAction?.Invoke();
        _count = maxCount;
        _locked = false;
    }
}