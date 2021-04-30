using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootSkill : MonoBehaviour
{

    public skillSystem skillSystem;

    private Camera cam;
    private bool onChange;
    
    [SerializeField] private detectDead detectD;

    [SerializeField] private gotSkill UI;

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

            if (!onChange)
            {
                BangBangSkills();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                skillSystem.changeSkill();
                onChange = true;
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                skillSystem.changeSkill();
                onChange = false;

            }


        }
    }



    private void BangBangSkills()
    {
        if (skillSystem.skills.Count > 0)
        {
            if (skillSystem.skillA != null)
            {
                SkillUse(KeyCode.A, skillSystem.skillA);

            }

            if (skillSystem.skillE != null)
            {
                SkillUse(KeyCode.E, skillSystem.skillE);

            }

            if (skillSystem.skillR != null)
            {
                SkillUse(KeyCode.R, skillSystem.skillR);

            }
        }
    }

    private void SkillUse(KeyCode bind, skill thisSkill)
    {
        Ray rayon = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown(bind))
        {
            skillSystem.UsingSkill(thisSkill);
        }
        else if (Input.GetKeyUp(bind))
        {
            skillSystem.EndUsing(rayon, thisSkill);
        }

        if (thisSkill.isCharging)
        {
            skillSystem.ChargingSkill(thisSkill.IdSkill, thisSkill);
        }
    }
}
