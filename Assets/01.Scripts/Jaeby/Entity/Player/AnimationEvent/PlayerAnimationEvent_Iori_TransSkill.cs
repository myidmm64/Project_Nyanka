using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using DG.Tweening;
public class PlayerAnimationEvent_Iori_TransSkill : PlayerAnimationEvent
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
    private float _damageMagni = 1f;


    public override void AttackAnimation(int id)
    {
        List<Cell> cells = CellUtility.SearchCells(
            _mainModule.CellIndex, CellUtility.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, _mainModule.AttackRange), true);
        if (cells.Count == 0) return;

        List<AIMainModule> enemys = new List<AIMainModule>();
        for (int i = 0; i < cells.Count; i++)
            if (cells[i].GetObj?.GetComponent<AIMainModule>() != null)
                enemys.Add(cells[i].GetObj?.GetComponent<AIMainModule>());

        GameObject obj = null;
        switch (id)
        {
            case 0:
                _cameraManager.CartUpdate(60f, null, null);
                PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f,
                    ImageManager.Instance.GetImageData(_mainModule.elementType).color, "천둥아 치거라", 0.8f, "고요 속의 뇌우", 0.3f);
                obj = Instantiate(_attackPrefab0, _mainModule.ModelController);
                break;
            case 1:
                _cameraManager.CartUpdate(75f, null, null);
                obj = Instantiate(_attackPrefab1, _mainModule.ModelController);
                for (int i = 0; i < cells.Count; i++)
                    cells[i].CellAttack(_mainModule.MinDamage, _mainModule.elementType, _mainModule.entityType);
                StartCoroutine(AttackCoroutine(enemys));
                break;
            default:
                break;
        }
        Destroy(obj, 1.5f);
    }

    private IEnumerator AttackCoroutine(List<AIMainModule> modules)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < modules.Count; j++)
            {
                if (modules[j] != null)
                {
                    int dmg = UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage);
                    bool critical = UnityEngine.Random.Range(0, 100) < 50;
                    modules[j].ApplyDamage(Mathf.RoundToInt(dmg * _damageMagni), _mainModule.elementType, critical, true);
                }
            }
            yield return new WaitForSeconds(0.2f);
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
