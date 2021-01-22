﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootSkill : MonoBehaviour
{

    public skillSystem skillSystem;

    private Camera cam;
    
    [SerializeField] private detectDead detectD;

    [SerializeField] private UISkill UI;

    [FMODUnity.EventRef]
    public string Changement_Skill = "";

    // Start is called before the first frame update
    void Start()
    {
        skillSystem = GetComponent<skillSystem>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skillSystem.skills.Count > 0)
        {
            Ray rayon = cam.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(1))
            {
                skillSystem.UsingSkill();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                skillSystem.EndUsing(rayon);
            }
            else if (!skillSystem.skills[skillSystem.skillID].isCharging)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    FMODUnity.RuntimeManager.PlayOneShot(Changement_Skill, transform.position);
                    skillSystem.changeSKill();
                    UI.changeSKill(skillSystem.skillID);
                }
            }

            if (skillSystem.skills[skillSystem.skillID].chargedSkill)
            {
                skillSystem.ChargingSkill(skillSystem.skills[skillSystem.skillID].IdSkill);
            }
        }
    }
}
