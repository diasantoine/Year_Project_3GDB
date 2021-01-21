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
    [SerializeField] private GameObject Canon;
    public int Charge = 0;
    private LineRenderer lineRenderer;
    private Vector3 HitPosition;
    private Vector3 LastPosition;
    public bool AfterShock = false;
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
            if (Charge<ChargeMax && detectDead.ressourceInt > 0)
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
                lineRenderer.SetPosition(0, Canon.transform.position);
                Vector3 HitPosition = Hit.point;
                //HitPosition.y = Parent.transform.position.y;
                HitPosition -= Canon.transform.position;
                HitPosition = HitPosition.normalized;
                HitPosition *= PorteMaximale*(Charge/ChargeMax);
                LastPosition = Canon.transform.position + HitPosition;
                lineRenderer.SetPosition(1,LastPosition);
                lineRenderer.startColor = Color.cyan;
                //LastPosition = HitPosition;
            }
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if (isCharging)
        {
            if (AfterShock)
            {
                Parent.GetComponent<CharacterMovement>().Aftershock = AfterShock;
            }
            if (Physics.Raycast(rayon, out RaycastHit Hit,Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            {
                Vector3 HitPosition = Hit.point;
                //HitPosition.y = Parent.transform.position.y;
                HitPosition -= Canon.transform.position;
                HitPosition = HitPosition.normalized;
                HitPosition *= PorteMaximale*(Charge/ChargeMax);
                LastPosition = Canon.transform.position + HitPosition;
                lineRenderer.SetPosition(1,LastPosition);
                lineRenderer.startColor = Color.cyan;
                //LastPosition = HitPosition;
            }
            Vector3 playerToMouse = LastPosition - Canon.transform.position; //transform.parent.parent.position;
            lineRenderer.SetPosition(1,playerToMouse);
            Debug.DrawRay(LastPosition,transform.forward, Color.black, 500f);
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
            case "AfterShock":
                AfterShock = true;
                break;
            default:
                break;
        }
    }
}
