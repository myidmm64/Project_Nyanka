using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoSingleTon<TutorialManager>
{
    [SerializeField]
    private GameObject _suzukaMoveButton = null;
    [SerializeField]
    private GameObject _wakamoLeftMoveButton = null; 
    [SerializeField]
    private GameObject _wakamoDownMoveButton = null;
    [SerializeField]
    private TutorialObject _tutorialObjPrefab = null;
    [SerializeField]
    private PlayerMainModule _wakamo = null;
    [SerializeField]
    private PlayerMainModule _suzuka = null;
    [SerializeField]
    private List<TutorialData> _tutoDatas = new List<TutorialData>();
    private int _index = 0;
    private int _count = 0;

    public void CountUp()
    {
        _count++;
        if (_tutoDatas[_index].nextCount <= _count)
        {
            DialogSystem.Instance.NextDialog();
            _count = 0;
            _index++;
        }
    }

    public void WakamoSelectObject()
    {
        TutorialObject o = Instantiate(_tutorialObjPrefab);
        o.Init(_wakamo.CellIndex, SelectWakamo);
    }

    public void WakamoForwardSelectObject()
    {
        TutorialObject o = Instantiate(_tutorialObjPrefab);
        o.Init(_wakamo.CellIndex + Vector3Int.forward * 2, 
            () => _wakamo.PreparationCellSelect(_wakamo.CellIndex + Vector3Int.forward * 2, false)
            );
    }

    public void WakamoLeftSelectObj()
    {
        TutorialObject o = Instantiate(_tutorialObjPrefab);
        o.Init(_wakamo.CellIndex + Vector3Int.left ,
            () =>
            {
                _wakamo.PreparationCellSelect(_wakamo.CellIndex + Vector3Int.left, false);
                _wakamoLeftMoveButton.SetActive(true);
            }
            );
    }


    public void WakamoDownSelectObj()
    {
        TutorialObject o = Instantiate(_tutorialObjPrefab);
        o.Init(_wakamo.CellIndex + Vector3Int.forward * -1,
            () =>
            {
                _wakamo.PreparationCellSelect(_wakamo.CellIndex + Vector3Int.forward * -1, false);
                _wakamoDownMoveButton.SetActive(true);
            }
            );
    }

    public void SuzukaSelectObject()
    {
        UIManager.Instance.TargettingUIEnable(false, true);
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, false);
        TutorialObject o = Instantiate(_tutorialObjPrefab);
        o.Init(_suzuka.CellIndex, SelectSuzuka);
    }


    public void SuzukaMoveSelectObject()
    {
        TutorialObject o = Instantiate(_tutorialObjPrefab);
        o.Init(_suzuka.CellIndex + Vector3Int.forward * 2 + Vector3Int.right * 2,
            () =>
            {
                _suzuka.PreparationCellSelect(_suzuka.CellIndex + Vector3Int.forward * 2 + Vector3Int.right * 2, false);
                _suzukaMoveButton.SetActive(true);
            }
            );
    }

    public void WakamoMove()
    {
        _wakamo.PlayerMove(_wakamo.CellIndex + Vector3Int.forward * 2);
    }

    public void WakamoMoveLeftOne()
    {
        _wakamo.PlayerMove(_wakamo.CellIndex + Vector3Int.left);
    }

    public void WakamoMoveDownTwo()
    {
        _wakamo.PlayerMove(_wakamo.CellIndex + Vector3Int.forward * -1);
    }

    public void SuzukaMove()
    {
        _suzuka.PlayerMove(_suzuka.CellIndex + Vector3Int.forward * 2 + Vector3Int.right * 2);
    }

    public void WakamoIdle()
    {
        _wakamo.PlayerIdle();
    }

    public void SuzukaIdle()
    {
        _suzuka.PlayerIdle();
    }

    public void SelectWakamo()
    {
        _wakamo.Selected();
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, false);
        UIManager.Instance.TargettingUIEnable(false, true);
    }

    public void SelectSuzuka()
    {
        _suzuka.Selected();
        SuzukaMoveSelectObject();
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, false);
        UIManager.Instance.TargettingUIEnable(false, true);
    }

    private void Start()
    {
        UIManager.Instance.TargettingUIEnable(false, true);
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, false);
    }

    public void ClickModeFalse()
    {
        UIManager.Instance.TargettingUIEnable(false, true);
        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, false);
    }

    public void TurnEndAndWakamoSelect()
    {
        TurnAction turnAction = new TurnAction(1, null, WakamoSelectObject, null);
        TurnManager.Instance.TurnActionAdd(turnAction, false);
    }

    public void WakamoSkill()
    {
        _wakamo.TrySkill();
    }

    public void WakamoTransform()
    {
        _wakamo.Transformation();
    }

    public void GoStart()
    {
        SceneManager.LoadScene("Start");
    }
}

[System.Serializable]
public struct TutorialData
{
    public string tutoName;
    public int nextCount;
}
