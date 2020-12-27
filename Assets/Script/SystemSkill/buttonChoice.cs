using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonChoice : MonoBehaviour
{

    [SerializeField] private skill buttonSkill;
    [SerializeField] private bool skillOrUpgrade;

    private choiceSkill CS;

    // Start is called before the first frame update
    void Start()
    {
        CS = GameObject.Find("SystemChoice").GetComponent<choiceSkill>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Choice()
    {
        Debug.Log("SkillChoisis");

        for (int i = 0; i < CS.buttonList.Count; i++)
        {
            Destroy(CS.buttonList[i]);
            
        }

        CS.buttonList.Clear();

    }
}
