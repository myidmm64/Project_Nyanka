using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public enum EntityType
{
    None,
    Player,
    Enemy
}

[System.Serializable]
public enum ElementType
{
    NONE,
    Fire,
    Water,
    Thunder,
    SIZE
}

public abstract class Entity : MonoBehaviour, ISelectable
{
    #region 변수
    [SerializeField]
    private Slider _hpSlider = null;
    private Coroutine _hpCoroutine = null;
    [SerializeField]
    private TextMeshProUGUI _hpText = null;

    protected EntityType _entityType = EntityType.None;
    public EntityType entityType => _entityType;
    public Animator _animator = null; // 애니메이터
    [SerializeField]
    protected EntityDataSO _dataSO = null; // SO
    public EntityDataSO DataSO => _dataSO;
    protected Vector3Int _cellIndex = Vector3Int.zero; // 현재 서있는 셀의 인덱스
    public Vector3Int CellIndex
    {
        get
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
                _cellIndex = hit.collider.GetComponent<Cell>().GetIndex();
            return _cellIndex;
        }
        set => _cellIndex = value;
    }

    public NavMeshAgent _agent = null; // 네브메시

    protected int _hp = 1; // 현재 체력
    public bool IsLived => _hp > 0; // 살아있누?

    public ElementType GetWeak // 약점 속성
    {
        get
        {
            return (ElementType)(((int)_dataSO.elementType + 1) % (int)ElementType.SIZE);
        }
    }

    public ElementType GetStrong // 강점 속성
    {
        get
        {
            int temp = (int)_dataSO.elementType - 1;
            if (temp == 0)
                return (ElementType)((int)ElementType.SIZE - 1);
            return (ElementType)temp;
        }
    }

    private bool _selectedFlag = false;
    public bool SelectedFlag { get => _selectedFlag; set => _selectedFlag = value; }

    protected bool _selectable = true;
    public bool Selectable => _selectable;
    #endregion

    protected virtual void Start()
    {
        _hp = _dataSO.hp;
        _hpSlider.minValue = 0;
        _hpSlider.maxValue = _hp;
        _hpSlider.value = _hp;
        _hpText.SetText($"{_hp} / {_dataSO.hp}");
    }

    public abstract void PhaseChanged(bool val); // 페이즈 종료되었을 때 실행
    public abstract void Targeted(); // OnMouseEnter
    public abstract void TargetEnd(); // OnMouseExit
    protected abstract void ChildSelected(); // 인터페이스 구현
    protected abstract void ChildSelectEnd(); // 인터페이스 구현
    public abstract IEnumerator Move(Vector3Int v); // 이동 함수


    public void Selected()
    {
        if (_selectable == false) return;
        Debug.Log("셀렉트");
        VCamOne.Follow = transform;
        CameraManager.Instance.CameraSelect(VCamOne);
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        ChildSelected();

        ViewStart();
    }

    public void SelectEnd()
    {
        Debug.Log("셀렉트 엔드");
        VCamOne.Follow = null;
        CameraManager.Instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        ChildSelectEnd();

        ViewEnd();
    }

    public virtual IEnumerator Attack()
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, _dataSO.normalAttackRange, true);
        if (cells.Count == 0) yield break;

        yield return new WaitForSeconds(0.1f);
        //셀들 받아오기
        _animator.Play("Attack");
        _animator.Update(0);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false);
        for (int i = 0; i < cells.Count; i++)
            cells[i].CellAttack(_dataSO.normalAtk, _dataSO.elementType, _entityType);
        if (cells.Count > 0 && _entityType == EntityType.Player)
            TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1);
        yield break;
    }

    protected void ViewStart()
    {
        ViewData(CellIndex);
    }

    public void ViewData(Vector3Int index)
    {
        CubeGrid.ViewRange(GridType.Normal, CellIndex, _dataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, index, GetAttackVectorByDirections(AttackDirection.Up, _dataSO.normalAttackRange), true);
    }

    protected void ViewEnd()
    {
        CubeGrid.ViewEnd();
    }


    public void Died()
    {
        Debug.Log("사망띠");
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void ApplyDamage(int dmg, ElementType elementType, bool critical, bool isPlayer)
    {
        if (IsLived == false)
            return;

        int realDmg = dmg;
        if (elementType == GetWeak)
        {
            realDmg = Mathf.RoundToInt(dmg * 1.5f);
            if(isPlayer)
                TurnManager.Instance.PlusTurnCheck();
        }
        else if (elementType == GetStrong)
        {
            realDmg = Mathf.RoundToInt(dmg * 0.5f);
            if (isPlayer)
                TurnManager.Instance.LoseTurnCheck();
        }

        PopupUtility.PopupDamage(transform.position, realDmg, critical, elementType);
        if (_hpCoroutine != null)
            StopCoroutine(_hpCoroutine);
        _hpCoroutine = StartCoroutine(HpDownCoroutine(realDmg));

    }

    private IEnumerator HpDownCoroutine(int dmg)
    {
        float delta = 0f;
        float start = _hp;
        _hp -= dmg;
        if (_hp <= 0)
            _hp = 0;
        _hpText.SetText($"{_hp} / {_dataSO.hp}");
        if (IsLived == false)
        {
            Died();
            yield break;
        }
        while (delta <= 1f)
        {
            delta += Time.deltaTime * 2f;
            _hpSlider.value = start - (dmg * delta);
            yield return null;   
        }
        _hpSlider.value = start - dmg;
    }

    public List<Vector3Int> GetAttackVectorByDirections(AttackDirection dir, List<Vector3Int> indexes)
    {
        List<Vector3Int> vecList = new List<Vector3Int>();
        float rot = 0f;
        switch (dir)
        {
            case AttackDirection.Up:
                rot = 0f;
                break;
            case AttackDirection.Right:
                rot = 90f;
                break;
            case AttackDirection.Left:
                rot = 270f;
                break;
            case AttackDirection.Down:
                rot = 180f;
                break;
            default:
                break;
        }
        for (int i = 0; i < indexes.Count; i++)
        {
            Vector3 v = Quaternion.AngleAxis(rot, transform.up) * indexes[i];
            vecList.Add(Vector3Int.RoundToInt(v));
        }
        return vecList;
    }

    private void OnMouseEnter()
    {
        Targeted();
    }

    private void OnMouseExit()
    {
        TargetEnd();
    }
}
