using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MapTileGridCreator.Core;
using static Define;

public class PlayerAnimationEvent_Kagura_Skill : PlayerAnimationEvent
{
    [SerializeField]
    private CinemachineSmoothPath _path;
    [SerializeField]
    private List<Transform> _lookPoints = new List<Transform>();

    [SerializeField]
    private GameObject _healPrefab = null;
    [SerializeField]
    private GameObject _dealPrefab = null;

    [SerializeField]
    private float[] _damageMagni = null;

    public override void AttackAnimation(int id)
    {
        switch (id)
        {
            case 0:
                PopupUtility.DialogText(transform.position + Vector3.right * 2f + Vector3.up * 1.5f, Color.white, "³ú¼Ó¿ìÁ¶", 2.5f, "¹ø°³ÀÇ ÈûÀ¸·Î!!");
                _cameraManager.CartUpdate(60f, null, null);
                break;
            case 1:
                _cameraManager.CartUpdate(60f, null, 0f);
                List<AIMainModule> ais = GameManager.Instance.LiveEnemys;
                for (int i = 0; i < ais.Count; i++)
                {
                    int dmg = (int)(UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage) * _damageMagni[0]);
                    bool critical = UnityEngine.Random.Range(0, 100) < 50;
                    GameObject obj = null;
                    obj = Instantiate(_dealPrefab, ais[i].CellIndex, Quaternion.identity);
                    ais[i]?.ApplyDamage(dmg, _mainModule.elementType, critical, true);
                    Destroy(obj, 1.5f);
                }
                List<PlayerMainModule> players = GameManager.Instance.LivePlayers;
                for (int i = 0; i < players.Count; i++)
                {
                    int dmg = (int)(UnityEngine.Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage) * _damageMagni[0]);
                    GameObject obj = null;
                    obj = Instantiate(_healPrefab, players[i].CellIndex, Quaternion.identity);
                    players[i]?.HPModule.Healing(dmg, _mainModule.elementType);
                    Destroy(obj, 1.5f);
                }
                break;
            default:
                break;
        }
        _cameraManager.CameraShake(12f, 10f, 0.24f);

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
