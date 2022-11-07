using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private ElementType _elementType = ElementType.NONE;
    public ElementType elementType => _elementType;

    private void Start()
    {
        //_elementType = (ElementType)(Random.Range(1, (int)ElementType.SIZE));
        _elementType = ElementType.Water;
        //ChangeBlock(_elementType);
    }

    public void Explosion(Vector3Int index)
    {
        ParticlePool a = PoolManager.Instance.Pop(PoolType.Bullet) as ParticlePool;
        a.transform.position = index;
        _elementType = (ElementType)(Random.Range(1, (int)ElementType.SIZE));
        ChangeBlock(_elementType);
    }

    public void ChangeBlock(ElementType type)
    {
        Color c = Color.white;
        switch (type)
        {
            case ElementType.NONE:
                break;
            case ElementType.Fire:
                c = Color.red;
                break;
            case ElementType.Water:
                c = Color.black;
                break;
            case ElementType.Leaf:
                c = Color.green;
                break;
            case ElementType.Light:
                c = Color.yellow;
                break;
            case ElementType.SIZE:
                break;
            default:
                break;
        }
        GetComponent<MeshRenderer>().material.color = c;
    }
}
