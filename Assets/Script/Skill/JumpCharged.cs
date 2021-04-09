using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCharged : skill
{
    [SerializeField] private Rigidbody ConteneurRigibody;
    [SerializeField] public float JumpSpeed = 20;
    [SerializeField] public float JumpHigh = 20;
    [SerializeField] public float ChargeMax = 7;
    [SerializeField] private GameObject Parent;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private int PorteMaximale;
    //[SerializeField] private GameObject Canon;
    [SerializeField] private Material BRO;
    public int Charge = 0;
    private int LastCharge = 0;
    private LineRenderer lineRenderer;
    private Vector3 HitPosition;
    private Vector3 LastPosition;
    
    public override void UsingSkill()
    {
        theProjo = gameObject;
        this.isCharging = true;
    }

    public override void ChargingSkill(int WhichWeapon)
    {
        if (this.isCharging)
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
                lineRenderer.SetPosition(0,this.Parent.transform.position);
                Vector3 HitPosition = Hit.point;
                HitPosition -= this.Parent.transform.position;
                HitPosition = HitPosition.normalized;
                HitPosition *= PorteMaximale*(Charge/ChargeMax);
                LastPosition = this.Parent.transform.position + HitPosition;
                lineRenderer.SetPosition(1,LastPosition);
                lineRenderer.material = BRO;
                lineRenderer.startWidth = 2;
            }
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if (isCharging)
        {
            if (Physics.Raycast(rayon, out RaycastHit Hit,Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            {
                lineRenderer.SetPosition(0, this.Parent.transform.position);
                Vector3 HitPosition = Hit.point;
                HitPosition -= this.Parent.transform.position;
                HitPosition = HitPosition.normalized;
                HitPosition *= PorteMaximale*(Charge/ChargeMax);
                LastPosition = this.Parent.transform.position + HitPosition;
                lineRenderer.SetPosition(1,LastPosition);
                lineRenderer.material = BRO;
                lineRenderer.startWidth = 2;
            }
            float Distance = Vector3.Distance(LastPosition, this.Parent.transform.position);
            Vector3 playerToMouse = LastPosition - this.Parent.transform.position; 
            Debug.DrawRay(LastPosition,transform.forward, Color.black, 500f);
            playerToMouse.y = 0;
            playerToMouse = playerToMouse.normalized;
            Parent.GetComponent<The_Player_Script>().OnJump = true;
            Parent.GetComponent<The_Player_Script>().DistanceJump = Distance;
            Parent.GetComponent<The_Player_Script>().PointOrigineJump = this.Parent.transform.position;
            Parent.GetComponent<The_Player_Script>().HighJump = this.JumpHigh;
            Parent.GetComponent<The_Player_Script>()
                .ListOfYourPlayer[Parent.GetComponent<The_Player_Script>().YourPlayerChoosed]
                .ConteneurRigibody.constraints = RigidbodyConstraints.FreezeRotation;
            Avatar.layer = 12;
            Parent.GetComponent<CapsuleCollider>().enabled = enabled;
            Destroy(Parent.GetComponent<LineRenderer>());
            Parent.tag = "Jump";
            ConteneurRigibody.useGravity = false;
            ConteneurRigibody.velocity = playerToMouse * this.JumpSpeed;
            ConteneurRigibody.mass = 250;
            Charge = 0;
            isCharging = false;
        }
    }
}
