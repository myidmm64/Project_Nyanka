using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using DG.Tweening;

public class CameraManager : MonoSingleTon<CameraManager>
{
    private float _originLens = 0f;

    private Coroutine _zoomCoroutine = null;
    private Coroutine _shakeCoroutine = null;

    private float _currentShakeAmount = 0f;

    [SerializeField]
    private CinemachineVirtualCamera _currentCam = null;
    [SerializeField]
    private CinemachineBasicMultiChannelPerlin _currentNoise = null;

    private List<CinemachineVirtualCamera> cams = new List<CinemachineVirtualCamera>();

    private CinemachineVirtualCamera _lastCam = null;
    private Sequence _sizeSeq = null;

    /// <summary>
    /// 카메라 초기화
    /// </summary>
    private void Awake()
    {
        cams.AddRange(GetComponentsInChildren<CinemachineVirtualCamera>());
        _originLens = FindObjectOfType<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        CameraSelect(VCamTwo);
    }


    /// <summary>
    /// 카메라 선택 함수
    /// </summary>
    public void CameraSelect(CinemachineVirtualCamera cam)
    {
        if (GameManager.Instance.LivePlayers.Count <= 0)
            return;

        Vector3 nextPos = VCamTwo.transform.position;

        float max = GameManager.Instance.LivePlayers[0].transform.position.z;
        float min = GameManager.Instance.LivePlayers[0].transform.position.z;
        for(int i = 0; i < GameManager.Instance.LivePlayers.Count; i++)
            if (min > GameManager.Instance.LivePlayers[i].transform.position.z)
                min = GameManager.Instance.LivePlayers[i].transform.position.z;
        for (int i = 0; i < GameManager.Instance.LivePlayers.Count; i++)
            if (max < GameManager.Instance.LivePlayers[i].transform.position.z)
                max = GameManager.Instance.LivePlayers[i].transform.position.z;

        nextPos.z = (max + min) / 2 - 2.5f;

        VCamTwo.transform.position = nextPos;

        CartCamReset();
        for(int i = 0; i < cams.Count; i++)
            if (cam == cams[i])
            {
                cams[i].gameObject.SetActive(true);
                _currentCam = cams[i];
                _currentNoise = cams[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            else
                cams[i].gameObject.SetActive(false);
    }

    /// <summary>
    /// 마지막으로 고른 캠 선택
    /// </summary>
    public void LastCamSelect()
    {
        CartCamReset();
        if (_lastCam != null)
            CameraSelect(_lastCam);
    }

    /// <summary>
    /// 트랙을 따라가는 캠 선택
    /// </summary>
    public void CartCamSelect(CinemachineSmoothPath path,  Transform look, float speed, LayerMask cullingMask = default(LayerMask))
    {
        _lastCam = _currentCam;
        CameraSelect(CartCam);
        if(cullingMask != default(LayerMask))
            Cam.cullingMask = cullingMask;
        Cam.orthographic = false;
        CartCam.LookAt = look;
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        cart.m_Path = path;
        cart.m_Position = 0f;
        cart.m_Speed = speed;
        cart.enabled = true;
    }

    /// <summary>
    /// 카트캠 리셋
    /// </summary>
    public void CartCamReset()
    {
        Cam.orthographic = true;
        
        CartCam.Follow = null;
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        cart.m_Path = null;
        cart.m_Position = 0f;
        cart.m_Speed = 0f;
        cart.enabled = false;
        Cam.cullingMask = -1;
    }

    /// <summary>
    /// 트랙 따라는 캠 선택 도중 값 변경
    /// </summary>
    public void CartUpdate(float? orthoSize, float? position, float? speed)
    {
        CinemachineDollyCart cart = CartCam.GetComponent<CinemachineDollyCart>();
        if (_currentCam == CartCam)
        {
            if (orthoSize != null)
                OrthoDotw(orthoSize.Value);
            if (position != null)
                cart.m_Position = position.Value;
            if (speed != null)
                cart.m_Speed = speed.Value;
        }
    }

    /// <summary>
    /// 오쏘그래피 줌 설정
    /// </summary>
    private void OrthoDotw(float endVal)
    {
        if (_sizeSeq != null)
            _sizeSeq.Kill();
        _sizeSeq = DOTween.Sequence();
        _sizeSeq.Append(DOTween.To(() => CartCam.m_Lens.FieldOfView, x => CartCam.m_Lens.FieldOfView = x, endVal, 0.5f));
        //_sizeSeq.Append(DOTween.To(() => CartCam.m_Lens.OrthographicSize, x => CartCam.m_Lens.OrthographicSize = x, endVal, 0.5f));
    }

    public void CompletePrevFeedBack()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }

        _currentNoise.m_FrequencyGain = 0; // 흔드는 빈도 정도
        _currentNoise.m_AmplitudeGain = 0;
        _currentShakeAmount = 0f;
    }

    /// <summary>
    /// 카메라 흔들흔들
    /// </summary>
    /// <param name="흔들리는 정도"></param>
    /// <param name="흔들리는 빈도"></param>
    /// <param name="흔들릴 시간"></param>
    public void CameraShake(float amplitude, float intensity, float duration, bool conti = false)
    {
        if (_currentShakeAmount > amplitude)
        {
            return;
        }
        CompletePrevFeedBack();

        _currentNoise.m_AmplitudeGain = amplitude; // 흔들리는 정도
        _currentNoise.m_FrequencyGain = intensity; // 흔드는 빈도 정도

        _currentShakeAmount = _currentNoise.m_AmplitudeGain;

        _shakeCoroutine = StartCoroutine(ShakeCorutine(amplitude, duration, conti));
    }

    private IEnumerator ShakeCorutine(float amplitude, float duration, bool conti)
    {
        float time = duration;
        while (time >= 0)
        {
            if (conti)
                _currentNoise.m_AmplitudeGain = amplitude;
            else
                _currentNoise.m_AmplitudeGain = Mathf.Lerp(0, amplitude, time / duration);

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
        float currentLens = _currentCam.m_Lens.FieldOfView;

        while (time <= duration)
        {
            nextLens = Mathf.Lerp(currentLens, maxValue, time / duration);
            Debug.Log(time / duration);
            _currentCam.m_Lens.FieldOfView = nextLens;
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
