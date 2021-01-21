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
                if (detectDead.ressourceInt <= 0)
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
        else if(Parent.GetComponent<CharacterMovement>().OnShieldProtection)
        {
            isCharging = false;
            Tick = 1;
            BulleProtectrice.SetActive(false);
            Parent.GetComponent<CharacterMovement>().OnShieldProtection = false;
        }
    }

    public override void UsingSkill()
    {
        if (isCharging)
        {
            isCharging = false;
            Tick = 1;
            BulleProtectrice.SetActive(false);
            Parent.GetComponent<CharacterMovement>().OnShieldProtection = false;
            TirDisabel.enabled = true;
        }
        else if(detectDead.ressourceInt>0)
        {
            isCharging = true;
            BulleProtectrice.SetActive(true);
            Parent.GetComponent<CharacterMovement>().OnShieldProtection = true;
            TirDisabel.enabled = false;
            conteneur = gameObject;
        }
    }
}
