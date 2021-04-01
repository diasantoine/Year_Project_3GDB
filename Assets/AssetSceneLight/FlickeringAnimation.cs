using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringAnimation : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string SonLightEteind = "";

    [FMODUnity.EventRef]
    public string SonLightAllume = "";

    [FMODUnity.EventRef]
    public string SonLightAmbiance = "";
    FMOD.Studio.EventInstance sonLightAmbiance;

    public Animator Light;
    public float TimeMax;
    private float Chrono;

    // Start is called before the first frame update
    void Start()
    {
        sonLightAmbiance = FMODUnity.RuntimeManager.CreateInstance(SonLightAmbiance);
        sonLightAmbiance.start();
    }

    // Update is called once per frame
    void Update()
    {
        sonLightAmbiance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        if (Chrono >= TimeMax)
        {
            if (Random.value >= 0.25)
            {
                Light.SetTrigger("Flick");
            }
            Chrono = 0;
        }
        else
        {
            Chrono += Time.deltaTime;
        }
    }

    void Eteind()
    {
        Debug.Log("ETEIND");
        FMODUnity.RuntimeManager.PlayOneShot(SonLightEteind, transform.position);
        sonLightAmbiance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void Allume()
    {
        Debug.Log("ALLUME");
        FMODUnity.RuntimeManager.PlayOneShot(SonLightAllume, transform.position);
        sonLightAmbiance.start();
    }
}
