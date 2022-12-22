using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    int _dmg = 0; // 화살 데미지
    ElementType _type = ElementType.NONE; // 화살 타입
    bool _critical = false; // 크리티컬
    bool _isPlayer = false; // 플레이어 여부
    int _penetrateCount = 0; // 관통 횟수
    List<Vector3Int> _boomVectors = new List<Vector3Int>(); // 터지는 바닥 범위
    EntityType _entityType = EntityType.None; // 엔티티 타입
    GameObject _atkObj = null; // 타격 오브젝트
    float _speed = 0f; // 탄속

    /// <summary>
    /// 화살 초기화
    /// </summary>
    public void ArrowInit(float speed, Vector3 position, Quaternion rot, List<Vector3Int> boomVectors, int penetrateCount, float lifeTime, int dmg, ElementType type, bool critical, bool isPlayer,GameObject obj = null)
    {
        _speed = speed;
        transform.position = position;
        transform.rotation = rot;
        _penetrateCount = penetrateCount;
        _dmg = dmg;
        _type = type;
        _critical = critical;
        _isPlayer = isPlayer;
        _entityType = _isPlayer ? EntityType.Player : EntityType.Enemy;
        _boomVectors = boomVectors;
        _atkObj = obj;

        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// 타격
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        BaseMainModule mainModule = other.GetComponent<BaseMainModule>();
        if (mainModule == null) return;
        if (mainModule.entityType == _entityType) return;
        mainModule.ApplyDamage(_dmg, _type, _critical, _isPlayer);
        if(_atkObj != null)
        {
            GameObject o = Instantiate(_atkObj, mainModule.CellIndex, Quaternion.identity);
            Destroy(o, 2f);
        }
        List<Cell> cells = CellUtility.SearchCells(mainModule.CellIndex, _boomVectors, true);
        for (int i = 0; i < cells.Count; i++)
            cells[i].CellAttack(_dmg, _type, _entityType);

        _penetrateCount--;
        
        if (_penetrateCount <= 0)
            Destroy(gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
