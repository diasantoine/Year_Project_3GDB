using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonChoice : MonoBehaviour
{

    [SerializeField] private skill buttonSkill;
    [SerializeField] private bool skillOrUpgrade;

    private choiceSkill CS;
    public skillSystem Skill;
    public string WhichSkill;

    // Start is called before the first frame update
    void Start()
    {
        CS = GameObject.Find("SystemChoice").GetComponent<choiceSkill>();
        SetSkill();
    }

    private void SetSkill()
    {
        switch (WhichSkill)
        {
            case "DashCharge":
                buttonSkill = GameObject.Find(WhichSkill).GetComponent<ChargedDash>();
                break;
            case "ImpulseCharge":
                buttonSkill = GameObject.Find(WhichSkill).GetComponent<ImpulseCharge>();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Choice()
    {
        Debug.Log("SkillChoisis");
        if (skillOrUpgrade)
        {
            buttonSkill.enabled = true;
            Skill.AddNewSkill(buttonSkill);
        }
        for (int i = 0; i < CS.buttonList.Count; i++)
        {
            Destroy(CS.buttonList[i]);
            
        }
        
        CS.buttonList.Clear();
    }
}
