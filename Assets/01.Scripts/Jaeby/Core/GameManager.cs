using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Define;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleTon<GameManager>
{
    [SerializeField]
    private bool _isTutorial = false;
    public bool IsTutorial => _isTutorial;

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
    private StageSettingOption _currentStageOption;
    public DialogEvent ClearDialog => _currentStageOption.clearDialog;
    public DialogEvent FailDialog => _currentStageOption.failDialog;

    private bool _first = true;

    private void Awake()
    {
        if (_isTutorial)
        {
            EntitysReset();
            TurnManager.Instance.MaxPoint = MaxAttackPoint;
            CubeGrid = GameObject.FindObjectOfType<Grid3D>();
        }
        else
        {
            if (PlayerPrefs.GetInt("CONTINUE", 0) == 1)
                _stage = PlayerPrefs.GetInt("STAGE", 0);
            else
                _stage = 0;
            StageChange();
        }
    }

    public void NextStage()
    {
        _stage++;
        _stage = Mathf.Clamp(_stage, 0, _stageSettingOptions.Count -1);
        StageChange();
    }

    public void PrevStage()
    {
        _stage--;
        _stage = Mathf.Clamp(_stage, 0, _stageSettingOptions.Count -1);
        StageChange();
    }

    public void RestartStage()
    {
        StageChange();
    }

    public void GoStartScene()
    {
        SceneManager.LoadScene("Start");
    }

    private void StageChange()
    {
        _currentStageOption = _stageSettingOptions[_stage];

        _nextStageLoadingObject.SetActive(true);

        CameraManager.Instance.CameraReset();
        CameraManager.Instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, true);
        UIManager.Instance.TargettingUIReset();


        for (int i = 0; i < _entitys.Count; i++)
            if (_entitys[i] != null)
                if (_entitys[i].isActiveAndEnabled)
                    Destroy(_entitys[i].gameObject);

        if(CubeGrid != null)
        {
            Destroy(CubeGrid.transform.root.gameObject);
            CubeGrid = null;
        }
        GameObject stageObj = Instantiate(_currentStageOption.stagePrefab, null);
        CubeGrid = stageObj.GetComponentInChildren<Grid3D>();

        EntitysReset();
        _maxAttackPoint = _currentStageOption.maxAttackPoint;
        TurnManager.Instance.MaxPoint = _currentStageOption.maxAttackPoint;
        _mapParents.transform.position = _currentStageOption.mapParentPositions;
        _mapParents.transform.rotation = Quaternion.Euler(_currentStageOption.mapParentRotations);
        _mapNameText.SetText(_currentStageOption.stageName);

        if(_first == false)
            OnNextStage?.Invoke();
        _first = false;

        _nextStageLoadingObject.SetActive(false);

        DialogSystem.Instance.TryStartDialog(_currentStageOption.startDialog);
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

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("STAGE", _stage);
        Debug.Log($"{PlayerPrefs.GetInt("STAGE", 0)}");
    }
}

[System.Serializable]
public struct StageSettingOption
{
    public string stageName;
    public int maxAttackPoint;
    public GameObject stagePrefab;
    public Vector3 mapParentPositions;
    public Vector3 mapParentRotations;
    public DialogEvent startDialog;
    public DialogEvent clearDialog;
    public DialogEvent failDialog;
}
