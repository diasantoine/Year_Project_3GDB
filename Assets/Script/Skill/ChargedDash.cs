using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedDash : skill
{
    // Start is called before the first frame update
    //[SerializeField] private detectDead detectD;
    [SerializeField] private Rigidbody ConteneurRigibody;
    [SerializeField] private float DashSpeed = 20;
    [SerializeField] public float ChargeMax = 7;
    [SerializeField] private GameObject Parent;
    [SerializeField] private GameObject Avatar;
    private float Compteur = 0;
    public int Charge = 0;
    private LineRenderer lineRenderer;
    private Vector3 HitPosition;
    RaycastHit floorHit;
    public bool isCharging;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UsingSkill()
    {
        conteneur = gameObject;
        isCharging = true;

    }

    public override void ChargingSkill(int WhichWeapon)
    {
        if (isCharging)
        {
            if (Charge<ChargeMax && ressource.deadList.Count>0)
            {
                base.ChargingSkill(WhichWeapon);
                Debug.Log("?");
            }
            if (!Parent.GetComponent<LineRenderer>())
            {
                lineRenderer = Parent.AddComponent<LineRenderer>();
            }
            else
            {
                Parent.GetComponent<LineRenderer>().enabled = true;
            }
            Ray MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(MousePosition, out RaycastHit Hit))
            {
                lineRenderer.SetPosition(0, transform.position);
                Vector3 HitPosition = Hit.point;
                HitPosition.x = transform.position.x + (Hit.point.x - transform.position.x)*(Charge/ChargeMax); 
                HitPosition.z = transform.position.z + (Hit.point.z - transform.position.z)*(Charge/ChargeMax);
                lineRenderer.SetPosition(1,new Vector3(HitPosition.x,transform.position.y,HitPosition.z));
                lineRenderer.startColor = Color.cyan;
            }
        }
        // if (Charge < ChargeMax && Charge < detectD.deadList.Count && Compteur <=0)
        // {
        //     Compteur = 1;
        //     //ChargementDash();
        //     //base.ChargingSkill(WhichWeapon);
        //     Debug.Log("?");
        //     Charge++;
        // }
        // else
        // {
        //     Debug.Log("??");
        //     Compteur -= Time.deltaTime;
        // }
       
    }

    public override void EndUsing(Ray rayon)
    {
        if (Physics.Raycast(rayon, out RaycastHit hit,Mathf.Infinity,LayerMask.GetMask("Sol")))
        {
            if (hit.transform.tag != "Collider")
            {
                //RemoveDeadList();
                //HitPosition = hit.point;
                HitPosition.x = transform.position.x + (hit.point.x - transform.position.x)*(Charge/ChargeMax); 
                HitPosition.z = transform.position.z + (hit.point.z - transform.position.z)*(Charge/ChargeMax);
                // HitPosition.x *= (Charge / ChargeMax);
                // HitPosition.z *= (Charge / ChargeMax);
                Vector3 playerToMouse = HitPosition - transform.parent.parent.position;
                playerToMouse.y = 0;
                playerToMouse = playerToMouse.normalized;
                //playerToMouse *= (Charge / ChargeMax);
                Parent.GetComponent<CharacterMovement>().OnDash = true;
                Parent.GetComponent<CharacterMovement>().HitPosition = HitPosition;
                Avatar.layer = 12;
                Parent.GetComponent<CapsuleCollider>().enabled = enabled;
                Parent.tag = "Player";
                ConteneurRigibody.useGravity = false;
                ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                //Vector3 Dir = (hit.transform.position - transform.parent.position).normalized;
                ConteneurRigibody.velocity = playerToMouse*DashSpeed;
                Debug.Log("    ?");
                // ConteneurRigibody.velocity *= (Charge / ChargeMax);
                //ConteneurRigibody.AddForce(playerToMouse*DashSpeed, ForceMode.Impulse);
            }
            Charge = 0;
            isCharging = false;
        }
    }
    
    void ChargementDash()
    {
        if(ressource.deadList.Count > 0)
        {
            takeCadavre ConteneurCadavre = ressource.deadList[Charge].GetComponent<takeCadavre>();
            if(ConteneurCadavre.isMunitions)
            {
                ConteneurCadavre.isMunitions = false;
                ConteneurCadavre.charge = true;
                //detectD.deadList.Remove(detectD.deadList[0]);
                Destroy(ConteneurCadavre.transform.gameObject);
            }
        }
    }

    void RemoveDeadList()
    {
        for (int i = 0; i < Charge; i++)
        {
            ressource.deadList.Remove(ressource.deadList[0]);
        }
    }

}
