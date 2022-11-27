using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillModule : MonoBehaviour
{
    private BaseMainModule _mainModule = null;

    public bool Skillable => _turnChecked && SkillCellCheck;

    //��Ÿ�� üũ
    private bool _turnChecked = false;
    public bool TurnChecked => _turnChecked;

    //�ٷ� ��ų ��� ������ ������
    [SerializeField]
    private bool _skillableAtStart = false;

    //��ų�� ��Ÿ��
    [SerializeField]
    private int _skillCooltime = 0;
    public int SkillCooltime => _skillCooltime;

    //���� ���� ī��Ʈ
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

    // üũ ����
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
        Debug.Log("�����");
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
