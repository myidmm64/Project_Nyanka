using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class BaseMoveModule : MonoBehaviour
{
    private bool _moveable = true;
    public bool Moveable
    {
        get => _moveable;
        set => _moveable = value;
    }
    private BaseMainModule _mainModule = null;

    private void Start()
    {
        _mainModule = GetComponent<BaseMainModule>();
    }

    public abstract IEnumerator Move(Vector3Int v); // 이동 함수
}
