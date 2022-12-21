using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using static Define;

public abstract class BaseHPModule : MonoBehaviour
{
    private BaseMainModule _mainModule = null;

    [field: SerializeField]
    private UnityEvent OnDie = null;
    [SerializeField]
    private GameObject _dieEffect = null;

    // 필요한 데이터들
    [SerializeField]
    private Slider _hpSlider = null;
    private Coroutine _hpCoroutine = null;
    [SerializeField]
    private TextMeshProUGUI _hpText = null;

    // 수치 데이터
    protected int _maxHp = 1; // 최대 체력
    public int maxHp => _maxHp;
    protected int _hp = 1; // 현재 체력
    public int hp => _hp;
    public bool IsLived => _hp > 0; // 살아있누?

    protected virtual void Start()
    {
        _mainModule = GetComponent<BaseMainModule>();
        _hp = _mainModule.DataSO.hp;
        _maxHp= _mainModule.DataSO.hp;
        _hpSlider.minValue = 0;
        _hpSlider.maxValue = _hp;
        _hpSlider.value = _hp;
        //_hpText?.SetText($"{_hp} / {_mainModule.DataSO.hp}");
        _hpText?.SetText((_hpSlider.normalizedValue * 100f).ToString("N0") + "%");
    }

    public virtual void Died()
    {
        _mainModule.enabled = false;
        OnDie?.Invoke();
        StopAllCoroutines();
        StartCoroutine(DieAnimationCoroutine());
    }

    private IEnumerator DieAnimationCoroutine()
    {
        bool isGameDie = false;
        isGameDie = TurnManager.Instance.GameEndCheck(_mainModule.entityType);
        if(isGameDie)
        {
            TurnManager.Instance.GameEnd();
            CameraManager.Instance.CameraSelect(VCamOne);
            VCamOne.Follow = _mainModule.transform;
        }

        _mainModule.Agent.ResetPath();
        _mainModule.animator.Play("Die");
        if (_mainModule.entityType == EntityType.Player)
        {
            PlayerMainModule m = _mainModule as PlayerMainModule;
            _mainModule.transform.DOMove(_mainModule.transform.position + (m.ModelController.transform.forward * -1f), 1.6f);
        }
        else
        {
            _mainModule.transform.DOMove(_mainModule.transform.position + (_mainModule.transform.forward * -0.5f), 1.6f);
        }
        _mainModule.animator.Update(0);
        yield return new WaitUntil(() => _mainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Die") == false);
        _mainModule.transform.DOKill();

        Destroy(gameObject);
    }

    public virtual void ApplyDamage(int dmg, ElementType elementType, bool critical, bool isPlayer) // 기본적 피격 수행
    {
        if (IsLived == false)
            return;

        int realDmg = dmg;
        if (elementType == _mainModule.GetWeak)
        {
            realDmg = Mathf.RoundToInt(dmg * 1.25f);
            if (isPlayer)
                TurnManager.Instance.PlusTurnCheck();
        }
        else if (elementType == _mainModule.GetStrong)
        {
            realDmg = Mathf.RoundToInt(dmg * 0.75f);
            if (isPlayer)
                TurnManager.Instance.LoseTurnCheck();
        }
        realDmg -= _mainModule.DataSO.normalDef;
        if (realDmg <= 0)
            realDmg = 0;

        if (_hpCoroutine != null)
            StopCoroutine(_hpCoroutine);
        _hpCoroutine = StartCoroutine(HpDownCoroutine(_hp, realDmg));
        _hp -= realDmg;
        _hp = Mathf.Clamp(_hp, 0, _mainModule.DataSO.hp);
        if (_hp <= 0)
        {
            PopupUtility.PopupDamage(transform.position, realDmg, critical, elementType, "격파");
        }
        else
        {
            PopupUtility.PopupDamage(transform.position, realDmg, critical, elementType);
        }
        _mainModule.HpDownAction?.Invoke(_hp);
        _hpText?.SetText(((_hp / (float)_mainModule.DataSO.hp) * 100f).ToString("N0") + "%");

        if (IsLived == false)
        {
            _hpSlider.value = 0;
            _hpText?.SetText("0%");
            Died();
        }
        else
        {
            _mainModule.animator.Play("Damaged");
        }
    }

    public void Healing(int amount, ElementType elementType)
    {
        if (_hpCoroutine != null)
            StopCoroutine(_hpCoroutine);
        _hp += amount;
        _hpSlider.value = _hp;
        _hp = Mathf.Clamp(_hp, 0, _mainModule.DataSO.hp);
        _mainModule.HpDownAction?.Invoke(_hp);
        _hpText?.SetText(((_hp / (float)_mainModule.DataSO.hp) * 100f).ToString("N0") + "%");
        PopupUtility.PopupDamage(transform.position, amount, false, elementType, "치유");
    }

    private IEnumerator HpDownCoroutine(float start, int dmg) // 슬라이더 소모 애니메이션
    {
        float delta = 0f;
        while (delta <= 1f)
        {
            delta += Time.deltaTime * 2f;
            _hpSlider.value = start - (dmg * delta);
            yield return null;
        }
        _hpSlider.value = start - dmg;
    }
}
