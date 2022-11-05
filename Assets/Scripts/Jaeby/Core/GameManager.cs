using MapTileGridCreator.Core;
using MapTileGridCreator.CubeImplementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class GameManager : MonoSingleTon<GameManager>
{
    private float _timeScale = 1f;
    public float TimeScale { get => _timeScale; set { _timeScale = value; Time.timeScale = _timeScale; } }

    private List<Entity> _entitys;
    private List<Player> _players;
    public List<Player> Players => _players;
    private List<Enemy> _enemys;
    public List<Enemy> Enemys => _enemys;

    [SerializeField]
    private int _turn = 1;

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
        OnStarted?.Invoke(1);
    }

    public void CostUp()
    {
        _playerTurnCount++;
        if (_playerTurnCount >= GetLiveCount(_players))
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
        _playerTurnCount = 0;
        _turn++;
        OnNextTurn?.Invoke(_turn);
        for (int i = 0; i < _players.Count; i++)
            _players[i].PhaseReset();
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
