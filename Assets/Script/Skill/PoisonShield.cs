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

    // Start is called before the first frame update
    void Start()
    {
        sonPoisonShield = FMODUnity.RuntimeManager.CreateInstance(SonPoisonShield);
    }

     public override void UsingSkill()
     {
        if(detectDead.ressourceInt >= 0)
        {
            if (isActive)
            {
                isActive = false;
                SphereCollider.SetActive(false);
                Debug.Log("Stop son poison");
                sonPoisonShield.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            }
            else
            {
                isActive = true;
                SphereCollider.SetActive(true);
                SphereCollider.GetComponent<PoisonCollider>().timer = 0;
                Debug.Log("Start son poison");
                sonPoisonShield.start();
                


            }
        }
         
        //lancement la fonction
        // si fonction déjà en route tu stops
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
