using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cam;

    private float shakeTimerTotal;
    private float shakeTimer;

    private float startingIntensity;

    private void Awake()
    {
        Instance = this;
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }

    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (!ShakingOrNot(intensity))
        {
            Debug.Log("dontshake");
            return;
        }
        perlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;

    }

    private bool ShakingOrNot(float newIntensity)
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        var nowIntensity = perlin.m_AmplitudeGain;

        if(newIntensity >= nowIntensity)
        {
            return true;
        }
        else
        {
            return false;
        }
        

    }


}
