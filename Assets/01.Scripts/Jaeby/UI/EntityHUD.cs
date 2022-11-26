using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityHUD : MonoBehaviour
{
    [SerializeField]
    private Image _elementImage = null;
    [SerializeField]
    private Image _weakSubImage = null;
    [SerializeField]
    private Image _strongSubImage = null;
    [SerializeField]
    private Image _classImage = null;
    [SerializeField]
    private TextMeshProUGUI _avrDamageText = null;
    [SerializeField]
    private TextMeshProUGUI _elementDamageText = null;

    private ElementType _element = ElementType.NONE;
    private EntityType _entityType = EntityType.None;

    public void Init(BaseMainModule targetModule)
    {
        ImageData elementData = ImageManager.Instance.GetImageData(targetModule.DataSO.elementType);
        ImageData classData = ImageManager.Instance.GetImageData(targetModule.DataSO.classType);
        float avrDamage = (targetModule.MinDamage + targetModule.MaxDamage) * 0.5f;

        _elementImage.sprite = elementData.sprite;
        _elementImage.color = elementData.color;
        _weakSubImage.sprite = elementData.sprite;
        _strongSubImage.sprite = elementData.sprite;

        Color weakColor = elementData.color;
        Color strongColor = new Color((1.0f - weakColor.r), (1.0f - weakColor.g), (1.0f - weakColor.b));
        weakColor.a = 200 / 255f;
        _weakSubImage.color = weakColor;
        _strongSubImage.color = strongColor;

        _classImage.sprite = classData.sprite;
        _classImage.color = classData.color;
        _avrDamageText.SetText(avrDamage.ToString("N0"));

        ClickManager.Instance.EntitySelectedAction += TryImageEnable;
        ImageDisable();
        _element = targetModule.DataSO.elementType;
        _entityType = targetModule.entityType;
    }

    private void TryImageEnable(BaseMainModule targetModule)
    {
        if(targetModule == null)
        {
            ImageDisable();
            return;
        }

        if (_element == targetModule?.GetStrong && _entityType != targetModule.entityType)
            ImageEnable(true);
        else if (_element == targetModule?.GetWeak && _entityType != targetModule.entityType)
            ImageEnable(false);
        else
            ImageDisable();
    }

    private void ImageEnable(bool isWeak)
    {
        ImageDisable();
        if (isWeak)
        {
            _elementDamageText.SetText("약점");
            _weakSubImage?.gameObject.SetActive(true);
        }
        else
        {
            _elementDamageText.SetText("강점");
            _strongSubImage?.gameObject.SetActive(true);
        }
    }

    private void ImageDisable()
    {
        _weakSubImage?.gameObject.SetActive(false);
        _strongSubImage?.gameObject.SetActive(false);
        _elementDamageText.SetText("");
    }

    private void OnDestroy()
    {
        //ClickManager.Instance.EntitySelectedAction -= TryWeakImageEnable;
    }
}
