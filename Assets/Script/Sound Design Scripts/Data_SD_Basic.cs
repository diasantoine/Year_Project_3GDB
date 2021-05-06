using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_SD_Basic : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Basic_FootStep = "";


    public void PlayFootStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Basic_FootStep, "TypeOfFootstep", 0, transform.position);
    }
}
