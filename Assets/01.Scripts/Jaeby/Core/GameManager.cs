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

    private List<BaseMainModule> _entitys;
    public List<BaseMainModule> Entitys
    {
        get => _entitys;
        set => _entitys = value;
    }

    private List<PlayerMainModule> _players;
    public List<PlayerMainModule> Players
    {
        get => _players;
        set => _players = value;
    }

    private List<AIMainModule> _enemys;
    public List<AIMainModule> Enemys
    {
        get => _enemys;
        set => _enemys = value;
    }

    public List<PlayerMainModule> LivePlayers => _players.FindAll(x => x.IsLived);
    public List<AIMainModule> LiveEnemys => _enemys.FindAll(x => x.IsLived);


    private int _stage = 0;
    [SerializeField]
    private List<Transform> _mapParents = new List<Transform>(); // ¸Ê ºÎ¸ðµé


    [SerializeField]
    private GameObject _nextStageLoadingObject = null;


    [field: SerializeField]
    private UnityEvent OnStageEnded = null;
    

    public void StageEnd()
    {
        OnStageEnded?.Invoke(); // 
    }
}


