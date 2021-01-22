using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkill : MonoBehaviour
{
    [SerializeField] private float TimeFeedback = 1;
    [SerializeField] private skillSystem SkillConteneur;
    [SerializeField] private List<Sprite> ListSprite = new List<Sprite>();
    private float Compteur = 0;
    private int ID = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ID >= SkillConteneur.skills.Count)
            {
                ID = 0;
            }
            else
            {
                ID = SkillConteneur.skillID;
            }
            switch (SkillConteneur.skills[ID].transform.name)
            {
                case "ImpulseCharge":
                    GetComponent<Image>().sprite = ListSprite[0];
                    break;
                case "DashCharge":
                    GetComponent<Image>().sprite = ListSprite[1];
                    break;
                case "Shield":
                    GetComponent<Image>().sprite = ListSprite[2];
                    break;
                case "ShieldProtection":
                    GetComponent<Image>().sprite = ListSprite[3];
                    break;
                default:
                    Debug.Log("bug");
                    break;
            }
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().color = new Color(1,1,1,1);
            // GetComponent<Image>().sprite = ;
            // GetComponent<Image>().enabled = true;
            Compteur = 0;
        }*/

        if (Compteur < TimeFeedback)
        {
            Compteur += Time.deltaTime;
            GetComponent<Image>().color -= new Color(0,0,0,Compteur*0.008f);
        }
        else if(GetComponent<Image>().enabled)
        {
            GetComponent<Image>().enabled = false;
        }

    }

    public void changeSKill(int IDn)
    {

        switch (SkillConteneur.skills[IDn].transform.name)
        {
            case "ImpulseCharge":
                GetComponent<Image>().sprite = ListSprite[0];
                break;
            case "DashCharge":
                GetComponent<Image>().sprite = ListSprite[1];
                break;
            case "Shield":
                GetComponent<Image>().sprite = ListSprite[2];
                break;
            case "ShieldProtection":
                GetComponent<Image>().sprite = ListSprite[3];
                break;
            default:
                Debug.Log("bug");
                break;
        }
        GetComponent<Image>().enabled = true;
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
        // GetComponent<Image>().sprite = ;
        // GetComponent<Image>().enabled = true;
        Compteur = 0;
    }
}
