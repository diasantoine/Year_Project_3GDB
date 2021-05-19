using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProtection : skill
{
    [SerializeField] private GameObject BulleProtectrice;
    [SerializeField] private GameObject Parent;
    // Start is called before the first frame update
    private float Tick = 0;
    [SerializeField] private float TickVar = 1;
    [SerializeField] private shootDead TirDisabel;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (isCharging)
        {
            if (Tick < TickVar)
            {
                Tick += Time.deltaTime;
            }
            else
            {
                if (detectDead.ressourceFloat <= 0)
                {
                    Tick = 0;
                    isCharging = false;
                }
                else
                {
                    Tick = 0;
                    base.ChargingSkill(3);
                }
            }
        }
        else if(Parent.GetComponent<The_Player_Script>().OnShieldProtection)
        {
            isCharging = false;
            Tick = 1;
            BulleProtectrice.SetActive(false);
            Parent.GetComponent<The_Player_Script>().OnShieldProtection = false;
        }
    }

    public override void UsingSkill()
    {
        if (isCharging)
        {
            isCharging = false;
            Tick = 1;
            BulleProtectrice.SetActive(false);
            Parent.GetComponent<The_Player_Script>().OnShieldProtection = false;
            Parent.GetComponent<The_Player_Script>().ListOfYourPlayer[
                Parent.GetComponent<The_Player_Script>().YourPlayerChoosed].ConteneurRigibody.mass = 1;
            TirDisabel.enabled = true;
        }
        else if(detectDead.ressourceFloat>0)
        {
            isCharging = true;
            BulleProtectrice.SetActive(true);
            Parent.GetComponent<The_Player_Script>().OnShieldProtection = true;
            Parent.GetComponent<The_Player_Script>().ListOfYourPlayer[
                    Parent.GetComponent<The_Player_Script>().YourPlayerChoosed].ConteneurRigibody.mass = 250;
            TirDisabel.enabled = false;
            theProjo = gameObject;
        }
    }
}
