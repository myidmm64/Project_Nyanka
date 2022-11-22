using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerAnimationEvent_Wakamo : PlayerAnimationEvent
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private GameObject _attackPrefab1 = null;
    [SerializeField]
    private GameObject _attackPrefab2 = null;

    public override void AttackAnimation(int id)
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;

        List<Cell> cells = CellUtility.SearchCells(_mainModule.CellIndex, _mainModule.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, module.AttackRange), true);
        if (cells.Count == 0) return;

        for (int i = 0; i < cells.Count; i++)
            cells[i].CellAttack(module.MinDamage, _mainModule.DataSO.elementType, _mainModule.entityType);

        List<AIMainModule> enemys = new List<AIMainModule>();
        for (int i = 0; i < cells.Count; i++)
            if (cells[i].GetObj?.GetComponent<AIMainModule>() != null)
                enemys.Add(cells[i].GetObj?.GetComponent<AIMainModule>());

        GameObject obj = null;
        switch (id)
        {
            case 0:
                obj = Instantiate(_attackPrefab0, _mainModule.ModelController);
                break;
            case 1:
                obj = Instantiate(_attackPrefab1, _mainModule.ModelController);
                break;
            case 2:
                obj = Instantiate(_attackPrefab2, _mainModule.ModelController);
                break;
            default:
                break;
        }
        Destroy(obj, 1.5f);
        CameraManager.Instance.CameraShake(8f, 10f, 0.24f);

        for (int i = 0; i < enemys.Count; i++)
        {
            int dmg = UnityEngine.Random.Range(module.MinDamage, module.MaxDamage);
            bool critical = UnityEngine.Random.Range(0, 100) < 50;
            if (critical)
                dmg = Mathf.RoundToInt(dmg * 1.5f);
            enemys[i].ApplyDamage(dmg, _mainModule.DataSO.elementType, critical, true);
        }
    }

    public override void AttackEnd()
    {
        CameraManager.Instance.CameraSelect(VCamTwo);
        _mainModule.animator.Play("Idle");
        _mainModule.animator.Update(0);
        StartCoroutine(CameraSelectTurm(() =>
        TurnManager.Instance.PressTurnCheck(_mainModule)));
    }

    public override void AttackStarted()
    {
        //CameraManager.Instance.CartCamSelect(_path, _modelController, 0f);
    }
}
