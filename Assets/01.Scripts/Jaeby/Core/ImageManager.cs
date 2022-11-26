using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoSingleTon<ImageManager>
{
    [Header("속성 이미지")]
    [SerializeField]
    private ImageData _fireImage;
    [SerializeField]
    private ImageData _waterImage ;
    [SerializeField]
    private ImageData _thunderImage;

    [Header("클래스 이미지")]
    [SerializeField]
    private ImageData _warriorImage;
    [SerializeField]
    private ImageData _shieldImage ;
    [SerializeField]
    private ImageData _mageImage;
    [SerializeField]
    private ImageData _archerImage ;
    [SerializeField]
    private ImageData _assassinImage;

    public ImageData GetImageData(ElementType type)
    {
        switch (type)
        {
            case ElementType.Fire:
                return _fireImage;
            case ElementType.Water:
                return _waterImage;
            case ElementType.Thunder:
                return _thunderImage;
            default:
                return default(ImageData);
        }
    }

    public ImageData GetImageData(EntityClassType type)
    {
        switch (type)
        {
            case EntityClassType.Warrior:
                return _warriorImage;
            case EntityClassType.Shield:
                return _shieldImage;
            case EntityClassType.Mage:
                return _mageImage;
            case EntityClassType.Archer:
                return _archerImage;
            case EntityClassType.Assassin:
                return _assassinImage;
            default:
                return default(ImageData);
        }
    }
}

[System.Serializable]
public struct ImageData
{
    public Sprite sprite;
    public Color color;
}
