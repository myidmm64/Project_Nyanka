using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerMoveModule : BaseMoveModule
{
    public void TryMove(Vector3Int v)
    {
        if (Moveable == false) return;
        PlayerMainModule module = _mainModule as PlayerMainModule;
        Debug.Log($"V {v} ce {module.CellIndex}");
        if (v == module.CellIndex)
        {
            module.PlayerIdle();
            return;
        }
        if (GetMoveableCheck(v) == false) return;
        StartCoroutine(Move(v));
        Moveable = false;
    }

    public override IEnumerator Move(Vector3Int v)
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        UIManager.Instance.UIDisable();

        ClickManager.Instance.ClickModeSet(LeftClickMode.Nothing, true);
        CubeGrid.ViewEnd();
        CubeGrid.ClcikViewEnd();

        module.animator.SetBool("Walk", true);
        module.animator.Update(0);
        Vector3 moveVec = v;
        moveVec.y = transform.position.y;
        module.ModelController.transform.LookAt(moveVec);
        module.Agent.SetDestination(moveVec);
        yield return new WaitUntil(() =>
            Vector3.Distance(transform.position, module.Agent.destination) <= module.Agent.stoppingDistance
            );
        module.CellIndex = v;
        module.animator.SetBool("Walk", false);

        UIManager.Instance.UIInit(module);
        module.TryAttack();
    }

    public bool GetMoveableCheck(Vector3Int index) // index가 무브 가능한지
    {
        PlayerMainModule module = _mainModule as PlayerMainModule;
        return CellUtility.CheckCell(_mainModule.CellIndex, index, module.MoveRange, false);
    }
}
