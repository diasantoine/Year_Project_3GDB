using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class changeSkill : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string ChangeUI = "";

    [SerializeField] private GameObject RoundUI;
    [SerializeField] private skillSystem system;

    private GraphicRaycaster gra;
    private PointerEventData ped;

    [SerializeField] private EventSystem eve;

    private bool onUI;

    [SerializeField] private RectTransform placement1;
    [SerializeField] private RectTransform placement2;
    [SerializeField] private RectTransform placement3;

    [SerializeField] private float scaleUI;

    // Start is called before the first frame update
    void Start()
    {
        gra = GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(eve);
        onUI = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (onUI)
        {
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            gra.Raycast(ped, results);

            foreach(RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("UiSkill"))
                {
                    if (result.gameObject.GetComponent<Animator>() != null)
                    {
                        result.gameObject.GetComponent<Animator>().SetTrigger("Touched");
                    }

                    KeyDown(KeyCode.Mouse1, result.gameObject);
                    KeyDown(KeyCode.LeftShift, result.gameObject);
                    KeyDown(KeyCode.Space, result.gameObject);
                }
            }
        }


    }

    public void change()
    {
        if (!onUI)
        {
            onUI = true;
            RoundUI.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot(ChangeUI, "", 0);
            gameObject.GetComponent<Pause>().StopGame();
        }
        else
        {
            onUI = false;
            RoundUI.SetActive(false);
            gameObject.GetComponent<Pause>().StopGame();

        }
    }

    public void KeyDown(KeyCode bind, GameObject UI)
    {
        if (Input.GetKeyDown(bind))
        {
            GameObject FB;

            switch (bind)
            {
                case KeyCode.Mouse1:
                    system.skill1 = UI.gameObject.GetComponent<UIgotSkill>().skillGot;

                    if(system.skill2 == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skill2 = null;
                        Destroy(placement2.GetChild(0).gameObject);

                    }
                    else if(system.skill3 == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skill3 = null;
                        Destroy(placement3.GetChild(0).gameObject);

                    }


                    if (placement1.childCount > 0)
                    {
                        Destroy(placement1.GetChild(0).gameObject);
                    }

                    FB = Instantiate(UI.gameObject.GetComponent<UIgotSkill>().UIFeedBack, placement1.position, Quaternion.identity, placement1);
                    FB.GetComponent<OnUISkill>().skillGot = UI.gameObject.GetComponent<UIgotSkill>().skillGot;
                    FB.transform.localScale = new Vector3(scaleUI, scaleUI, scaleUI);
                    break;
                case KeyCode.Space:
                    system.skill2 = UI.gameObject.GetComponent<UIgotSkill>().skillGot;

                    if (system.skill3 == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skill3 = null;
                        Destroy(placement3.GetChild(0).gameObject);

                    }
                    else if (system.skill1 == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skill1 = null;
                        Destroy(placement1.GetChild(0).gameObject);

                    }


                    if (placement2.childCount > 0)
                    {
                        Destroy(placement2.GetChild(0).gameObject);
                    }

                    FB = Instantiate(UI.gameObject.GetComponent<UIgotSkill>().UIFeedBack, placement2.position, Quaternion.identity, placement2);
                    FB.GetComponent<OnUISkill>().skillGot = UI.gameObject.GetComponent<UIgotSkill>().skillGot;
                    FB.transform.localScale = new Vector3(scaleUI, scaleUI, scaleUI);
                    break;
                case KeyCode.LeftShift:
                    system.skill3 = UI.gameObject.GetComponent<UIgotSkill>().skillGot;

                    if (system.skill2 == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skill2 = null;
                        Destroy(placement2.GetChild(0).gameObject);

                    }
                    else if (system.skill1 == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skill1 = null;
                        Destroy(placement1.GetChild(0).gameObject);

                    }


                    if (placement3.childCount > 0)
                    {
                        Destroy(placement3.GetChild(0).gameObject);
                    }

                    FB = Instantiate(UI.gameObject.GetComponent<UIgotSkill>().UIFeedBack, placement3.position, Quaternion.identity, placement3);
                    FB.GetComponent<OnUISkill>().skillGot = UI.gameObject.GetComponent<UIgotSkill>().skillGot;
                    FB.transform.localScale = new Vector3(scaleUI, scaleUI, scaleUI);
                    break;
            }
        }
    }
}
