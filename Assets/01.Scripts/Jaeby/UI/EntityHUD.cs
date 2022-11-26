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
    private Image _weakImage = null;
    [SerializeField]
    private Image _classImage = null;
    [SerializeField]
    private TextMeshProUGUI _avrDamageText = null;

    private void Start()
    {
        ClickManager.Instance.EntitySelectedAction += TryWeakImageEnable;
    }

    public void Init(BaseMainModule targetModule)
    {

    }

    private void TryWeakImageEnable(BaseMainModule targetModule)
    {

    }

    private void OnDestroy()
    {
        //ClickManager.Instance.EntitySelectedAction -= TryWeakImageEnable;
    }
}
