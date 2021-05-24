using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_SD_Basic : MonoBehaviour
{

    [SerializeField] private GameObject particleDeath;

    [FMODUnity.EventRef]
    public string Basic_FootStep = "";


    public void PlayFootStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Basic_FootStep, "TypeOfFootstep", 0, transform.position);
    }

    public void DeathVFX()
    {
        gameObject.SetActive(false);
        GameObject ps = Instantiate(particleDeath, transform.position + -transform.forward * 2f + new Vector3(0, 2f, 0), particleDeath.transform.rotation);
        Destroy(ps, 1f);
    }
}
