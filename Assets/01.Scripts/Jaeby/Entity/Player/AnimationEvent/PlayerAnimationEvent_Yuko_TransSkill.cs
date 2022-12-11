using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerAnimationEvent_Yuko_TransSkill : PlayerAnimationEvent
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();

    [SerializeField]
    private GameObject _effect0 = null;
    [SerializeField]
    private GameObject _effect1 = null;

    public override void AttackAnimation(int id)
    {
        _cameraManager.CartUpdate(80f, null, null);
        List<Cell> cells = CellUtility.SearchCells(
            _mainModule.CellIndex, CellUtility.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, _mainModule.SkillRange), true);
        if (cells.Count == 0) return;

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<Block>().ChangeBlock(_mainModule.elementType);
            cells[i].CellAttack(_mainModule.MinDamage, _mainModule.elementType, _mainModule.entityType);
        }

        List<AIMainModule> enemys = new List<AIMainModule>();
        for (int i = 0; i < cells.Count; i++)
            if (cells[i].GetObj?.GetComponent<AIMainModule>() != null)
                enemys.Add(cells[i].GetObj?.GetComponent<AIMainModule>());

        GameObject obj = null;
        switch (id)
        {
            case 0:
                PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f,
                    ImageManager.Instance.GetImageData(_mainModule.elementType).color,
                    "¾èº¸Áö ¸»¶ó°í!!", 0.7f, "ÆÄÁ×Áö¼¼", 0.3f);
                _cameraManager.CartUpdate(60f, null, 0f);
                return;
            case 1:
                _cameraManager.CartUpdate(60f, null, null);
                obj = Instantiate(_effect0, _mainModule.ModelController);
                _cameraManager.CameraShake(8f, 10f, 0.24f);
                break;
            case 2:
                _cameraManager.CartUpdate(70f, null, 0f);
                obj = Instantiate(_effect1, _mainModule.ModelController);
                _cameraManager.CameraShake(12f, 15f, 0.24f);
                break;
            default:
                break;
        }
        Destroy(obj, 1.5f);

        for (int i = 0; i < enemys.Count; i++)
        {
            int dmg = UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage);
            bool critical = UnityEngine.Random.Range(0, 100) < 50;
            enemys[i].ApplyDamage(dmg, _mainModule.elementType, critical, true);
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
