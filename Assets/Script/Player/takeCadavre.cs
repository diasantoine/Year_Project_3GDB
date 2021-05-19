using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCadavre : MonoBehaviour
{

    public bool gotcha;

    [HideInInspector] public bool isMunitions;
    [HideInInspector] public bool charge;
    [HideInInspector] public bool dash;
    [HideInInspector] public bool ShieldProtection;
    [HideInInspector] public bool jump;


    public Transform player;
    public Transform Dash;
    public Transform bombe;
    public Transform Protection;
    public Transform Jump;


    [SerializeField] private float threshold;
    [SerializeField] private float vitesse;
    [SerializeField] private float radiusGave;
    [SerializeField] private int DmgShield = 1;



    public detectDead deadD;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //         if (gotcha)
//         {
//             if(player != null)
//             {
//                 Vector3 direction = player.position - transform.position;
//
//                 if (direction.magnitude < threshold)
//                 {
//                     isMunitions = true;
//                     gotcha = false;
//                     gameObject.transform.parent = player.transform;
//                     gameObject.layer = 10;
//                     GetComponent<MeshRenderer>().enabled = false;
//                     GetComponent<Collider>().isTrigger = true;
//                 }
//
//                 direction = direction.normalized;
//
//                 transform.position += direction * vitesse * Time.deltaTime;
//             }
//         }
//
//         if (player != null)
//         {
//             if (player.parent.GetComponent<CharacterMovement>().OnDash)
//             {
//                 GetComponent<Collider>().enabled = !enabled;
//             }
//             else if(GetComponent<Collider>().enabled == !enabled)
//             {
//                 GetComponent<Collider>().enabled = enabled;
//             }
//         }
//         if (isMunitions)
//         {
//             if (!deadD.deadList.Contains(this.gameObject))
//             {
//                 deadD.deadList.Add(this.gameObject);
//             }
//
//             transform.position = player.position;
//
//             /*Vector3 Position = 2 * Vector3.Normalize(transform.position - player.position) + player.position;
//             transform.position = Position;
//             gameObject.transform.RotateAround(player.position, Vector3.up, 280f * Time.deltaTime);
//             gameObject.transform.LookAt(player);*/
//         }
//
//         if (charge)
//         {
//             if (bombe != null)
//             {
//                 if (!bombe.GetComponent<TirCharge>().tipar)
//                 {
//                     //Vector3 direction = bombe.position - transform.position;
//
//                     //direction = direction.normalized;
//
//                     //transform.position += direction * vitesse * Time.deltaTime;
//
//                     bombe.GetComponent<TirCharge>().nCharge++;
//                     bombe.transform.localScale = bombe.transform.localScale + new Vector3(radiusGave, radiusGave, radiusGave);
//                     Destroy(gameObject);
//                     deadD.deadList.Remove(gameObject);
//                 }
//                 else
//                 {
//                     bombe = null;
//                 }
// ;
//             }
//             else
//             {
//                 gotcha = true;
//                 charge = false;
//                 gameObject.layer = 8;
//
//             } 
//         }else if (dash)
//         {
//             if (Dash != null)
//             {
//                 if (Dash.GetComponent<ChargedDash>().isCharging)
//                 {
//                     //Vector3 direction = Dash.position - transform.position;
//
//                     //direction = direction.normalized;
//
//                     //transform.position += direction * vitesse * Time.deltaTime;
//                     Dash.GetComponent<ChargedDash>().Charge++;
//                     Destroy(gameObject);
//                     deadD.deadList.Remove(gameObject);
//                     if (Dash.GetComponent<ChargedDash>().Charge >= Dash.GetComponent<ChargedDash>().ChargeMax)
//                     {
//                         gotcha = true;
//                         dash = false;
//                         gameObject.layer = 8;
//                     }
//                 }
//                 else
//                 {
//                     Dash = null;
//                 }
//                 ;
//             }
//             else
//             {
//                 gotcha = true;
//                 dash = false;
//                 gameObject.layer = 8;
//             } 
//         }else if (ShieldProtection)
//         {
//             if (Protection != null)
//             {
//                 if (Protection.GetComponent<ShieldProtection>().isCharging)
//                 {
//                     //Vector3 direction = Dash.position - transform.position;
//
//                     //direction = direction.normalized;
//
//                     //transform.position += direction * vitesse * Time.deltaTime;
//                     Destroy(gameObject);
//                     deadD.deadList.Remove(gameObject);
//                     if (deadD.deadList.Count == 0)
//                     {
//                         gotcha = true;
//                         ShieldProtection = false;
//                         gameObject.layer = 8;
//                     }
//                 }
//                 else
//                 {
//                     Protection = null;
//                 }
//                 ;
//             }
//             else
//             {
//                 gotcha = true;
//                 ShieldProtection = false;
//                 gameObject.layer = 8;
//             } 
//         }
    }

    public void SkillCharging()
    {
        if (charge)
        {
            if (bombe != null)
            {
                if (!bombe.GetComponent<TirCharge>().tipar)
                {
                    bombe.GetComponent<TirCharge>().nCharge++;
                    bombe.transform.localScale =
                    bombe.transform.localScale + new Vector3(radiusGave, radiusGave, radiusGave);
                }
                else
                {
                    bombe = null;
                }
            }
            else
            {
                charge = false;
            }
        }
        else if (dash)
        {
            if (Dash != null)
            {
                if (Dash.GetComponent<ChargedDash>().isCharging)
                {
                    Dash.GetComponent<ChargedDash>().Charge++;
                    if (Dash.GetComponent<ChargedDash>().Charge >= Dash.GetComponent<ChargedDash>().ChargeMax)
                    {
                        dash = false;
                    }
                }
                else
                {
                    Dash = null;
                }
            }
            else
            {
                dash = false;
            }
        }
        else if (ShieldProtection)
        {
            if (Protection != null)
            {
                if (Protection.GetComponent<ShieldProtection>().isCharging)
                {
                    if (detectDead.ressourceFloat == 0)
                    {
                        ShieldProtection = false;
                    }
                }
                else
                {
                    Protection = null;
                }
            }
            else
            {
                ShieldProtection = false;
            }
        }else if (this.jump)
        {
            if (this.Jump != null)
            {
                if (this.Jump.GetComponent<JumpCharged>().isCharging)
                {
                    this.Jump.GetComponent<JumpCharged>().Charge++;
                    if (this.Jump.GetComponent<JumpCharged>().Charge >= this.Jump.GetComponent<JumpCharged>().ChargeMax)
                    {
                        this.jump = false;
                    }
                }
                else
                {
                    this.Jump = null;
                }
            }
            else
            {
                this.jump = false;
            }
        }
    }
}
