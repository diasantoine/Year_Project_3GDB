using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseCharge : skill
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string CantUse = "";

    [Header("Propriety Of Skill")]
    [SerializeField] private GameObject preProjo;
    [SerializeField] private Transform canonCharge;

    [FMODUnity.EventRef]
    public string TirCharge_Charge = "";
    FMOD.Studio.EventInstance _TirCharge_Charge;

    [FMODUnity.EventRef]
    public string TirCharge_Tir = "";

    // Start is called before the first frame update
    void Start()
    {
        _TirCharge_Charge = FMODUnity.RuntimeManager.CreateInstance(TirCharge_Charge);
    }

    public override void UsingSkill()
    {
        if(detectDead.ressourceFloat >= canUseRessource)
        {
            isCharging = true;
            _TirCharge_Charge.start();
            theProjo = Instantiate(preProjo, canonCharge.position, Quaternion.identity, transform.parent);
            theProjo.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(CantUse, "", 0, transform.position);
        }
    }

    public override void ChargingSkill(int WhichWeapon)
    {
        if(theProjo != null)
        {
            if (isCharging && theProjo.GetComponent<TirCharge>().nCharge < theProjo.GetComponent<TirCharge>().nChargeMax)
            {
                if (theProjo != null)
                {
                    if (!theProjo.GetComponent<TirCharge>().tipar)
                    {
                        if(chrono >= freqCharge)
                        {
                            theProjo.GetComponent<TirCharge>().nCharge++;
                            theProjo.transform.localScale =
                            theProjo.transform.localScale + new Vector3(0.25f, 0.25f, 0.25f);
                            detectDead.ressourceFloat--;
                            chrono = 0;
                        }
                        else
                        {
                            chrono += Time.deltaTime;
                        }
                    }
                }
            }
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if(theProjo.GetComponent<TirCharge>().nCharge > 0)
        {
            isCharging = false;
            RaycastHit floorHit;

            if (Physics.Raycast(rayon, out floorHit, Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            {
                Vector3 playerToMouse = floorHit.point - canonCharge.position;
                theProjo.GetComponent<TirCharge>().tipar = true;
                theProjo.GetComponent<Rigidbody>().isKinematic = false;
                theProjo.transform.parent = null;
                theProjo.GetComponent<TirCharge>().Shoot(playerToMouse);
                FMODUnity.RuntimeManager.PlayOneShot(TirCharge_Tir, "", 0, transform.position);
            }
        }
        else
        {
            Destroy(theProjo);
            isCharging = false;
        }
        _TirCharge_Charge.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        theProjo = null;
    }
}
