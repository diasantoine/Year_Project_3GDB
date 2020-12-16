using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillSystem : MonoBehaviour
{

    public List<skill> skills;
    public int skillID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UsingSkill()
    {
        skills[skillID].UsingSkill();
    }

    public void changeSKill()
    {
        skillID++;
        skillID %= skills.Count;
    }
}
