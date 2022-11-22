using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AIMainModule : BaseMainModule
{
    private List<(Vector3Int, int)> cells = new List<(Vector3Int, int)>();

    private PlayerMainModule target;

    public List<Vector3Int> MoveRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            //���߿� ���� ���� ���� ������ ���� ��� transMoveRange�ϱ� �÷��̾�ó��
            return so.normalMoveRange;
        }
    }
    public List<Vector3Int> AttackRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalAttackRange;
        }
    }
    public List<Vector3Int> SkillRange
    {
        get
        {
            EntityDataSO so = DataSO as EntityDataSO;
            return so.normalSkillRange;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void PhaseChange(PhaseType type)
    {

    }

    public override void Selected()
    {

    }

    public override void SelectEnd()
    {

    }
}
