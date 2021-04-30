﻿using System.Collections;
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

    [SerializeField] private RectTransform placementA;
    [SerializeField] private RectTransform placementR;
    [SerializeField] private RectTransform placementE;

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

                    KeyDown(KeyCode.A, result.gameObject);
                    KeyDown(KeyCode.R, result.gameObject);
                    KeyDown(KeyCode.E, result.gameObject);
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
                case KeyCode.A:
                    system.skillA = UI.gameObject.GetComponent<UIgotSkill>().skillGot;

                    if(system.skillE == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skillE = null;
                        Destroy(placementE.GetChild(0).gameObject);

                    }
                    else if(system.skillR == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skillR = null;
                        Destroy(placementR.GetChild(0).gameObject);

                    }


                    if (placementA.childCount > 0)
                    {
                        Destroy(placementA.GetChild(0).gameObject);
                    }

                    FB = Instantiate(UI.gameObject.GetComponent<UIgotSkill>().UIFeedBack, placementA.position, Quaternion.identity, placementA);
                    FB.transform.localScale = new Vector3(scaleUI, scaleUI, scaleUI);
                    break;
                case KeyCode.R:
                    system.skillR = UI.gameObject.GetComponent<UIgotSkill>().skillGot;

                    if (system.skillE == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skillE = null;
                        Destroy(placementE.GetChild(0).gameObject);

                    }
                    else if (system.skillA == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skillA = null;
                        Destroy(placementA.GetChild(0).gameObject);

                    }


                    if (placementR.childCount > 0)
                    {
                        Destroy(placementR.GetChild(0).gameObject);
                    }

                    FB = Instantiate(UI.gameObject.GetComponent<UIgotSkill>().UIFeedBack, placementR.position, Quaternion.identity, placementR);
                    FB.transform.localScale = new Vector3(scaleUI, scaleUI, scaleUI);
                    break;
                case KeyCode.E:
                    system.skillE = UI.gameObject.GetComponent<UIgotSkill>().skillGot;

                    if (system.skillR == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skillR = null;
                        Destroy(placementR.GetChild(0).gameObject);

                    }
                    else if (system.skillA == UI.gameObject.GetComponent<UIgotSkill>().skillGot)
                    {
                        system.skillA = null;
                        Destroy(placementA.GetChild(0).gameObject);

                    }


                    if (placementE.childCount > 0)
                    {
                        Destroy(placementE.GetChild(0).gameObject);
                    }

                    FB = Instantiate(UI.gameObject.GetComponent<UIgotSkill>().UIFeedBack, placementE.position, Quaternion.identity, placementE);
                    FB.transform.localScale = new Vector3(scaleUI, scaleUI, scaleUI);
                    break;
            }
        }
    }
}