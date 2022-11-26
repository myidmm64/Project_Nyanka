using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Image/Data")]
public class ImageDataSO : ScriptableObject
{
    [Header("�Ӽ� �̹���")]
    public ImageData fireImage;
    public ImageData waterImage;
    public ImageData thunderImage;

    [Header("Ŭ���� �̹���")]
    public ImageData warriorImage;
    public ImageData shieldImage;
    public ImageData mageImage;
    public ImageData archerImage;
    public ImageData assassinImage;
}

[System.Serializable]
public struct ImageData
{
    public Sprite sprite;
    public Color color;
}
