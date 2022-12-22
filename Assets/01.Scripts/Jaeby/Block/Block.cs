using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private LayerMask _mask = 0;
    public LayerMask Mask => _mask; // Cell에서 사용
    [SerializeField]
    private ElementType _elementType = ElementType.NONE; // 셀의 속성
    public ElementType elementType => _elementType;

    private MeshRenderer _meshRenderer = null; // 캐싱 준비

    private void Start()
    {
        _elementType = (ElementType)(Random.Range(1, (int)ElementType.SIZE));
        ChangeBlock(_elementType);
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// 이펙트만 생성
    /// </summary>
    public void JustEffect(Vector3Int index, bool change)
    {
        ParticlePool a = PoolManager.Instance.Pop(PoolType.Bullet) as ParticlePool;
        a.transform.position = index;

        if(change)
        {
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

    /// <summary>
    /// 속성 변경
    /// </summary>
    public void ChangeBlock(ElementType type)
    {
        Color c = Color.white;
        switch (type)
        {
            case ElementType.NONE:
                break;
            case ElementType.Fire:
                c = ImageManager.Instance.GetImageData(ElementType.Fire).color;
                break;
            case ElementType.Water:
                c = ImageManager.Instance.GetImageData(ElementType.Water).color;
                break;
            case ElementType.Thunder:
                c = ImageManager.Instance.GetImageData(ElementType.Thunder).color;
                break;
            case ElementType.SIZE:
                break;
            default:
                break;
        }
        c.r -= 100f / 255;
        c.g -= 100f / 255;
        c.b -= 100f / 255;
        _meshRenderer.material.color = c;
        _elementType = type;
    }
}
