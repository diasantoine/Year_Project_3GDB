using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBasic : MonoBehaviour
{
    [SerializeField] private BasicAI ContainerBasicAI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player") && this.ContainerBasicAI.InPunch)
        {
            if (!other.transform.GetComponent<The_Player_Script>().JustHit)
            {
                this.ContainerBasicAI.PatateDansLeJoueur();
            }
        }
    }
}
