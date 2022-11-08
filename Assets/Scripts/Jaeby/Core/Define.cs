using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Define
{
    private static Grid3D _grid = null;
    public static Grid3D CubeGrid
    {
        get
        {
            if(_grid == null)
            {
                _grid = GameObject.FindObjectOfType<Grid3D>();
            }
            return _grid;
        }
    }

    private static Camera _cam = null;
    public static Camera Cam
    {
        get
        {
            if(_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }

    private static CinemachineVirtualCamera _vCamOne = null;
    private static CinemachineVirtualCamera _vCamTwo = null;

    public static CinemachineVirtualCamera VCamOne
    {
        get
        {
            if (_vCamOne == null)
            {
                _vCamOne = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            }
            return _vCamOne;
        }
    }
    public static CinemachineVirtualCamera VCamTwo
    {
        get
        {
            if (_vCamTwo == null)
            {
                _vCamTwo = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
            }
            return _vCamTwo;
        }
    }
}
