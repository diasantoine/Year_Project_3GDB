using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonShield : skill
{
    [SerializeField] private GameObject SphereCollider;

    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

     public override void UsingSkill()
     {
        if(detectDead.ressourceInt >= 0)
        {
            if (isActive)
            {
                isActive = false;
                SphereCollider.SetActive(false);

            }
            else
            {
                isActive = true;
                SphereCollider.SetActive(true);
                SphereCollider.GetComponent<PoisonCollider>().timer = 0;


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
