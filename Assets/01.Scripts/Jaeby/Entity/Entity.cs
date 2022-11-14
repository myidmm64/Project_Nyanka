using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;
using UnityEngine.AI;

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
    Wind,
    SIZE
}

public abstract class Entity : MonoBehaviour, ISelectable
{
    #region ����
    protected EntityType _entityType = EntityType.None;
    public EntityType entityType => _entityType;
    [SerializeField]
    protected Animator _animator = null; // �ִϸ�����
    [SerializeField]
    protected EntityDataSO _dataSO = null; // SO
    public EntityDataSO DataSO => _dataSO;
    protected Vector3Int _cellIndex = Vector3Int.zero; // ���� ���ִ� ���� �ε���
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

    [SerializeField]
    protected NavMeshAgent _agent = null; // �׺�޽�

    protected int _hp = 0; // ���� ü��
    public bool IsLived => _hp > 0; // ����ִ�?

    public ElementType GetWeak // ���� �Ӽ�
    {
        get
        {
            return (ElementType)(((int)_dataSO.elementType + 1) % (int)ElementType.SIZE);
        }
    }

    public ElementType GetStrong // ���� �Ӽ�
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
    #endregion

    protected virtual void Start()
    {
        _hp = _dataSO.hp;
    }
    /// <summary>
    /// �ڽ��� ����� ������ �� ����, val�� true�� �÷��̾��� ������ ����
    /// </summary>
    /// <param name="val"></param>
    public abstract void PhaseChanged(bool val);
    public abstract void Targeted();
    public abstract void TargetEnd();
    protected abstract void ChildSelected();
    protected abstract void ChildSelectEnd();
    public abstract IEnumerator Move(Vector3Int v);


    public void Selected()
    {
        Debug.Log("����Ʈ");
        VCamOne.Follow = transform;
        VCamOne.gameObject.SetActive(true);
        CameraManager.instance.CameraSelect(VCamOne);
        ClickManager.Instance.ClickModeSet(LeftClickMode.JustCell, false);
        ChildSelected();

        ViewStart();
    }

    public void SelectEnd()
    {
        Debug.Log("����Ʈ ����");
        VCamOne.Follow = null;
        VCamTwo.gameObject.SetActive(true);
        CameraManager.instance.CameraSelect(VCamTwo);
        ClickManager.Instance.ClickModeSet(LeftClickMode.AllClick, false);
        ChildSelectEnd();

        ViewEnd();
    }

    public virtual IEnumerator Attack()
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, _dataSO.normalAttackRange, true);
        if (cells.Count == 0) yield break;

        yield return new WaitForSeconds(0.1f);
        //���� �޾ƿ���
        _animator.Play("Attack");
        _animator.Update(0);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false);
        for (int i = 0; i < cells.Count; i++)
            cells[i].TryAttack(_dataSO.normalAtk, _dataSO.elementType, _entityType);
        if (cells.Count > 0 && _entityType == EntityType.Player)
            TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1);
        yield break;
    }

    protected void ViewStart()
    {
        CubeGrid.ViewRange(GridType.Normal, CellIndex, _dataSO.normalMoveRange, false);
        CubeGrid.ViewRange(GridType.Attack, CellIndex, GetAttackVectorByDirections(AttackDirection.Up, _dataSO.normalAttackRange), true);
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
        Debug.Log("�����");
        Destroy(gameObject);
    }

    public void ApplyDamage(int dmg, ElementType elementType)
    {
        if (elementType == GetWeak)
        {
            Debug.Log("ũ��Ƽ�� !!");
            _hp -= dmg * 2;
            TurnManager.Instance.PlusTurnCheck();
        }
        else if (elementType == GetStrong)
        {
            Debug.Log("������ !!");
            _hp -= Mathf.RoundToInt(dmg * 0.5f);
            TurnManager.Instance.LoseTurnCheck();
        }
        else
            _hp -= dmg;

        Debug.Log($"���� HP : {_hp}");
        if (IsLived == false)
        {
            Died();
        }
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
