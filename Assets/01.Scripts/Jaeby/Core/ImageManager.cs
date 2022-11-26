using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoSingleTon<ImageManager>
{
    [SerializeField]
    private ImageDataSO _dataSO = null;

    public ImageData GetImageData(ElementType type)
    {
        switch (type)
        {
            case ElementType.Fire:
                return _dataSO.fireImage;
            case ElementType.Water:
                return _dataSO.waterImage;
            case ElementType.Thunder:
                return _dataSO.thunderImage;
            default:
                Debug.LogError("���! �� �̻��� ������Ʈ ���ٴ��?");
                return default(ImageData);
        }
    }

    public ImageData GetImageData(EntityClassType type)
    {
        switch (type)
        {
            case EntityClassType.Warrior:
                return _dataSO.warriorImage;
            case EntityClassType.Shield:
                return _dataSO.shieldImage;
            case EntityClassType.Mage:
                return _dataSO.mageImage;
            case EntityClassType.Archer:
                return _dataSO.archerImage;
            case EntityClassType.Assassin:
                return _dataSO.assassinImage;
            default:
                Debug.LogError("���! �� �̻��� Ŭ���� ���ٴ��?");
                return default(ImageData);
        }
    }
}

