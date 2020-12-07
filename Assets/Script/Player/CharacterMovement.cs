﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public CharacterController CC;

    //[SerializeField] private Animator anim;

    private bool isWalking;

    public float vitesse = 6f;

    public Transform cam;

    private Rigidbody ConteneurRigibody;

    [SerializeField] private bool Grounded;
    public bool OnDash = false;
    public bool JustFinishedDash = false;
    public bool JustHit = false;
    private float Compteur = 0;
    private float Compteur1 = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        ConteneurRigibody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (JustFinishedDash)
        {
            if (Compteur<=0.4f)
            {
                Compteur += Time.deltaTime;
            }
            else
            {
                Compteur = 0;
                JustFinishedDash = false;
            }
        }

        if (JustHit)
        {
            if (Compteur1<=0.1f)
            {
                Compteur1 += Time.deltaTime;
            }
            else
            {
                if (Compteur1 != 0)
                {
                    Compteur1 = 0;
                }
                if (ConteneurRigibody.velocity.magnitude<=1)
                {
                    Debug.Log("bjr");
                    JustHit = false;
                }
                else
                {
                    ConteneurRigibody.velocity *=0.5f;
                }
            }


        }
        if (transform.position.y<=-6)
        {
            transform.position = new Vector3(30, 1.25f, -15.7f);
            if (OnDash)
            {
                OnDash = false;
                GetComponent<CapsuleCollider>().enabled = !enabled;
                transform.GetChild(1).GetChild(0).gameObject.layer = 9;
                transform.tag = "Untagged";
                ConteneurRigibody.useGravity = true;
                ConteneurRigibody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
            }
            ConteneurRigibody.velocity = Vector3.zero;
        }
        
        if ((Input.GetButton("Vertical")|| Input.GetButton("Horizontal")) && Grounded && !OnDash)
        { 
            Vector3 Vector3_Deplacement_Player =  new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
            //Vector3_Deplacement_Player = transform.TransformDirection(Vector3_Deplacement_Player);
            ConteneurRigibody.velocity = Vector3_Deplacement_Player * vitesse;
            //RigibodyAvatar.AddForce(Vector3_Deplacement_Player * Speed_Player);
        }

        if (!Grounded && !OnDash)
        {
            ConteneurRigibody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        }
        else if(ConteneurRigibody.constraints != (RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation))
        {
            ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
        /*float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            isWalking = true;
            CC.Move(moveDir.normalized * vitesse * Time.deltaTime);
        }
        else
        {
            isWalking = false;
        }*/
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("sol") && !Grounded)
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("sol") && Grounded)
        {
            Grounded = false;
        }
    }
}
