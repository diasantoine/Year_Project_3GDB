using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedDash : skill
{
    // Start is called before the first frame update
    //[SerializeField] private detectDead detectD;

    [SerializeField] private Rigidbody ConteneurRigibody;
    [SerializeField] public float DashSpeed = 20;
    [SerializeField] public float ChargeMax = 7;
    [SerializeField] private GameObject Parent;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private int PorteMaximale;
    public int Charge = 0;
    private LineRenderer lineRenderer;
    private Vector3 HitPosition;
    private Vector3 LastPosition;
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
        if (!Parent.GetComponent<CharacterMovement>().JustHit)
        {
            conteneur = gameObject;
            isCharging = true;
        }
    }

    public override void ChargingSkill(int WhichWeapon)
    {
        if (isCharging)
        {
            if (Charge<ChargeMax && ressource.deadList.Count>0)
            {
                base.ChargingSkill(WhichWeapon);
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
            if (Physics.Raycast(MousePosition, out RaycastHit Hit,Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            {
                lineRenderer.SetPosition(0, Parent.transform.position);
                Vector3 HitPosition = Hit.point;
                //HitPosition.y = Parent.transform.position.y;
                HitPosition -= Parent.transform.position;
                HitPosition = HitPosition.normalized;
                HitPosition *= PorteMaximale*(Charge/ChargeMax);
                LastPosition = Parent.transform.position + HitPosition;
                lineRenderer.SetPosition(1,LastPosition);
                lineRenderer.startColor = Color.cyan;
            }
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if (isCharging)
        {
            Vector3 playerToMouse = LastPosition - transform.parent.parent.position;
            playerToMouse.y = 0;
            playerToMouse = playerToMouse.normalized;
            //playerToMouse *= (Charge / ChargeMax);
            Parent.GetComponent<CharacterMovement>().OnDash = true;
            Parent.GetComponent<CharacterMovement>().HitPosition = LastPosition;
            Avatar.layer = 12;
            Parent.GetComponent<CapsuleCollider>().enabled = enabled;
            Destroy(Parent.GetComponent<LineRenderer>());
            Parent.tag = "Player";
            ConteneurRigibody.useGravity = false;
            ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            ConteneurRigibody.velocity = playerToMouse * DashSpeed;
            Charge = 0;
            isCharging = false;
        }


        // if (Physics.Raycast(rayon, out RaycastHit hit,Mathf.Infinity,LayerMask.GetMask("Sol")))
        // {
        //     if (hit.transform.tag != "Collider")
        //     {
        //         //RemoveDeadList();
        //         //HitPosition = hit.point;
        //         HitPosition.x = transform.position.x + (hit.point.x - transform.position.x)*(Charge/ChargeMax); 
        //         HitPosition.z = transform.position.z + (hit.point.z - transform.position.z)*(Charge/ChargeMax);
        //         // HitPosition.x *= (Charge / ChargeMax);
        //         // HitPosition.z *= (Charge / ChargeMax);
        //         Vector3 playerToMouse = HitPosition - transform.parent.parent.position;
        //         playerToMouse.y = 0;
        //         playerToMouse = playerToMouse.normalized;
        //         //playerToMouse *= (Charge / ChargeMax);
        //         Parent.GetComponent<CharacterMovement>().OnDash = true;
        //         Parent.GetComponent<CharacterMovement>().HitPosition = HitPosition;
        //         Avatar.layer = 12;
        //         Parent.GetComponent<CapsuleCollider>().enabled = enabled;
        //         Parent.tag = "Player";
        //         ConteneurRigibody.useGravity = false;
        //         ConteneurRigibody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        //         //Vector3 Dir = (hit.transform.position - transform.parent.position).normalized;
        //         ConteneurRigibody.velocity = playerToMouse*DashSpeed;
        //         Debug.Log("    ?");
        //         // ConteneurRigibody.velocity *= (Charge / ChargeMax);
        //         //ConteneurRigibody.AddForce(playerToMouse*DashSpeed, ForceMode.Impulse);
        //     }
        //     Charge = 0;
        //     isCharging = false;
        // }
    }

    public void DashUpgrade(string TypeUpgrade)
    {
        switch (TypeUpgrade)
        {
            case "Speed":
                DashSpeed *= 1.5f;
                break;
            case "Charge":
                ChargeMax--;
                break;
            default:
                break;
        }
    }
}
