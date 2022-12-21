using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAttackModule : MonoBehaviour
{
    protected BaseMainModule _mainModule = null;
    protected BaseSkillModule _skillModule = null;

    // 애니메이션 이벤트들
    [SerializeField]
    protected PlayerAnimationEvent _normalAttackEvent = null;
    [SerializeField]
    protected PlayerAnimationEvent _normalSkillEvent = null;

    [field: SerializeField]
    protected UnityEvent OnAttackEnd = null;

    private void Start()
    {
        _mainModule = GetComponent<BaseMainModule>();
        _skillModule = GetComponent<BaseSkillModule>();
    }

    public virtual IEnumerator Attack()
    {
        List<Cell> cells = CellUtility.SearchCells(_mainModule.CellIndex, _mainModule.DataSO.normalAttackRange, true);
        if (cells.Count == 0) yield break;

        yield return new WaitForSeconds(0.1f);
        //셀들 받아오기
        _mainModule.animator.Play("Attack");
        _mainModule.animator.Update(0);
        yield return new WaitUntil(() => _mainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false);
        for (int i = 0; i < cells.Count; i++)
        {
            int dmg = Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage);
            cells[i].CellAttack(dmg, _mainModule.elementType, _mainModule.entityType);
        }
        if (cells.Count > 0 && _mainModule.entityType == EntityType.Player)
            TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1);
        yield break;
    }

    public virtual IEnumerator Skill()
    {
        List<Cell> cells = CellUtility.SearchCells(_mainModule.CellIndex, _mainModule.DataSO.normalAttackRange, true);
        if (cells.Count == 0) yield break;

        yield return new WaitForSeconds(0.1f);
        //셀들 받아오기
        _mainModule.animator.Play("Skill");
        _mainModule.animator.Update(0);
        yield return new WaitUntil(() => _mainModule.animator.GetCurrentAnimatorStateInfo(0).IsName("Skill") == false);
        for (int i = 0; i < cells.Count; i++)
        {
            int dmg = Random.Range(_mainModule.MinDamage, _mainModule.MaxDamage);
            cells[i].CellAttack(dmg, _mainModule.elementType, _mainModule.entityType);
        }
        if (cells.Count > 0 && _mainModule.entityType == EntityType.Player)
            TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1);
        yield break;
    }
}
