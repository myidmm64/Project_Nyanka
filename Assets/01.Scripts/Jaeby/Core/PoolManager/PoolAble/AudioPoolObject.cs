using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPoolObject : PoolAbleObject
{
    [SerializeField] private AudioSource source;
    public override void Init_Pop()
    {
        //DoNothing
    }

    public override void Init_Push()
    {
        source.Stop();
    }
    /// <summary>
    /// 오디오 재생
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public void Play(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        StartCoroutine(WaitForPush(source.clip.length * 1.05f));
    }
    IEnumerator WaitForPush(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        PoolManager.Instance.Push(PoolType, gameObject);
    }
}
