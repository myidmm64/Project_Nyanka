using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 날짜 : 2021-01-18 PM 4:31:08
// 작성자 : Rito

public sealed class MeshAfterImage : AfterImageBase
{
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region .
    private MeshFilter[] TargetMeshFilterArray { get; set; }

    #endregion
    /***********************************************************************
    *                               Methods
    ***********************************************************************/
    #region .
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Init()
    {
        
        // 1. Target Meshes
        if (_containChildrenMeshes)
            TargetMeshFilterArray = GetComponentsInChildren<MeshFilter>();
        else
            TargetMeshFilterArray = new[] { GetComponent<MeshFilter>() };

        // 2. Queues
        FaderWaitQueue = new Queue<AfterImageFaderBase>();
        FaderRunningQueue = new Queue<AfterImageFaderBase>();

        // 3. Container
        _faderContainer = new GameObject($"{gameObject.name} AfterImage Container");
        _faderContainer.transform.SetPositionAndRotation(default, default);
        _faderContainer.transform.localScale = transform.localScale;

        _data.Mat = _afterImageMaterial;
        //_data.Mat.color = new Vector4(_data.Mat.color.r, _data.Mat.color.g, _data.Mat.color.b, 0.4f);
        isMotionTrail = false;
        
    }

    protected override void SetupFader(out AfterImageFaderBase fader)
    {
        GameObject faderGo = new GameObject($"{gameObject.name} AfterImage");
        faderGo.transform.SetParent(_faderContainer.transform);

        fader = faderGo.AddComponent<MeshAfterImageFader>();
        fader.Setup(TargetMeshFilterArray, _data, this);

    }

    #endregion
}