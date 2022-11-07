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
}
