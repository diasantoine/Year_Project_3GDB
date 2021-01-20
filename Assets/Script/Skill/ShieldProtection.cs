using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProtection : skill
{
    [SerializeField] private GameObject BulleProtectrice;
    [SerializeField] private GameObject Parent;
    [SerializeField] private detectDead DetecList;
    // Start is called before the first frame update
    private float Tick = 1f;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (isCharging)
        {
            if (Tick >=1)
            {
                Tick -= Time.deltaTime;
            }
            else
            {
                if (DetecList.deadList.Count == 0)
                {
                    Tick = 1;
                    isCharging = false;
                }
                else
                {
                    Tick = 1;
                    base.ChargingSkill(3);
                }
            }
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
        }
    }
}
