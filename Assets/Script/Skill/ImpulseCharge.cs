using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseCharge : skill
{

    [SerializeField] private GameObject preProjo;
    [SerializeField] private Transform canonCharge;

    
    private bool isCharging;


    // Start is called before the first frame update
    void Start()
    {
        chrono = freqCharge;
    }

    public override void UsingSkill()
    {
        isCharging = true;
        conteneur = Instantiate(preProjo, canonCharge.position, Quaternion.identity, transform.parent);
        conteneur.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override void ChargingSkill(int WhichWeapon)
    {
        if(conteneur != null)
        {
            if (isCharging && conteneur.GetComponent<TirCharge>().nCharge < conteneur.GetComponent<TirCharge>().nChargeMax)
            {
                base.ChargingSkill(WhichWeapon);

            }
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if(conteneur.GetComponent<TirCharge>().nCharge > 0)
        {
            isCharging = false;
            RaycastHit floorHit;

            if (Physics.Raycast(rayon, out floorHit, Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            {
                Vector3 playerToMouse = floorHit.point - canonCharge.position;
                conteneur.GetComponent<TirCharge>().tipar = true;
                conteneur.GetComponent<Rigidbody>().isKinematic = false;
                conteneur.transform.parent = null;
                conteneur.GetComponent<TirCharge>().Shoot(playerToMouse);

            }
        }
        else
        {
            Destroy(conteneur);
            isCharging = false;
        }
    }



}
