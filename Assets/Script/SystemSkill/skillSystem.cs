using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillSystem : MonoBehaviour
{

    public List<skill> skills;
    //public int skillID;

    [SerializeField] private changeSkill changing;

    [HideInInspector] public skill skillA;
    [HideInInspector] public skill skillR;
    [HideInInspector] public skill skillE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void UsingSkill(skill thisSkill)
    {
        thisSkill.UsingSkill();
    }

    public void EndUsing(Ray rayon, skill thiSkill)
    {
        thiSkill.EndUsing(rayon);
    }

    public void ChargingSkill(int WhichWeapon, skill thisSkill)
    {
        thisSkill.ChargingSkill(WhichWeapon);
    }

    public void changeSkill()
    {
        //skillID++;
        //skillID %= skills.Count;

        changing.change();
    }

    public void AddNewSkill(skill Skill)
    {
        skills.Add(Skill);
    }

    /*public void UsingSkill()
    {
        skills[skillID].UsingSkill();
    }

    public void EndUsing(Ray rayon)
    {
        skills[skillID].EndUsing(rayon);
    }

    public void ChargingSkill(int WhichWeapon)
    {
        skills[skillID].ChargingSkill(WhichWeapon);
    }

    public void AddNewSkill(skill Skill)
    {
        skills.Add(Skill);
        changeSKill();
    }*/
}
