using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_SD : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string footStep = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayFootStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot(footStep, transform.position);
        Debug.Log("test son footstep");
    }
}
