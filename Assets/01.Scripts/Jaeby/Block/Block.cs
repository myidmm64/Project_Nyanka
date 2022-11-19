using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private LayerMask _mask = 0;
    public LayerMask Mask => _mask;
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
    public void Explosion(int dmg, Vector3Int index, Entity entity)
    {
        StartCoroutine(ExplosionWait(dmg, index, entity, 0.5f));
    }

    private IEnumerator ExplosionWait(int dmg, Vector3Int index, Entity entity, float duration)
    {
        Debug.Log($"{_elementType.ToString()}으로 같음");
        yield return new WaitForSeconds(duration);
        entity?.ApplyDamage(Random.Range(Mathf.RoundToInt(dmg * 0.9f), Mathf.RoundToInt(dmg * 1.1f)), _elementType, false);
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
            case ElementType.Thunder:
                c = new Color(1,0,1);
                break;
            case ElementType.SIZE:
                break;
            default:
                break;
        }
        GetComponent<MeshRenderer>().material.color = c;
    }
}
