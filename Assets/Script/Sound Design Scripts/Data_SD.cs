using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_SD : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string Ambiance = "";

    [FMODUnity.EventRef]
    public string footStep = "";

    [FMODUnity.EventRef]
    public string Ruant_FootStep = "";

    [FMODUnity.EventRef]
    public string Screamer_Explosion = "";

    [FMODUnity.EventRef]
    public string Screamer_FootStep = "";

    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Ambiance, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayFootStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot(footStep, transform.position);
        /// Debug.Log("test son footstep");
    }


    public void PlayFootStepRuant()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Ruant_FootStep, transform.position);
        /// Debug.Log("test son footstep");
    }

    public void PlayScreamerExplosion()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Screamer_Explosion, transform.position);
        /// Debug.Log("test son footstep");
    }

    public void PlayFootStepScreamer()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Screamer_FootStep, transform.position);
        /// Debug.Log("test son footstep");
    }
}
