using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_SD_Screamer : MonoBehaviour
{

    [SerializeField] private GameObject particleBoom;
    [SerializeField] private GameObject particleDeath;


    [FMODUnity.EventRef]
    public string Screamer_Explosion = "";

    [FMODUnity.EventRef]
    public string Screamer_FootStep = "";



    public void PlayScreamerExplosion()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Screamer_Explosion, "", 0, transform.position);
        /// Debug.Log("test son footstep");
    }

    public void PlayFootStepScreamer()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Screamer_FootStep, "", 0, transform.position);
        /// Debug.Log("test son footstep");
    }

    public void BoomBoomDeath()
    {
        gameObject.SetActive(false);
        particleBoom.SetActive(true);
        particleBoom.GetComponent<ParticleSystem>().Play();
        gameObject.transform.parent.gameObject.GetComponent<ScreamerState>().Die();

    }

    public void Death()
    {
        gameObject.SetActive(false);
        GameObject ps = Instantiate(particleDeath, transform.position + -transform.forward * 2f + new Vector3(0, 2f, 0), particleDeath.transform.rotation);
        Destroy(ps, 1f);
        gameObject.transform.parent.gameObject.GetComponent<ScreamerState>().Die();
    }
}
