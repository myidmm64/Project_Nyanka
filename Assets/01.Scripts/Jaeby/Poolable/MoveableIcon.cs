using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableIcon : PoolAbleObject
{
    public override void Init_Pop()
    {
        transform.position = Vector3.zero;
    }

    public override void Init_Push()
    {
        transform.position = Vector3.zero;
    }
}
