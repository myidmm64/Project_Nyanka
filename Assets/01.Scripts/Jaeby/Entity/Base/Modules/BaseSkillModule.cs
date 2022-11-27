using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillModule : MonoBehaviour
{
    private BaseMainModule _mainModule = null;
    private bool _skillable = false;
    public bool Skillable => _skillable;

    [SerializeField]
    private bool _skillableAtStart = false;
    [SerializeField]
    private int _skillCooltime = 0;
    public int SkillCooltime => _skillCooltime;

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
        _skillTurnAction = new TurnAction(_skillCooltime, null, null);
        if (_skillableAtStart)
            _skillTurnAction.Count = 0;

        TurnManager.Instance.TurnActionAdd(_skillTurnAction, false);
        _mainModule = GetComponent<BaseMainModule>();
    }

    public void SkillEnable()
    {
        _skillable = true;
    }

    public void TrySkill()
    {
        if ((SkillCellCheck && _skillable) == false) return;

    }

    public void RestartSkillCoolTime()
    {
        _skillTurnAction.Start();
    }
}
