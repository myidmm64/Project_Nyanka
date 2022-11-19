using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static Define;
using System;

public class Player_Wakamo : Player
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private GameObject _attackPrefab1 = null;
    [SerializeField]
    private GameObject _attackPrefab2 = null;

    public override void AttackStarted()
    {
        base.AttackStarted();
        //CameraManager.Instance.CartCamSelect(_path, _modelController, 0f);
    }

    public override void AttackEnd()
    {
        CameraManager.Instance.CameraSelect(VCamTwo);
        _animator.Play("Idle");
        _animator.Update(0);
        StartCoroutine(CameraSelectTurm(base.AttackEnd));
    }

    private IEnumerator CameraSelectTurm(Action a)
    {
        yield return new WaitForSeconds(0f);
        a?.Invoke();
    }

    public override void AttackAnimation(int id)
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, GetAttackVectorByDirections(_currentDirection, _dataSO.normalAttackRange), true);
        if (cells.Count == 0) return;

        for (int i = 0; i < cells.Count; i++)
            cells[i].CellAttack(_dataSO.normalAtk, _dataSO.elementType, _entityType);

        List<Enemy> enemys = new List<Enemy>();
        for (int i = 0; i < cells.Count; i++)
            if (cells[i].GetObj?.GetComponent<Enemy>() != null)
                enemys.Add(cells[i].GetObj?.GetComponent<Enemy>());

        GameObject obj = null;
        switch(id)
        {
            case 0:
                obj = Instantiate(_attackPrefab0, _modelController);
                break;
            case 1:
                obj = Instantiate(_attackPrefab1, _modelController);
                break;
            case 2:
                obj = Instantiate(_attackPrefab2, _modelController);
                break;
            default:
                break;
        }
        Destroy(obj, 1.5f);
        CameraManager.Instance.CameraShake(8f, 10f, 0.24f);

        for(int i = 0; i < enemys.Count; i++)
        {
            int dmg = UnityEngine.Random.Range(_dataSO.normalMinAtk, _dataSO.normalMaxAtk);
            enemys[i].ApplyDamage(dmg, _dataSO.elementType, UnityEngine.Random.Range(0, 100) < 50 ? true : false);
        }
    }
}
