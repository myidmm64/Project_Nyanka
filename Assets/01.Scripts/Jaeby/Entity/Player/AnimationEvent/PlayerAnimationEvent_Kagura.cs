using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MapTileGridCreator.Core;
using static Define;

public class PlayerAnimationEvent_Kagura : PlayerAnimationEvent
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
    private float _healMagni = 1f;


    public override void AttackAnimation(int id)
    {
        List<Cell> cells = CellUtility.SearchCells(
            _mainModule.CellIndex, CellUtility.GetAttackVectorByDirections(_mainModule.AttackModule.CurrentDirection, _mainModule.AttackRange), true);
        if (cells.Count == 0) return;

        GameObject obj = null;
        switch (id)
        {
            case 0:
                _cameraManager.CartUpdate(60f, null, null);
                obj = Instantiate(_attackPrefab0, _mainModule.ModelController);
                break;
            default:
                break;
        }
        obj.transform.SetParent(null);
        Arrow arrow = obj.AddComponent<Arrow>();
        arrow.ArrowInit(_arrowSpeed, transform.position + Vector3.up, Quaternion.LookRotation(CellUtility.GetAttackDirection(_mainModule.AttackModule.CurrentDirection)), _cellBoomVectors,
            0, 1f, Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage), _mainModule.elementType, Random.Range(0, 100) < 50, true);

        _cameraManager.CameraShake(8f, 10f, 0.24f);
    }

    public override void AttackEnd()
    {
        List<PlayerMainModule> players = GameManager.Instance.LivePlayers;
        for (int i = 0; i < players.Count; i++)
        {
            int dmg = (int)(UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage) * _healMagni);
            players[i]?.HPModule.Healing(dmg, _mainModule.elementType);
        }

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
