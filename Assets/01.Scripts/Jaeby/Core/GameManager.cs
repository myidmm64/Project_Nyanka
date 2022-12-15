
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Define;

public class GameManager : MonoSingleTon<GameManager>
{
    private float _timeScale = 1f;
    public float TimeScale { get => _timeScale; set { _timeScale = value; Time.timeScale = _timeScale; } }

    #region 엔티티 관리
    private List<BaseMainModule> _entitys;
    public List<BaseMainModule> Entitys
    {
        get => _entitys;
        set => _entitys = value;
    }

    private List<PlayerMainModule> _players;
    public List<PlayerMainModule> Players
    {
        get => _players;
        set => _players = value;
    }

    private List<AIMainModule> _enemys;
    public List<AIMainModule> Enemys
    {
        get => _enemys;
        set => _enemys = value;
    }

    public List<PlayerMainModule> LivePlayers => _players.FindAll(x => x.IsLived);
    public List<AIMainModule> LiveEnemys => _enemys.FindAll(x => x.IsLived);
    #endregion

    [SerializeField]
    private int _maxAttackPoint = 0;
    public int MaxAttackPoint => _maxAttackPoint;

    private int _stage = 0;
    [SerializeField]
    private List<Transform> _mapParents = new List<Transform>(); // 맵 부모들
    [SerializeField]
    private TextMeshProUGUI _mapNameText = null;

    [SerializeField]
    private GameObject _nextStageLoadingObject = null;

    [field: SerializeField]
    private UnityEvent OnNextStage = null;

    [SerializeField]
    private List<StageSettingOption> _stageSettingOptions = new List<StageSettingOption>();

    private void Awake()
    {
        EntitysReset();
    }

    private void Start()
    {
        TurnManager.Instance.MaxPoint = MaxAttackPoint;
    }

    public void NextStage()
    {
        _stage++;
        StageSettingOption setting = _stageSettingOptions[_stage];

        _nextStageLoadingObject.SetActive(true);

        CameraManager.Instance.CameraReset();
        CameraManager.Instance.CameraSelect(VCamTwo);

        UIManager.Instance.TargettingUIReset();

        Destroy(CubeGrid.gameObject);
        Instantiate(setting.gridPrefab, null);
        
        for (int i = 0; i < Entitys.Count; i++)
            Destroy(Entitys[i]);
        Entitys.Clear();
        Players.Clear();
        Enemys.Clear();
        for (int i = 0; i < setting.entitySpawnDatas.Length; i++)
            Instantiate(setting.entitySpawnDatas[i].prefab, 
                setting.entitySpawnDatas[i].position, 
                Quaternion.Euler(setting.entitySpawnDatas[i].rotation));
        EntitysReset();
        TurnManager.Instance.MaxPoint = setting.maxAttackPoint;
        for (int i = 0; i < _mapParents.Count; i++)
            _mapParents[i].transform.position = setting.mapParentPositions[i];
        _mapNameText.SetText(setting.stageName);

        OnNextStage?.Invoke(); // 
    }

    private void EntitysReset()
    {
        Entitys = new List<BaseMainModule>(FindObjectsOfType<BaseMainModule>());
        Players = new List<PlayerMainModule>(FindObjectsOfType<PlayerMainModule>());
        Enemys = new List<AIMainModule>(FindObjectsOfType<AIMainModule>());
    }
}

[System.Serializable]
public struct StageSettingOption
{
    public string stageName;
    public int maxAttackPoint;
    public GameObject gridPrefab;
    public Vector3[] mapParentPositions;
    public EntitySpawnData[] entitySpawnDatas;
}

[System.Serializable]
public struct EntitySpawnData
{
    public GameObject prefab;
    public Vector3 position;
    public Vector3 rotation;
}
