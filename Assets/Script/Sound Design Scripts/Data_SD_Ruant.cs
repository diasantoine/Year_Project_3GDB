using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_SD_Ruant : MonoBehaviour
{
    [SerializeField] private RuantAI AI;

    [FMODUnity.EventRef]
    public string Ruant_FootStep = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootStepRuant()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Ruant_FootStep, "", 0, transform.position);
        /// Debug.Log("test son footstep");
    }

    public void Stomp()
    {
        AI.StompGround();
        AI.SwitchState(RuantAI.State.IDLE);
    }
}
