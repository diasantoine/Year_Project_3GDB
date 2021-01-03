using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootSkill : MonoBehaviour
{

    private skillSystem skillSystem;

    private Camera cam;
    
    [SerializeField] private detectDead detectD;

    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        skillSystem = GetComponent<skillSystem>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
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
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            skillSystem.changeSKill();
        }

        // if (Input.GetButton("Fire3") && detectD.deadList.Count >=3)
        // {
        //     skillSystem.ChargingSkill();
        // }
        // else if (Input.GetButtonUp("Fire3") && detectD.deadList.Count >=3)
        // {
        //     skillSystem.EndUsing(rayon);
        // }

        if (skillSystem.skills[skillSystem.skillID].chargedSkill)
        {
            skillSystem.ChargingSkill(skillSystem.skills[skillSystem.skillID].IdSkill);
        }
    }
}
