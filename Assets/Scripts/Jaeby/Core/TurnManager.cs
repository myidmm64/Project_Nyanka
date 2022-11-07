using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoSingleTon<TurnManager>
{
    private List<Entity> _entitys;
    private List<Player> _players;
    public List<Player> Players => _players;
    private List<Enemy> _enemys;
    public List<Enemy> Enemys => _enemys;

    [SerializeField]
    private int _turn = 1;
    private bool _isTransed = false;

    public UnityEvent<int> OnStarted = null;
    public UnityEvent OnNextPhase = null;
    public UnityEvent<int> OnNextTurn = null;

    private int _playerTurnCount = 0;

    private void Awake()
    {
        _entitys = new List<Entity>(FindObjectsOfType<Entity>());
        _players = new List<Player>(FindObjectsOfType<Player>());
        _enemys = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    private void Start()
    {
        _playerTurnCount = GetLiveCount(_players);
        OnStarted?.Invoke(1);
    }

    public void UseTurn(int count)
    {
        _playerTurnCount -= count;
        if (_playerTurnCount <= 0)
            EnemyPhase();
    }

    public void EnemyPhase()
    {
        OnNextPhase?.Invoke();
        StartCoroutine(EnemysTurn());
    }

    private IEnumerator EnemysTurn()
    {
        List<Enemy> liveEnemys = _enemys.FindAll(v => v.IsLived);
        for (int i = 0; i < liveEnemys.Count; i++)
        {
            yield return StartCoroutine(liveEnemys[i].EnemyAction());
        }
        NextTurn();
    }

    private void NextTurn()
    {
        Debug.Log("´ÙÀ½ ÅÏ");
        _playerTurnCount = GetLiveCount(_players);
        _turn++;
        OnNextTurn?.Invoke(_turn);
        List<Player> livePlayers = _players.FindAll(v => v.IsLived);
        for (int i = 0; i < livePlayers.Count; i++)
            livePlayers[i].PhaseReset();
        OnNextPhase?.Invoke();
    }



    private int GetLiveCount(List<Player> entitys)
    {
        return entitys.FindAll(x => x.IsLived).Count;
    }
    private int GetLiveCount(List<Enemy> entitys)
    {
        return entitys.FindAll(x => x.IsLived).Count;
    }
}
