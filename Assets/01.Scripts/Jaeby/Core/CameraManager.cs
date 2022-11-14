using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private static CinemachineVirtualCamera _cmVCam = null;

    private CinemachineBasicMultiChannelPerlin _noise = null;

    public static CameraManager instance = null;

    private float _originLens = 0f;

    private Coroutine _zoomCoroutine = null;
    private Coroutine _shakeCoroutine = null;

    private float _currentShakeAmount = 0f;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (_cmVCam == null)
        {
            _cmVCam = FindObjectOfType<CinemachineVirtualCamera>();
        }

        _noise = _cmVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _originLens = _cmVCam.m_Lens.FieldOfView;
    }

    public void CameraSelect(CinemachineVirtualCamera cam)
    {
        CinemachineVirtualCamera[] cams = new CinemachineVirtualCamera[3];
        cams[0] = VCamOne;
        cams[1] = VCamTwo;
        cams[2] = CartCam;
        for(int i = 0; i < cams.Length; i++)
            if (cam == cams[i])
                cams[i].gameObject.SetActive(true);
            else
                cams[i].gameObject.SetActive(false);
    }

    public void CartCamSelect(CinemachineSmoothPath path,  Transform follow, float speed)
    {
        CameraSelect(CartCam);
        CartCam.Follow = follow;
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        cart.m_Path = path;
        cart.m_Position = 0f;
        cart.m_Speed = speed;
        cart.enabled = true;
    }

    public void CartCamReset()
    {
        CartCam.Follow = null;
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        cart.m_Path = null;
        cart.m_Position = 0f;
        cart.m_Speed = 0f;
        cart.enabled = false;
        CameraSelect(VCamTwo);
    }

    public void CompletePrevFeedBack()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }

        _noise.m_FrequencyGain = 0; // Èçµå´Â ºóµµ Á¤µµ
        _noise.m_AmplitudeGain = 0;
        _currentShakeAmount = 0f;
    }

    /// <summary>
    /// Ä«¸Þ¶ó ÈçµéÈçµé
    /// </summary>
    /// <param name="Èçµé¸®´Â Á¤µµ"></param>
    /// <param name="Èçµé¸®´Â ºóµµ"></param>
    /// <param name="Èçµé¸± ½Ã°£"></param>
    public void CameraShake(float amplitude, float intensity, float duration, bool conti = false)
    {
        if (_currentShakeAmount > amplitude)
        {
            return;
        }
        CompletePrevFeedBack();

        _noise.m_AmplitudeGain = amplitude; // Èçµé¸®´Â Á¤µµ
        _noise.m_FrequencyGain = intensity; // Èçµå´Â ºóµµ Á¤µµ

        _currentShakeAmount = _noise.m_AmplitudeGain;

        _shakeCoroutine = StartCoroutine(ShakeCorutine(amplitude, duration, conti));
    }

    private IEnumerator ShakeCorutine(float amplitude, float duration, bool conti)
    {
        float time = duration;
        while (time >= 0)
        {
            if (conti)
                _noise.m_AmplitudeGain = amplitude;
            else
                _noise.m_AmplitudeGain = Mathf.Lerp(0, amplitude, time / duration);

            yield return null;
            time -= Time.deltaTime;
        }
        CompletePrevFeedBack();
    }

    public void ZoomCamera(float maxValue, float time)
    {
        CameraReset();

        _zoomCoroutine = StartCoroutine(ZoomCoroutine(maxValue, time));
    }

    private IEnumerator ZoomCoroutine(float maxValue, float duration)
    {
        float time = 0f;
        float nextLens = 0f;
        float currentLens = _cmVCam.m_Lens.FieldOfView;

        while (time <= duration)
        {
            nextLens = Mathf.Lerp(currentLens, maxValue, time / duration);
            Debug.Log(time / duration);
            _cmVCam.m_Lens.FieldOfView = nextLens;
            yield return null;
            time += Time.deltaTime;
        }
    }

    public void CameraReset()
    {
        if (_zoomCoroutine != null)
        {
            StopCoroutine(_zoomCoroutine);
        }
    }
}
