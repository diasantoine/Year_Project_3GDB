using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamSceneLight : MonoBehaviour
{
    public GameObject Cam1;
    public GameObject Cam2;
    public GameObject Cam3;
    public GameObject Cam4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Cam1.SetActive(true);
            Cam2.SetActive(false);
            Cam3.SetActive(false);
            Cam4.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            Cam3.SetActive(false);
            Cam4.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cam1.SetActive(false);
            Cam2.SetActive(false);
            Cam3.SetActive(true);
            Cam4.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Cam1.SetActive(false);
            Cam2.SetActive(false);
            Cam3.SetActive(false);
            Cam4.SetActive(true);
        }
    }
}
