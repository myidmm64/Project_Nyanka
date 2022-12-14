using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerAnimationEvent_Suzuka_Skill : PlayerAnimationEvent
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();

    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private GameObject _attackPrefab1 = null;

    [SerializeField]
    private float[] _damageMagni = null;


    public override void AttackAnimation(int id)
    {
        List<Cell> cells = CellUtility.SearchCells(
            _mainModule.CellIndex, CellUtility.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, _mainModule.SkillRange), true);
        if (cells.Count == 0) return;

        for (int i = 0; i < cells.Count; i++)
            cells[i].CellAttack(_mainModule.MinDamage, _mainModule.elementType, _mainModule.entityType);

        List<AIMainModule> enemys = new List<AIMainModule>();
        for (int i = 0; i < cells.Count; i++)
            if (cells[i].GetObj?.GetComponent<AIMainModule>() != null)
                enemys.Add(cells[i].GetObj?.GetComponent<AIMainModule>());

        GameObject obj = null;
        switch (id)
        {
            case 0:
                PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f, ImageManager.Instance.GetImageData(ElementType.Water).color, "?ѱ? ?ӿ? ???? ??????", 0.8f, "?ѷ???", 0.5f);
                _cameraManager.CartUpdate(60f, null, null);
                obj = Instantiate(_attackPrefab0, _mainModule.ModelController);
                break;
            case 1:
                _cameraManager.CartUpdate(60f, null, 0f);
                obj = Instantiate(_attackPrefab1, _mainModule.ModelController);
                break;
            default:
                break;
        }
        Destroy(obj, 1.5f);
        _cameraManager.CameraShake(8f, 10f, 0.24f);

        StartCoroutine(EnemyDamageCoroutine(enemys));
    }

    private IEnumerator EnemyDamageCoroutine(List<AIMainModule> enemys)
    {
        for(int j = 0; j < 10; j++)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                int dmg = (int)(UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage) * _damageMagni[0]);
                bool critical = UnityEngine.Random.Range(0, 100) < 50;
                if (enemys[i] == null)
                    continue;
                Vector3Int index = enemys[i].CellIndex;
                Cell c = CubeGrid.TryGetCellByIndex(ref index);
                c?.CellAttack(dmg, _mainModule.elementType, _mainModule.entityType);
                enemys[i].ApplyDamage(dmg, _mainModule.elementType, critical, true);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    public override void AttackEnd()
    {
        _cameraManager.CameraSelect(VCamTwo);
        _mainModule.animator.Play("Idle");
        _mainModule.animator.Update(0);
        StartCoroutine(CameraSelectTurm(() =>
        TurnManager.Instance.PressTurnCheck(_mainModule)));
    }

    public override void AttackStarted()
    {
        _cameraManager.CartCamSelect(_path, _lookPoints[0], 0f);
    }
}
