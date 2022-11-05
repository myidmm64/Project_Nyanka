using MapTileGridCreator.Core;
using MapTileGridCreator.CubeImplementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoSingleTon<GameManager>
{
    private float _timeScale = 1f;
    public float TimeScale { get => _timeScale; set { _timeScale = value; Time.timeScale = _timeScale; } }

    [SerializeField]
    private List<Entity> _entitys = new List<Entity>();
    [SerializeField]
    private List<Player> _players = new List<Player>();

    [SerializeField]
    private int _turn = 1;

    public UnityEvent<int> OnStarted = null;
    public UnityEvent<int> OnNextTurn = null;

    private void Start()
    {
        OnStarted?.Invoke(1);
    }

    public void NextTurn()
    {
        _turn++;
        OnNextTurn?.Invoke(_turn);
    }
}
