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
            return _grid;
        }
        set => _grid = value;
    }

    private static Camera _cam = null;
    public static Camera Cam
    {
        get
        {
            if(_cam == null)
                _cam = Camera.main;
            return _cam;
        }
    }

    private static CinemachineVirtualCamera _vCamOne = null;
    private static CinemachineVirtualCamera _vCamTwo = null;
    private static CinemachineVirtualCamera _cartCam = null;

    public static CinemachineVirtualCamera VCamOne
    {
        get
        {
            if (_vCamOne == null)
                _vCamOne = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            return _vCamOne;
        }
    }
    public static CinemachineVirtualCamera VCamTwo
    {
        get
        {
            if (_vCamTwo == null)
                _vCamTwo = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
            return _vCamTwo;
        }
    }
    public static CinemachineVirtualCamera CartCam
    {
        get
        {
            if (_cartCam == null)
                _cartCam = GameObject.FindObjectOfType<CameraManager>().transform.GetChild(2).GetComponent<CinemachineVirtualCamera>();
            return _cartCam;
        }
    }

    private static  Canvas _cameraCanvas = null;
    public static Canvas CameraCanvas
    {
        get
        {
            if(_cameraCanvas == null)
                _cameraCanvas = GameObject.Find("CameraCanvas").GetComponent<Canvas>();
            return _cameraCanvas;
        }
    }
}
