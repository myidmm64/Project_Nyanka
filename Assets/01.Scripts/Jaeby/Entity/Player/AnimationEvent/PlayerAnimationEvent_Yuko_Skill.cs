using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerAnimationEvent_Yuko_Skill : PlayerAnimationEvent
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();
    [SerializeField]
    private List<Vector3Int> _boomVectors = new List<Vector3Int>();

    public override void AttackAnimation(int id)
    {
        switch (id)
        {
            case 0:
                PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f, 
                    ImageManager.Instance.GetImageData(_mainModule.elementType).color, 
                    "쉽게 끝나진 않을 걸??", 0.3f, "이번 턴 방어력 증가", 0.15f);
                _cameraManager.CartUpdate(60f, null, 0f);
                break;
            case 1:
                _cameraManager.CartUpdate(80f, null, null);
                List<Cell> cells = CellUtility.SearchCells(
                    _mainModule.CellIndex, _boomVectors, true);
                if (cells.Count == 0) return;
                for (int i = 0; i < cells.Count; i++)
                {
                    cells[i].GetComponent<Block>().ChangeBlock(_mainModule.elementType);
                    cells[i].CellAttack(_mainModule.MinDamage, _mainModule.elementType, _mainModule.entityType);
                }
                for (int i = 0; i < cells.Count; i++)
                    cells[i].GetComponent<Block>().ChangeBlock(_mainModule.elementType);
                List<AIMainModule> enemys = new List<AIMainModule>();
                for (int i = 0; i < cells.Count; i++)
                    if (cells[i].GetObj?.GetComponent<AIMainModule>() != null)
                        enemys.Add(cells[i].GetObj?.GetComponent<AIMainModule>());
                for (int i = 0; i < enemys.Count; i++)
                    enemys[i].ApplyDamage(1, _mainModule.elementType, false, true);
                break;
            default:
                break;
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
