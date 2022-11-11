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

public abstract class Entity : MonoBehaviour
{
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

    protected virtual void Start()
    {
        _hp = _dataSO.hp;
        _agent = GetComponent<NavMeshAgent>();
    }

    public abstract IEnumerator Move(Vector3Int v);

    public virtual IEnumerator Attack()
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, _dataSO.normalAttackRange, true);
        if (cells.Count == 0) yield break;

        yield return new WaitForSeconds(0.1f);
        //���� �޾ƿ���
        _animator.Play("Attack");
        _animator.Update(0);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false);
        for(int i = 0; i <cells.Count; i++)
            cells[i].TryAttack(_dataSO.normalAtk, _dataSO.elementType, _entityType);
        if (cells.Count > 0 && _entityType == EntityType.Player)
            TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1);
        yield break;
    }

    List<GameObject> obj = new List<GameObject>();
    [SerializeField]
    private GameObject _testObj = null;
    /// <summary>
    /// indexes�� ���� ������ �����մϴ�. ignore�� false�� ��, �� ���� ������Ʈ�� ������ �����մϴ�.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    protected void ViewStart(List<Vector3Int> indexes, bool ignore)
    {
        List<Cell> cells = CellUtility.SearchCells(CellIndex, indexes, ignore);
        for (int i = 0; i < cells.Count; i++)
        {
            if (_testObj == null) return;
            GameObject o = Instantiate(_testObj, cells[i].GetIndex() + Vector3.up * 0.5f, Quaternion.identity);
            o.transform.localScale = Vector3.one * 0.5f;
            obj.Add(o);
        }
    }

    /// <summary>
    /// indexes�� ���� �� ������ �����մϴ�. ignore�� false�� ��, �� ���� ������Ʈ�� ������ �����մϴ�.
    /// </summary>
    /// <param name="indexes"></param>
    /// <param name="ignore"></param>
    protected void ViewEnd()
    {
        for (int i = 0; i < obj.Count; i++)
            Destroy(obj[i]);
    }

    private void OnMouseEnter()
    {
        Targeted();
    }

    private void OnMouseExit()
    {
        TargetEnd();
    }

    /// <summary>
    /// �ڽ��� ����� ������ �� ����, val�� true�� �÷��̾��� ������ ����
    /// </summary>
    /// <param name="val"></param>
    public abstract void PhaseChanged(bool val);

    public abstract void Targeted();

    public abstract void TargetEnd();

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
}
