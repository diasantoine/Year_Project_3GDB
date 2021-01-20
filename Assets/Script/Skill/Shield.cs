using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : skill
{
    [SerializeField] private GameObject SphereCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

     public override void UsingSkill()
     {
         if (isCharging)
         {
             isCharging = false;
         }
         else
         {
             isCharging = true;
             SphereCollider.SetActive(true);
             
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
