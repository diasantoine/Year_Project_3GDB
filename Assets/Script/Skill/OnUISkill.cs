using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnUISkill : MonoBehaviour
{

    public skill skillGot;
    private float disableColor;

    private float lerp;
    private float chrono = 0;
    
    
    private float colorPropor;

    [Header("ChargingEffect")]
    [SerializeField] private GameObject usingScreen;
    [SerializeField] private float ecranTime;

    // Update is called once per frame
    void Update()
    {
        lerp = 5 * Time.deltaTime;

        TheSkillIsUse();

        if (!skillGot.isCharging)
        {
            UISkillActivation();

        }
    }

    private void TheSkillIsUse()
    {
        if (skillGot.isCharging)
        {
            if(chrono >= ecranTime)
            {
                if (usingScreen.activeSelf)
                {
                    usingScreen.SetActive(false);
                    chrono = 0f;
                }
                else
                {
                    usingScreen.SetActive(true);
                    chrono = 0f;

                }
            }
            else
            {
                chrono += Time.deltaTime;
            }
            

            
        }
        else
        {
            usingScreen.SetActive(false);
            chrono = 0f;
        }
    }

    private void UISkillActivation()
    {
        if(detectDead.ressourceFloat >= skillGot.canUseRessource)
        {

            disableColor = 2;
            
        }
        else
        {
            disableColor = 0.5f;

        }

        colorPropor = Mathf.Clamp(Mathf.Lerp(colorPropor, colorPropor * disableColor, lerp), 0.5f, 1);
        this.GetComponent<Image>().color = new Vector4(colorPropor, colorPropor, colorPropor, 1);
    }
}
