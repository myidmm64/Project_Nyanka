using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BillBoardCamera : MonoBehaviour
{
    public bool flagReverse = false;

    public enum WorldUpDirection
    {
        up, down, left, right, forward, back
    };

    public WorldUpDirection worldUpDirection = WorldUpDirection.up;

    public Vector3 GetAxis(WorldUpDirection refWorldUpDirection)
    {
        switch (refWorldUpDirection)
        {
            case WorldUpDirection.down:
                return Vector3.down;
            case WorldUpDirection.forward:
                return Vector3.forward;
            case WorldUpDirection.back:
                return Vector3.back;
            case WorldUpDirection.left:
                return Vector3.left;
            case WorldUpDirection.right:
                return Vector3.right;
        }

        return Vector3.up;
    }

    void LateUpdate()
    {
        Vector3 posTarget = transform.position + Cam.transform.rotation * (flagReverse ? Vector3.forward : Vector3.back);
        Vector3 orientationTarget = Cam.transform.rotation * GetAxis(worldUpDirection); // 오른쪽 벡터를 회전시켜
        transform.LookAt(posTarget, orientationTarget);
    }
}
