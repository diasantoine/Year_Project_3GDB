using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    [SerializeField] private GameObject Parent;
    [SerializeField] private float disToGround;

    private RaycastHit hit;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if ((other.transform.CompareTag("sol") || other.transform.CompareTag("Ennemy"))  || other.transform.CompareTag("Mur") 
    //         && !Parent.GetComponent<The_Player_Script>().Grounded)
    //     {
    //         Parent.GetComponent<The_Player_Script>().Grounded = true;
    //     }
    // }
    //
    // private void OnTriggerStay(Collider other)
    // {
    //     if ((other.transform.CompareTag("sol") || other.transform.CompareTag("Ennemy"))  || other.transform.CompareTag("Mur") 
    //         && !Parent.GetComponent<The_Player_Script>().Grounded)
    //     {
    //         Parent.GetComponent<The_Player_Script>().Grounded = true;
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     if ((other.transform.CompareTag("sol") || other.transform.CompareTag("Ennemy"))  || other.transform.CompareTag("Mur") 
    //         && !Parent.GetComponent<The_Player_Script>().Grounded)
    //     {
    //         Parent.GetComponent<The_Player_Script>().Grounded = false;
    //     }
    // }

    private void Update()
    {
        //Ground(this.hit);
        this.Ground();
    }


    // public void Ground(RaycastHit hit)
    // {
    //     Debug.DrawRay(transform.position, -Vector3.up,Color.yellow, 5000f);
    //     if (Physics.Raycast(this.Parent.transform.position, -Vector3.up, out hit, disToGround, LayerMask.GetMask("Sol", "Wall")))
    //     {
    //         if (hit.collider.transform.CompareTag("sol") || hit.collider.transform.CompareTag("Ennemy")  || hit.collider.transform.CompareTag("Mur") && !Parent.GetComponent<The_Player_Script>().Grounded)
    //         {
    //             Parent.GetComponent<The_Player_Script>().Grounded = true;
    //         }
    //     }
    //     else
    //     {
    //         Parent.GetComponent<The_Player_Script>().Grounded = false;
    //     }
    // }
    private void Ground()
    {
        //Debug.DrawRay(transform.position, -Vector3.up,Color.yellow, 5000f);
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, disToGround, LayerMask.GetMask("Sol", "Wall")))
        {
            if (hit.collider.transform.CompareTag("sol") || hit.collider.transform.CompareTag("Ennemy")  || hit.collider.transform.CompareTag("Mur") && !this.Parent.GetComponent<The_Player_Script>().Grounded)
            {
                    this.Parent.GetComponent<The_Player_Script>().Grounded = true;
                    this.Parent.GetComponent<The_Player_Script>().particleJump.SetActive(false);
            }
        }
        else
        {
            this.Parent.GetComponent<The_Player_Script>().Grounded = false;
        }
    }

}
