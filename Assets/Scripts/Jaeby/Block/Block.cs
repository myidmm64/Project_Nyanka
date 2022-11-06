using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private ElementType _elementType = ElementType.None;
    public ElementType elementType => _elementType;
}
