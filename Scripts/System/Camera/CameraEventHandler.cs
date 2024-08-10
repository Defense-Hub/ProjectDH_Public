using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraEventHandler : MonoBehaviour
{
    [Header("# Cam Info")]
    [SerializeField] private CinemachineVirtualCamera stageCam;
    // [SerializeField] private CinemachineVirtualCamera transitionCam;

    [Header("# Shake Info")]
    [SerializeField] private float frequencyValue;
    [SerializeField] private float amplitudeValue;
    private CinemachineBasicMultiChannelPerlin shakeCam;
    private Coroutine cameraShakeEffectCoroutine;
    private WaitForSeconds wait;
    
    private const int ActivePriority = 10;
    private const int InactivePriority = 0;
    private void Start()
    {
        GameManager.Instance.CameraEvent = this;
        shakeCam = stageCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    

    // public void ToggleCameraTransition(bool isTransition)
    // {
    //     stageCam.Priority = isTransition ? InactivePriority : ActivePriority;
    //     transitionCam.Priority = isTransition ? ActivePriority : InactivePriority;
    // }

    public void CameraShakeEffect(float duration)
    {
        if (wait == null)
        {
            wait = new WaitForSeconds(duration);
        }

        if (cameraShakeEffectCoroutine != null)
        {
            StopCoroutine(cameraShakeEffectCoroutine);
        }

        cameraShakeEffectCoroutine = StartCoroutine(CameraShakeEffectCoroutine());
    }

    private IEnumerator CameraShakeEffectCoroutine()
    {
        shakeCam.m_FrequencyGain = frequencyValue;
        shakeCam.m_AmplitudeGain = amplitudeValue;
        yield return wait;
        shakeCam.m_FrequencyGain = 0f;
        shakeCam.m_AmplitudeGain = 0f;
    }
    
}
