﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    [SerializeField] private GameObject Parent;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.CompareTag("sol") || other.transform.CompareTag("Ennemy"))  || other.transform.CompareTag("Mur") 
            && !Parent.GetComponent<The_Player_Script>().Grounded)
        {
            Parent.GetComponent<The_Player_Script>().Grounded = true;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if ((other.transform.CompareTag("sol") || other.transform.CompareTag("Ennemy"))  || other.transform.CompareTag("Mur") 
            && !Parent.GetComponent<The_Player_Script>().Grounded)
        {
            Parent.GetComponent<The_Player_Script>().Grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.transform.CompareTag("sol") || other.transform.CompareTag("Ennemy"))  || other.transform.CompareTag("Mur") 
            && !Parent.GetComponent<The_Player_Script>().Grounded)
        {
            Parent.GetComponent<The_Player_Script>().Grounded = false;
        }
    }


}
