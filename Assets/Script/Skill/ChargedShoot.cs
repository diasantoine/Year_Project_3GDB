using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShoot : skill
{

    [Header("Propriety Of Skill")]
    [SerializeField] private GameObject preShoot;
    [SerializeField] private Transform canon;

    [SerializeField] private int chargeMax;
    private int charge;

    public override void UsingSkill()
    {
        if(detectDead.ressourceFloat >= canUseRessource)
        {
            chrono = 0;
            isCharging = true;
            theProjo = Instantiate(preShoot, canon.position, transform.rotation, canon).transform.GetChild(0).gameObject;
        }
        
    }


    public override void ChargingSkill(int WhichWeapon)
    {
        if (theProjo != null)
        {
            if (isCharging && charge <= chargeMax && detectDead.ressourceFloat > 0)
            {
                if(chrono >= freqCharge)
                {
                    charge++;
                    detectDead.ressourceFloat--;
                    chrono = 0;
                }
                else
                {
                    chrono += Time.deltaTime;
                }

                if (this.charge > 0)
                {
                    theProjo.GetComponent<LaserShoot>().IsCharging = true;
                    theProjo.GetComponent<LaserShoot>().charged = this.charge;
                }
            }
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if(charge > 0)
        {
            if(theProjo.GetComponent<LaserShoot>() != null)
            {
                theProjo.GetComponent<LaserShoot>().IsCharging = false;              
                //theProjo.GetComponent<LaserShoot>().charged = charge;
                theProjo.GetComponent<LaserShoot>().goGoGo = true;
                //theProjo.GetComponent<MeshRenderer>().enabled = true;
                //theProjo.GetComponent<Collider>().enabled = true;
            }

        }
        else
        {
            Destroy(theProjo);
            isCharging = false;
        }

        charge = 0;
        chrono = 0;
    }
}
