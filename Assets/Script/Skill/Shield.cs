using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : skill
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

     public override void UsingSkill()
     {
         isCharging = true;
     }

    public override void ChargingSkill(int WhichWeapon)
    {
        if (isCharging)
        {
            
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if (isCharging)
        {
        }
    }
}
