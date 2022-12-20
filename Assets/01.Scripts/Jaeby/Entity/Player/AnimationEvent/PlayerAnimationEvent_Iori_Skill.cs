using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using DG.Tweening;

public class PlayerAnimationEvent_Iori_Skill : PlayerAnimationEvent
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();

    [SerializeField]
    private GameObject _modelPrefab = null;
    [SerializeField]
    private GameObject _attackPrefab0 = null;

    [SerializeField]
    private List<float> _damageMagni = new List<float>();


    public override void AttackAnimation(int id)
    {
        List<Cell> cells = CellUtility.SearchCells(
            _mainModule.CellIndex, CellUtility.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, _mainModule.SkillRange), true);
        if (cells.Count == 0) return;

        List<AIMainModule> enemys = new List<AIMainModule>();
        for (int i = 0; i < cells.Count; i++)
            if (cells[i].GetObj?.GetComponent<AIMainModule>() != null)
                enemys.Add(cells[i].GetObj?.GetComponent<AIMainModule>());

        switch (id)
        {
            case 0:
                _cameraManager.CartUpdate(60f, null, null);
                PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f, 
                    ImageManager.Instance.GetImageData(_mainModule.elementType).color, "지금이, 종말의 시간", 0.8f, "신속", 0.5f);
                Sequence seq = DOTween.Sequence();
                List<Vector3Int> positions = new List<Vector3Int>();
                positions.Add(new Vector3Int(2, 0, 2));
                positions.Add(new Vector3Int(0, 0, 4));
                positions.Add(new Vector3Int(-2, 0, 2));
                positions = CellUtility.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, positions);
                for (int i = 0; i < 3; i++)
                {
                    GameObject obj = null;
                    obj = Instantiate(_modelPrefab, _mainModule.CellIndex + positions[i], Quaternion.Euler(0f, _mainModule.ModelController.eulerAngles.y - (90f * (i + 1)),0f));
                    seq.Append(obj.transform.DOMove(obj.transform.position + obj.transform.forward * 2f, 0.8f));
                    seq.AppendCallback(() =>
                    {
                        _cameraManager.CameraShake(8f, 10f, 0.24f);
                        for (int i = 0; i < enemys.Count; i++)
                        {
                            int dmg = UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage);
                            bool critical = UnityEngine.Random.Range(0, 100) < 50;
                            enemys[i].ApplyDamage(Mathf.RoundToInt(dmg * _damageMagni[0]), _mainModule.elementType, critical, true);
                        }
                        for (int i = 0; i < cells.Count; i++)
                            cells[i].CellAttack(_mainModule.MinDamage, _mainModule.elementType, _mainModule.entityType);
                        Destroy(obj);
                    });
                }
                break;
            case 1:
                _cameraManager.CartUpdate(70f, null, 0f);
                _cameraManager.CameraShake(8f, 10f, 0.24f);
                GameObject obj2 = null;
                obj2 = Instantiate(_attackPrefab0, _mainModule.ModelController);
                Destroy(obj2, 1.5f);
                for (int i = 0; i < enemys.Count; i++)
                {
                    int dmg = UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage);
                    bool critical = UnityEngine.Random.Range(0, 100) < 50;
                    enemys[i].ApplyDamage(Mathf.RoundToInt(dmg * _damageMagni[1]), _mainModule.elementType, critical, true);
                }
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
