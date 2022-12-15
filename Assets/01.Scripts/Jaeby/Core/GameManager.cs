
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
    [SerializeField]
    private List<BaseMainModule> _entitys;
    public List<BaseMainModule> Entitys
    {
        get
        {
            ListNullCheck(_entitys);
            return _entitys;
        }
        set => _entitys = value;
    }

    [SerializeField]
    private List<PlayerMainModule> _players;
    public List<PlayerMainModule> Players
    {
        get
        {
            ListNullCheck(_players);
            return _players;
        }
        set => _players = value;
    }

    [SerializeField]
    private List<AIMainModule> _enemys;
    public List<AIMainModule> Enemys
    {
        get
        {
            ListNullCheck(_enemys);
            return _enemys;
        }
        set => _enemys = value;
    }

    public List<BaseMainModule> LiveEntitys => _entitys.FindAll(x => x.IsLived);
    public List<PlayerMainModule> LivePlayers => _players.FindAll(x => x.IsLived);
    public List<AIMainModule> LiveEnemys => _enemys.FindAll(x => x.IsLived);
    #endregion

    [SerializeField]
    private int _maxAttackPoint = 0;
    public int MaxAttackPoint => _maxAttackPoint;

    private int _stage = 0;
    [SerializeField]
    private Transform _mapParents = null; // 맵 부모들
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
        //_stage++;
        StageSettingOption setting = _stageSettingOptions[_stage];

        _nextStageLoadingObject.SetActive(true);

        CameraManager.Instance.CameraReset();
        CameraManager.Instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, true);
        UIManager.Instance.TargettingUIReset();

        Destroy(CubeGrid.gameObject);
        Instantiate(setting.gridPrefab, null);

        for (int i = 0; i < _entitys.Count; i++)
            if (_entitys[i] != null)
                if (_entitys[i].isActiveAndEnabled)
                    Destroy(_entitys[i].gameObject);

        for (int i = 0; i < setting.entitySpawnDatas.Length; i++)
            Instantiate(setting.entitySpawnDatas[i].prefab,
                setting.entitySpawnDatas[i].position,
                Quaternion.Euler(setting.entitySpawnDatas[i].rotation));

        EntitysReset();
        _maxAttackPoint = setting.maxAttackPoint;
        TurnManager.Instance.MaxPoint = setting.maxAttackPoint;
        _mapParents.transform.position = setting.mapParentPositions;
        _mapParents.transform.rotation = Quaternion.Euler(setting.mapParentRotations);
        _mapNameText.SetText(setting.stageName);

        OnNextStage?.Invoke(); // 
    }

    private void EntitysReset()
    {
        Entitys.Clear();
        Players.Clear();
        Enemys.Clear();
        Entitys = new List<BaseMainModule>(FindObjectsOfType<BaseMainModule>());
        Players = new List<PlayerMainModule>(FindObjectsOfType<PlayerMainModule>());
        Enemys = new List<AIMainModule>(FindObjectsOfType<AIMainModule>());
    }

    private void ListNullCheck<T>(List<T> list) where T : MonoBehaviour
    {
        List<T> destroyEntitys = new List<T>();
        for (int i = 0; i < list.Count; i++)
            if (list[i] == null || list[i].isActiveAndEnabled == false || list[i].gameObject.activeSelf == false)
                destroyEntitys.Add(list[i]);
        for (int i = 0; i < destroyEntitys.Count; i++)
            list.Remove(destroyEntitys[i]);
    }
}

[System.Serializable]
public struct StageSettingOption
{
    public string stageName;
    public int maxAttackPoint;
    public GameObject gridPrefab;
    public Vector3 mapParentPositions;
    public Vector3 mapParentRotations;
    public EntitySpawnData[] entitySpawnDatas;
}

[System.Serializable]
public struct EntitySpawnData
{
    public GameObject prefab;
    public Vector3 position;
    public Vector3 rotation;
}
