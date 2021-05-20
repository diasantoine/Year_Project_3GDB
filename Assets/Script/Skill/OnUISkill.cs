using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnUISkill : MonoBehaviour
{

    public skill skillGot;
    private float disableColor;

    private float lerp;
    private float colorPropor;

    // Update is called once per frame
    void Update()
    {
        lerp = 5 * Time.deltaTime;

        UISkillActivation();
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
