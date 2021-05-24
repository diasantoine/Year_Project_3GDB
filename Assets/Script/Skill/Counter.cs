using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : skill
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string Counter_Use = "";

    [FMODUnity.EventRef]
    public string CantUse = "";

    [Header("VarCounter")]
    [SerializeField] private float DurationOfTheCounterStance;
    [SerializeField] private float DurationOfTheCounterWall;
    [SerializeField] private GameObject Wall;

    [Header("¨PlayerVar")]
    [SerializeField] private The_Player_Script PlayerScriptContainer;

    [Header("WhichCounter")] 
    [SerializeField] private bool StanceOrWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.StanceOrWall)
        {
            if (this.isCharging)
            {
                if (!this.PlayerScriptContainer.OnCounter)
                {
                    PlayerScriptContainer.OnCounter = true;
                    StartCoroutine(this.TimeOfTheCounterStanceStance());
                }
            }
            else if (this.PlayerScriptContainer.OnCounter)
            {
                PlayerScriptContainer.OnCounter = false;
            }
        }
        else
        {
            if (this.isCharging)
            {
                Debug.Log("a");
                if (!this.PlayerScriptContainer.OnCounter)
                {
                    PlayerScriptContainer.OnCounter = true;
                    this.Wall.SetActive(true);
                    StartCoroutine(this.TimeOfTheCounterStanceWall());
                }
            }
            else if (this.PlayerScriptContainer.OnCounter)
            {
                PlayerScriptContainer.OnCounter = false;
                this.Wall.SetActive(false);
            }
        }
    }

    IEnumerator TimeOfTheCounterStanceStance()
    {
        yield return new WaitForSeconds(this.DurationOfTheCounterStance);
        this.isCharging = false;
        PlayerScriptContainer.OnCounter = false;
    }
    
    IEnumerator TimeOfTheCounterStanceWall()
    {
        yield return new WaitForSeconds(this.DurationOfTheCounterWall);
        this.isCharging = false;
        PlayerScriptContainer.OnCounter = false;
        this.Wall.SetActive(false);
    }


    public override void UsingSkill()
    {
       
        if (detectDead.ressourceFloat >= canUseRessource && !this.PlayerScriptContainer.OnCounter && !this.isCharging)
        {
            FMODUnity.RuntimeManager.PlayOneShot(Counter_Use, "", 0, transform.position);
            this.isCharging = true;
            detectDead.ressourceFloat -= 10;
        }
        else if(detectDead.ressourceFloat < canUseRessource)
        {
            FMODUnity.RuntimeManager.PlayOneShot(CantUse, "", 0, transform.position);
        }
    }

    public override void ChargingSkill(int WhichWeapon)
    {
    }
}
