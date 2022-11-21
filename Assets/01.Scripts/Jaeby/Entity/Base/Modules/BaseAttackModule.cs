using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackModule : MonoBehaviour
{
    protected BaseMainModule _mainModule = null;

    private void Start()
    {
        _mainModule = GetComponent<BaseMainModule>();
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
            cells[i].CellAttack(_mainModule.DataSO.normalAtk, _mainModule.DataSO.elementType, _mainModule.entityType);
        if (cells.Count > 0 && _mainModule.entityType == EntityType.Player)
            TurnManager.Instance.BattlePointChange(TurnManager.Instance.BattlePoint + 1);
        yield break;
    }
}
