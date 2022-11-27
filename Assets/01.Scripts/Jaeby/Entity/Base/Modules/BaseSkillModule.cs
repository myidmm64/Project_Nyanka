using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillModule : MonoBehaviour
{
    private BaseMainModule _mainModule = null;

    public bool Skillable => _turnChecked && SkillCellCheck;

    //쿨타임 체크
    private bool _turnChecked = false;
    public bool TurnChecked => _turnChecked;

    //바로 스킬 사용 가능할 것인지
    [SerializeField]
    private bool _skillableAtStart = false;

    //스킬의 쿨타임
    [SerializeField]
    private int _skillCooltime = 0;
    public int SkillCooltime => _skillCooltime;

    //현재 남은 카운트
    public int Count
    {
        get
        {
            if (_skillTurnAction == null)
                return 0;
            else
                return _skillTurnAction.Count;
        }
    }

    private TurnAction _skillTurnAction = null;
    public int CurCooltime => _skillTurnAction.Count;

    // 체크 관련
    public virtual bool SkillCellCheck
    {
        get
        {
            bool check = false;
            for (int i = 0; i < 4; i++)
                if (CellUtility.FindTarget<AIMainModule>(_mainModule.CellIndex, _mainModule.GetAttackVectorByDirections((AttackDirection)i, _mainModule.SkillRange), true).Count > 0)
                    check = true;

            return check;
        }
    }

    private void Start()
    {
        _mainModule = GetComponent<BaseMainModule>();

        if (_skillableAtStart)
            _turnChecked = true;
        else
            TryMakeTurnAction();
    }

    private void SkillEnable()
    {
        Debug.Log("끼모띠");
        _turnChecked = true;
    }

    public void RestartSkillCoolTime(bool turnCheck)
    {
        TryMakeTurnAction();
        _turnChecked = turnCheck;
        _skillTurnAction.Start();
    }

    private void TryMakeTurnAction()
    {
        if (_skillTurnAction != null) return;
        _skillTurnAction = new TurnAction(_skillCooltime, null, SkillEnable);
        TurnManager.Instance.TurnActionAdd(_skillTurnAction, false);
    }
}
