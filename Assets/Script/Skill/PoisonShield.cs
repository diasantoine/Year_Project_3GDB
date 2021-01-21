using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonShield : skill
{
    [SerializeField] private GameObject SphereCollider;

    private bool isActive;

    [FMODUnity.EventRef]
    public string SonPoisonShield = "";
    FMOD.Studio.EventInstance sonPoisonShield;

    [SerializeField] private GameObject poisonCloud;
    private GameObject cloudNow;

    // Start is called before the first frame update
    void Start()
    {
        sonPoisonShield = FMODUnity.RuntimeManager.CreateInstance(SonPoisonShield);
        

    }
    
    void Update()
    {
        if (detectDead.ressourceInt == 0)
        {
            sonPoisonShield.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            SphereCollider.SetActive(false);
            isActive = false;
            if (cloudNow != null)
            {
                cloudNow.GetComponent<ParticleSystem>().Stop();
                Destroy(cloudNow, 2);
            }
        }
    }

     public override void UsingSkill()
     {
        if(detectDead.ressourceInt > 0)
        {
            if (isActive)
            {
                isActive = false;
                SphereCollider.SetActive(false);
                sonPoisonShield.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                cloudNow.GetComponent<ParticleSystem>().Stop();
                Destroy(cloudNow, 2);

            }
            else
            {
                isActive = true;
                SphereCollider.SetActive(true);
                SphereCollider.GetComponent<PoisonCollider>().timer = 0;
                sonPoisonShield.start();
                cloudNow = Instantiate(poisonCloud, transform.position + new Vector3(0, 0.5f, 0), poisonCloud.transform.rotation);
            }
        }
         
    }
     

    // public override void ChargingSkill(int WhichWeapon)
    // {
    // }
    //
    // public override void EndUsing(Ray rayon)
    // {
    //     if (isCharging)
    //     {
    //         
    //     }
    // }
}
