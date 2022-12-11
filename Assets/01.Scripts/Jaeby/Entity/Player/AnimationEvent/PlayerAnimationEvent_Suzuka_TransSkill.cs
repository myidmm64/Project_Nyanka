using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerAnimationEvent_Suzuka_TransSkill : PlayerAnimationEvent
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();

    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private List<Vector3Int> _cellBoomVectors = new List<Vector3Int>();
    [SerializeField]
    private float _arrowSpeed = 0f;

    [SerializeField]
    private float[] _damageMagni = null;


    public override void AttackAnimation(int id)
    {
        List<Cell> cells = CellUtility.SearchCells(
            _mainModule.CellIndex, CellUtility.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, _mainModule.AttackRange), true);
        if (cells.Count == 0) return;

        switch (id)
        {
            case 0:
                PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f, 
                    ImageManager.Instance.GetImageData(ElementType.Water).color, "µµ¸ÁÄ¥ ¼ö ¾ø¾î", 0.5f, "ºù¸¶ÀÇ È­»ì", 0.3f);
                _cameraManager.CartUpdate(60f, null, null);
                _cameraManager.CameraShake(4f, 5f, 5f, true);
                break;
            case 1:
                _cameraManager.CartUpdate(70f, null, 0f);
                GameObject obj = null;
                obj = Instantiate(_attackPrefab0, _mainModule.ModelController);
                obj.transform.SetParent(null);
                Arrow arrow = obj.AddComponent<Arrow>();
                PlayerMainModule m = _mainModule as PlayerMainModule;
                Quaternion plusRot = Quaternion.AngleAxis
                    (((_mainModule.AttackModule.CurrentDirection == AttackDirection.Up) || 
                    (_mainModule.AttackModule.CurrentDirection == AttackDirection.Down)) 
                    ? 180f : 180f, Vector3.up);
                arrow.ArrowInit(-_arrowSpeed, _mainModule.CellIndex + Vector3.up * 1.5f + m.ModelController.forward * -2f, plusRot * Quaternion.LookRotation(CellUtility.GetAttackDirection(_mainModule.AttackModule.CurrentDirection)), _cellBoomVectors,
                    10, 2f, Mathf.RoundToInt(Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage) * _damageMagni[0]), _mainModule.elementType, Random.Range(0, 100) < 50, true);
                Destroy(obj, 1.5f);

                _cameraManager.CompletePrevFeedBack();
                _cameraManager.CameraShake(30f, 10f, 0.25f);
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
