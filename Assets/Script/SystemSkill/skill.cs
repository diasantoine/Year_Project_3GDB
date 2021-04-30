using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill : MonoBehaviour
{
    public detectDead ressource;
    public bool chargedSkill;
    public int IdSkill;

    [SerializeField] protected private float freqCharge;
    [SerializeField] private takeCadavre parentTakeCadavre;
    protected private float chrono;

    public bool isCharging;

    [HideInInspector] public GameObject theProjo;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UsingSkill()
    {

    }
    
    public virtual void ChargingSkill(int WhichWeapon)
    {
        if (detectDead.ressourceInt > 0)
        {
            if (chrono >= freqCharge)
            {
                    if (WhichWeapon == 0)
                    {
                        parentTakeCadavre.charge = true;
                        parentTakeCadavre.bombe = theProjo.transform;
                        parentTakeCadavre.SkillCharging();
                    }
                    else if (WhichWeapon == 1)
                    {
                        parentTakeCadavre.dash = true;
                        parentTakeCadavre.Dash = theProjo.transform;
                        parentTakeCadavre.SkillCharging();
                    }else if (WhichWeapon == 3)
                    {
                        parentTakeCadavre.ShieldProtection = true;
                        parentTakeCadavre.Protection = theProjo.transform;
                        parentTakeCadavre.SkillCharging();
                    }else if (WhichWeapon == 4)
                    {
                        parentTakeCadavre.jump = true;
                        parentTakeCadavre.Jump = theProjo.transform;
                        parentTakeCadavre.SkillCharging();
                    }
                    detectDead.ressourceInt--;
                    chrono = 0;
            }
            else
            {
                chrono += Time.deltaTime;
            }
                
                
                
                
                
                //takeCadavre TC = ressource.deadList[0].GetComponent<takeCadavre>();
            //     int TC = detectDead.ressourceInt;
            //     if (TC.isMunitions)
            //     {
            //         TC.isMunitions = false;
            //         if (WhichWeapon == 0)
            //         {
            //             TC.charge = true;
            //             TC.bombe = conteneur.transform;
            //         }
            //         else if (WhichWeapon == 1)
            //         {
            //             TC.gameObject.layer = 0;
            //             TC.GetComponent<Collider>().isTrigger = true;
            //             TC.dash = true;
            //             TC.Dash = conteneur.transform;
            //         }else if (WhichWeapon == 3)
            //         {
            //             TC.gameObject.layer = 0;
            //             TC.GetComponent<Collider>().isTrigger = true;
            //             TC.ShieldProtection = true;
            //             TC.Protection = conteneur.transform;
            //         }
            //         ressource.deadList.Remove(ressource.deadList[0]);
            //         chrono = 0;
            //     }
            //
            //     detectDead.ressourceInt--;
            // }
            // else
            // {
            //     chrono += Time.deltaTime;
            // }
        }
    }

    public virtual void EndUsing(Ray rayon)
    {

    }
}
