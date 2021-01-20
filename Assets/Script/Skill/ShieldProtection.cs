using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProtection : skill
{
    [SerializeField] private GameObject BulleProtectrice;
    [SerializeField] private GameObject Parent;
    [SerializeField] private detectDead DetecList;
    // Start is called before the first frame update
    private float Tick = 0.5f;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (isCharging)
        {
            if (Tick >=0.5f)
            {
                Tick -= Time.deltaTime;
            }
            else
            {
                if (DetecList.deadList.Count == 0)
                {
                    Tick = 0.5f;
                    isCharging = false;
                }
                else
                {
                    Tick = 0.5f;
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
        }
        else
        {
            isCharging = true;
            BulleProtectrice.SetActive(true);
            Parent.GetComponent<CharacterMovement>().OnShieldProtection = true;
            conteneur = gameObject;
        }
    }
}
