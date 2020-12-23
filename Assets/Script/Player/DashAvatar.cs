using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAvatar : MonoBehaviour
{
    [SerializeField] private detectDead detectD;
    [SerializeField] private Rigidbody ConteneurRigibody;
    [SerializeField] private float DashSpeed = 20;
    [SerializeField] private float ChargeMax = 7;
    [SerializeField] private GameObject Parent;
    private float Charge;
    private LineRenderer lineRenderer;
    private Vector3 HitPosition;
    RaycastHit floorHit;
    
    void Update()
    {

        if (Input.GetButton("Fire3") && !transform.parent.parent.GetComponent<CharacterMovement>().JustHit &&
            detectD.deadList.Count>=3)
        {
            if (Charge < ChargeMax && Charge < detectD.deadList.Count)
            {
                Charge++;
            }
            else
            {
                Debug.Log("MaxCharge");
            }
            if (!Parent.GetComponent<LineRenderer>())
            {
              lineRenderer = Parent.AddComponent<LineRenderer>();
            }
            Ray MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(MousePosition, out RaycastHit Hit))
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1,new Vector3(Hit.point.x,transform.position.y,Hit.point.z));
                lineRenderer.startColor = Color.cyan;
                Debug.Log(Hit.point);
            }
        }

        if (Input.GetButtonUp("Fire3") && !transform.parent.parent.GetComponent<CharacterMovement>().JustHit &&
            detectD.deadList.Count>=3)
            {
                ChargementDash();
                Ray MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(MousePosition, out RaycastHit hit,Mathf.Infinity,LayerMask.GetMask("Sol")))
                {
                    if (hit.transform.tag != "Collider")
                    {
                        //HitPosition = hit.point;
                        HitPosition.x = transform.position.x + (hit.point.x - transform.position.x)*(Charge/ChargeMax); 
                        HitPosition.z = transform.position.z + (hit.point.z - transform.position.z)*(Charge/ChargeMax);
                        // HitPosition.x *= (Charge / ChargeMax);
                        // HitPosition.z *= (Charge / ChargeMax);
                        Vector3 playerToMouse = HitPosition - transform.parent.parent.position;
                        playerToMouse.y = 0;
                        playerToMouse = playerToMouse.normalized;
                        //playerToMouse *= (Charge / ChargeMax);
                        transform.parent.parent.GetComponent<CharacterMovement>().OnDash = true;
                        transform.gameObject.layer = 12;
                        transform.parent.parent.GetComponent<CapsuleCollider>().enabled = enabled;
                        transform.parent.parent.tag = "Player";
                        ConteneurRigibody.useGravity = false;
                        ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                        //Vector3 Dir = (hit.transform.position - transform.parent.position).normalized;
                        ConteneurRigibody.velocity = playerToMouse*DashSpeed;
                        // ConteneurRigibody.velocity *= (Charge / ChargeMax);
                        //ConteneurRigibody.AddForce(playerToMouse*DashSpeed, ForceMode.Impulse);
                    }
                    Charge = 0;
                }

                /*if(detectD.deadList.Count > 0)
                {
                
                    for (int i = 0; i < 5; i++)
                    {
                        takeCadavre TC = detectD.deadList[i].GetComponent<takeCadavre>();
                        if(TC.isMunitions)
                        {
                            TC.isMunitions = false;
                        }
                        detectD.deadList.Remove(detectD.deadList[i]);
                        Destroy(TC.transform.gameObject);
                    }
                }*/
            }

        if (transform.parent.parent.GetComponent<CharacterMovement>().OnDash)
        {
            if (transform.parent.parent.GetComponent<Rigidbody>().velocity.magnitude < 3 )
            {
                transform.parent.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ConteneurRigibody.constraints = RigidbodyConstraints.FreezeAll;
                transform.parent.parent.GetComponent<CharacterMovement>().JustFinishedDash = true;
                transform.parent.parent.GetComponent<CharacterMovement>().OnDash = false;
                transform.parent.parent.GetComponent<CapsuleCollider>().enabled = !enabled;
                transform.gameObject.layer = 9;
                transform.parent.parent.tag = "Untagged";
                ConteneurRigibody.useGravity = true;
                ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
            if (Vector3.Distance(HitPosition,transform.position)<2)
            {
                transform.parent.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ConteneurRigibody.constraints = RigidbodyConstraints.FreezeAll;
                transform.parent.parent.GetComponent<CharacterMovement>().JustFinishedDash = true;
                transform.parent.parent.GetComponent<CharacterMovement>().OnDash = false;
                transform.parent.parent.GetComponent<CapsuleCollider>().enabled = !enabled;
                transform.gameObject.layer = 9;
                transform.parent.parent.tag = "Untagged";
                ConteneurRigibody.useGravity = true;
                ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
        }
    }
    
    void ChargementDash()
    {

        if(detectD.deadList.Count > 0)
        {
            for (int i = 0; i < Charge; i++)
            {
                
                takeCadavre ConteneurCadavre = detectD.deadList[0].GetComponent<takeCadavre>();

                if(ConteneurCadavre.isMunitions)
                {
                    ConteneurCadavre.isMunitions = false;
                    ConteneurCadavre.charge = true;
                    detectD.deadList.Remove(detectD.deadList[0]);
                    Destroy(ConteneurCadavre.transform.gameObject);
                }

            }
        }

    }
}
