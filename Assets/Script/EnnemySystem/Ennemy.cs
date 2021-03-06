﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{
    [Header("Function")]
    [SerializeField] private float fieldOfView;
    [SerializeField] private float distanceOfView;
    [SerializeField] private float disToGround;

    [Header("Essential")]
    [SerializeField] private protected Rigidbody RB;
    [SerializeField] private protected Animator AnimatorConteneur;
    public NavMeshAgent agent;
    public Transform player;

    [Header("System")]
    public int DmgArmorHeat;
    public State health;


    private protected bool JustHit;
    private protected bool Grounded;
    private protected bool SeeThePlayer;

    public void VisionCone(Transform player)
    {
        var rayDirection = player.transform.position - transform.position;
        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfView
            && Vector3.Distance(transform.position, player.transform.position) < distanceOfView 
            && player.GetComponent<The_Player_Script>().Grounded)
        {
            Debug.Log(player.transform.position);
            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position + new Vector3(0,2,0), rayDirection, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Player", "Wall")))
            {
                Debug.DrawRay(transform.position, rayDirection, Color.blue);
                if (hit.collider.CompareTag("Mur"))
                {
                    SeeThePlayer = false;

                }
                else if (hit.collider.CompareTag("Player"))
                {
                    SeeThePlayer = true;
                    Debug.DrawRay(new Vector3(transform.position.x, 1, transform.position.z), player.position - transform.position, Color.blue);
                }
            }
        }
        else
        {
            SeeThePlayer = false;
            //Debug.Log(SeeThePlayer);
        }
    }

    public void Ground(RaycastHit hit)
    {
        Debug.DrawRay(transform.position, -Vector3.up,Color.yellow);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, disToGround, LayerMask.GetMask("Sol", "Wall")))
        {
            if (hit.collider.CompareTag("sol") || hit.collider.CompareTag("Mur"))
            {
                Grounded = true;
                //agent.enabled = true;
                RB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                //Debug.Log(Grounded);
            }
        }
        else
        {
            Grounded = false;
            agent.enabled = false;
            RB.constraints = RigidbodyConstraints.None;
            //Debug.Log(Grounded);

        }
    }
}
