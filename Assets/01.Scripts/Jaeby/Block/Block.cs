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
    public void Explosion(int dmg, Vector3Int index, BaseMainModule entity)
    {
        StartCoroutine(ExplosionWait(dmg, index, entity, 0.5f));
    }

    private IEnumerator ExplosionWait(int dmg, Vector3Int index, BaseMainModule entity, float duration)
    {
        yield return new WaitForSeconds(duration);
        entity?.ApplyDamage(Random.Range(Mathf.RoundToInt(dmg * 0.9f), Mathf.RoundToInt(dmg * 1.1f)), _elementType, false, false);
        JustEffect(index, true);
    }

    public void ChangeBlock(ElementType type)
    {
        Color c = Color.white;
        Sprite s = null;
        switch (type)
        {
            case ElementType.NONE:
                break;
            case ElementType.Fire:
                c = ImageManager.Instance.GetImageData(ElementType.Fire).color;
                s = ImageManager.Instance.GetImageData(ElementType.Fire).sprite;
                break;
            case ElementType.Water:
                c = ImageManager.Instance.GetImageData(ElementType.Water).color;
                s = ImageManager.Instance.GetImageData(ElementType.Water).sprite;
                break;
            case ElementType.Thunder:
                c = ImageManager.Instance.GetImageData(ElementType.Thunder).color;
                s = ImageManager.Instance.GetImageData(ElementType.Thunder).sprite;
                break;
            case ElementType.SIZE:
                break;
            default:
                break;
        }
        c.r -= 100f / 255;
        c.g -= 100f / 255;
        c.b -= 100f / 255;
        GetComponent<MeshRenderer>().material.color = c;
        _elementType = type;
    }
}
