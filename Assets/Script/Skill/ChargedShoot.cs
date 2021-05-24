using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShoot : skill
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string CantUse = "";

    [Header("Propriety Of Skill")]
    [SerializeField] private GameObject preShoot;
    [SerializeField] private Transform canon;

    [SerializeField] private int chargeMax;
    private int charge;

    [Header("ChargementEffect")]
    [FMODUnity.EventRef]
    public string Charged_Charge = "";
    FMOD.Studio.EventInstance _Charged_Charge;

    [SerializeField] private GameObject chargingParticle;
    private GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        _Charged_Charge = FMODUnity.RuntimeManager.CreateInstance(Charged_Charge);
    }

    public override void UsingSkill()
    {
        if(detectDead.ressourceFloat >= canUseRessource)
        {
            chrono = 0;
            isCharging = true;
            _Charged_Charge.start();
            particle = Instantiate(chargingParticle, canon.position, Quaternion.identity, canon);
            theProjo = Instantiate(preShoot, canon.position, transform.rotation, canon).transform.GetChild(0).gameObject;
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(CantUse, "", 0, transform.position);
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
                CameraShake.Instance.Shake(2, 0.5f);
                theProjo.GetComponent<LaserShoot>().IsCharging = false;              
                //theProjo.GetComponent<LaserShoot>().charged = charge;
                theProjo.GetComponent<LaserShoot>().goGoGo = true;
                isCharging = false;
                //theProjo.GetComponent<MeshRenderer>().enabled = true;
                //theProjo.GetComponent<Collider>().enabled = true;
            }

        }
        else
        {
            Destroy(theProjo);
            isCharging = false;
        }

        _Charged_Charge.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Destroy(particle);
        charge = 0;
        chrono = 0;
        theProjo = null;

    }
}
