using Cinemachine;
using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EnemyAnimationEvent_Warrior : EnemyAnimationEvent
{
    /*[SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();

    [SerializeField]
    private GameObject _attackPrefab0 = null;
    [SerializeField]
    private GameObject _attackPrefab1 = null;
    [SerializeField]
    private GameObject _attackPrefab2 = null;


    public override void AttackAnimation(int id)
    {

        AttackDirection dir = AttackDirection.Up;
        for (int i = 0; i < (int)AttackDirection.Down + 1; i++)
        {
            bool isChk = false;
            List<Vector3Int> vecs = _mainModule.GetAttackVectorByDirections((AttackDirection)i, _mainModule.DataSO.normalAttackRange);
            for (int j = 0; j < vecs.Count; j++)
            {
                List<PlayerMainModule> m = CellUtility.FindTarget<PlayerMainModule>(_mainModule.ChangeableCellIndex, vecs, true);
                if (m.Count > 0)
                {
                    dir = (AttackDirection)i;
                    isChk = true;
                    break;
                }
            }
            if (isChk)
                break;
        }

        GameObject obj = null;
        switch (id)
        {
            case 0:
                _cameraManager.CartUpdate(60f, null, null);
                obj = Instantiate(_attackPrefab0, _mainModule.ModelController);
                break;
            case 1:
                _cameraManager.CartUpdate(60f, null, 0f);
                obj = Instantiate(_attackPrefab1, _mainModule.ModelController);
                break;
            case 2:
                _cameraManager.CartUpdate(70f, null, 0f);
                obj = Instantiate(_attackPrefab2, _mainModule.ModelController);
                break;
            default:
                break;
        }
        Destroy(obj, 1.5f);
        _cameraManager.CameraShake(8f, 10f, 0.24f);

        for (int i = 0; i < enemys.Count; i++)
        {
            int dmg = UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage);
            bool critical = UnityEngine.Random.Range(0, 100) < 50;
            if (critical)
                dmg = Mathf.RoundToInt(dmg * 1.5f);
            enemys[i].ApplyDamage(dmg, _mainModule.DataSO.elementType, critical, true);
        }
    }

    public override void AttackEnd()
    {
        _cameraManager.CameraSelect(VCamTwo);
        _mainModule.animator.Play("Idle");
        _mainModule.animator.Update(0);
    }

    public override void AttackStarted()
    {
        _cameraManager.CartCamSelect(_path, _lookPoints[0], 0f);
    }
    public override void AttackAnimation(int id)
    {
        
    }

    public override void AttackEnd()
    {
        
    }

    public override void AttackStarted()
    {
        
    }*/
    public override void AttackAnimation(int id)
    {
        throw new System.NotImplementedException();
    }

    public override void AttackEnd()
    {
        throw new System.NotImplementedException();
    }

    public override void AttackStarted()
    {
        throw new System.NotImplementedException();
    }
}
