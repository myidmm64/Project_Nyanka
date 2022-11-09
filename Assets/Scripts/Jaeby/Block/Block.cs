using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private int _blockPlusDamage = 1;
    [SerializeField]
    private ElementType _elementType = ElementType.NONE;
    public ElementType elementType => _elementType;

    private void Start()
    {
        _elementType = (ElementType)(Random.Range(1, (int)ElementType.SIZE));
        //_elementType = ElementType.Water;
        ChangeBlock(_elementType);
    }

    public void JustEffect(Vector3Int index, bool change)
    {
        ParticlePool a = PoolManager.Instance.Pop(PoolType.Bullet) as ParticlePool;
        a.transform.position = index;

        if(change)
        {
            //_elementType = ElementType.Water;
            _elementType = (ElementType)(Random.Range(1, (int)ElementType.SIZE));
            ChangeBlock(_elementType);
        }
    }

    /// <summary>
    /// 후속타 함수
    /// </summary>
    /// <param name="index"></param>
    /// <param name="type"></param>
    /// <param name="entity"></param>
    public void Explosion(Vector3Int index, Entity entity)
    {
        StartCoroutine(ExplosionWait(index, entity, 1f));
    }

    private IEnumerator ExplosionWait(Vector3Int index, Entity entity, float duration)
    {
        Debug.Log($"{_elementType.ToString()}으로 같음");
        yield return new WaitForSeconds(duration);
        entity.ApplyDamage(_blockPlusDamage, entity.elementType);
        JustEffect(index, true);
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
                c = Color.blue;
                break;
            case ElementType.Wind:
                c = Color.white;
                break;
            case ElementType.SIZE:
                break;
            default:
                break;
        }
        GetComponent<MeshRenderer>().material.color = c;
    }
}
