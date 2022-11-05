using MapTileGridCreator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
