using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //스킬 UI
    [SerializeField]
    private Slider _skillCoolSlider = null;

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
        _skillCoolSlider.SetDirection(Slider.Direction.RightToLeft, false);
        _skillCoolSlider.minValue = 0;
        _skillCoolSlider.maxValue = _skillCooltime;
        _skillCoolSlider.value = Count;
    }

    private void SkillEnable()
    {
        _turnChecked = true;
    }

    public void RestartSkillCoolTime(bool turnCheck)
    {
        TryMakeTurnAction();
        _turnChecked = turnCheck;
        ChangeSkillCount(_skillCooltime);
        _skillTurnAction.Start();
    }

    private void TryMakeTurnAction()
    {
        if (_skillTurnAction != null) return;
        _skillTurnAction = new TurnAction(_skillCooltime, null, SkillEnable, ChangeSkillCount);
        TurnManager.Instance.TurnActionAdd(_skillTurnAction, false);
    }

    private void ChangeSkillCount(int val)
    {
        StartCoroutine(SkillCountDownCoroutine(val));
    }

    private IEnumerator SkillCountDownCoroutine(int end) // 슬라이더 소모 애니메이션
    {
        float delta = 0f;
        while (delta <= 1f)
        {
            delta += Time.deltaTime * 2f;
            _skillCoolSlider.value = end * delta;
            yield return null;
        }
        _skillCoolSlider.value = end;
    }
}
