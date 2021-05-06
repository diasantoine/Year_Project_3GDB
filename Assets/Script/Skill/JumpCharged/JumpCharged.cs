using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCharged : skill
{
    [Header("VarJump")]
    [SerializeField] public float JumpSpeed = 20;
    [SerializeField] public float JumpHigh = 20;
    [SerializeField] public float ChargeMax = 7;
    [SerializeField] private int PorteMaximale;
    [SerializeField] private float RadiusSouffleAtterissage;
    [SerializeField] private float ForceSouffleAtterissage;
    [SerializeField] private List<int> ListPalier = new List<int>();
    
    [Header("ParameterJump")]
    [SerializeField] private GameObject Parent;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private Rigidbody ConteneurRigibody;
    [SerializeField] private GameObject CercleFeedback;
    //[SerializeField] private GameObject Canon;
    [SerializeField] private Material BRO;
    public int Charge = 0;
    private int LastCharge = 0;
    private LineRenderer lineRenderer;
    private Vector3 HitPosition;
    private Vector3 LastPosition;
    private int IndexWinner;
    private int NumberOfRessourcesUsed;
    
    public override void UsingSkill()
    {
        theProjo = gameObject;
        this.isCharging = true;
    }

    public override void ChargingSkill(int WhichWeapon)
    {
        if (this.isCharging)
        {
            if (!this.CercleFeedback.activeSelf)
            {
                this.CercleFeedback.SetActive(true);
            }

            foreach (int PalierRessources in this.ListPalier)
            {
                if (detectDead.ressourceInt >= PalierRessources)
                {
                    this.IndexWinner = this.ListPalier.IndexOf(PalierRessources);
                }
            }

            this.CercleFeedback.transform.localScale = new Vector3(this.PorteMaximale / 5.880002f / (this.ListPalier.Count - this.IndexWinner),this.PorteMaximale/ 5.880002f / (this.ListPalier.Count - this.IndexWinner),this.PorteMaximale/ 5.880002f / (this.ListPalier.Count - this.IndexWinner));
            // if (Charge<ChargeMax && detectDead.ressourceInt > 0)
            // {
            //     base.ChargingSkill(WhichWeapon);
            // }  
            // if (!Parent.GetComponent<LineRenderer>())
            // {
            //     lineRenderer = Parent.AddComponent<LineRenderer>();
            // }
            // else
            // {
            //     Parent.GetComponent<LineRenderer>().enabled = true;
            // }
            //
            // Ray MousePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            // if (Physics.Raycast(MousePosition, out RaycastHit Hit,Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            // {
            //     lineRenderer.SetPosition(0,this.Parent.transform.position);
            //     Vector3 HitPosition = Hit.point;
            //     HitPosition -= this.Parent.transform.position;
            //     HitPosition = HitPosition.normalized;
            //     HitPosition *= PorteMaximale*(Charge/ChargeMax);
            //     LastPosition = this.Parent.transform.position + HitPosition;
            //     lineRenderer.SetPosition(1,LastPosition);
            //     lineRenderer.material = BRO;
            //     lineRenderer.startWidth = 2;
            // }
        }
    }

    public override void EndUsing(Ray rayon)
    {
        if (isCharging)
        {
            if (this.CercleFeedback.activeSelf)
            {
                this.CercleFeedback.SetActive(false);
            }
            if (Physics.Raycast(rayon, out RaycastHit Hit,Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            {
                if ( new Vector3(Hit.point.x - this.Parent.transform.position.x, 0, Hit.point.z - this.Parent.transform.position.z).magnitude <= this.PorteMaximale)
                {
                    //Debug.Log(Hit.point + " "+ this.Parent.transform.position);
                    // lineRenderer.SetPosition(0, this.Parent.transform.position);
                    // Vector3 HitPosition = Hit.point;
                    // HitPosition -= this.Parent.transform.position;
                    // HitPosition = HitPosition.normalized;
                    // HitPosition *= PorteMaximale*(Charge/ChargeMax);
                    // LastPosition = this.Parent.transform.position + HitPosition;
                    // lineRenderer.SetPosition(1,LastPosition);
                    // lineRenderer.material = BRO;
                    // lineRenderer.startWidth = 2;
                    Vector3 HitPosition = Hit.point;
                    HitPosition -= this.Parent.transform.position;
                    LastPosition = HitPosition;
                    this.LastPosition.y = 0;
                }
                else
                {
                    Vector3 HitPosition = Hit.point - this.Parent.transform.position;
                    HitPosition = HitPosition.normalized;
                    HitPosition = this.Parent.transform.position + HitPosition * this.PorteMaximale;
                    LastPosition = HitPosition;
                    this.LastPosition.y = 0;
                    Debug.DrawRay(LastPosition,transform.forward, Color.red, 500f);
                }
            }
            foreach (int PalierRessources in this.ListPalier)
            {
                if (Vector3.Distance(this.LastPosition, this.CercleFeedback.transform.position) <= PalierRessources )
                {
                    this.NumberOfRessourcesUsed = PalierRessources;
                }
            }
            detectDead.ressourceInt -= this.NumberOfRessourcesUsed;
            //float Distance = Vector3.Distance(LastPosition, this.Parent.transform.position);
            //float Distance = this.LastPosition.magnitude;
            //Vector3 playerToMouse = LastPosition - this.Parent.transform.position; 
            //Vector3 playerToMouse = LastPosition;
            //Debug.DrawRay(LastPosition,transform.forward, Color.black, 500f);
            //playerToMouse.y = 0;
           // playerToMouse = playerToMouse.normalized;
            Parent.GetComponent<The_Player_Script>().OnJump = true;
            //Parent.GetComponent<The_Player_Script>().DistanceJump = Distance;
            Parent.GetComponent<The_Player_Script>().DistanceJump = this.LastPosition.magnitude;
            //Parent.GetComponent<The_Player_Script>().target = this.LastPosition;
            Parent.GetComponent<The_Player_Script>().PointOrigineJump = this.Parent.transform.position;
            Parent.GetComponent<The_Player_Script>()
                .ListOfYourPlayer[Parent.GetComponent<The_Player_Script>().YourPlayerChoosed]
                .ConteneurRigibody.constraints = RigidbodyConstraints.FreezeRotation;
            Parent.GetComponent<The_Player_Script>().radiusExploBase = this.RadiusSouffleAtterissage;
            Parent.GetComponent<The_Player_Script>().ForceExplosion = this.ForceSouffleAtterissage;
            Avatar.layer = 12;
            Parent.GetComponent<CapsuleCollider>().enabled = enabled;
            Destroy(Parent.GetComponent<LineRenderer>());
            Parent.tag = "Jump";
            ConteneurRigibody.useGravity = false;
            ConteneurRigibody.drag = 0;
            ConteneurRigibody.angularDrag = 0;
            //this.JumpSpeed = Distance / 3.800f*2;
            //Debug.Log(this.JumpSpeed);
            //ConteneurRigibody.velocity = playerToMouse * this.JumpSpeed;
            Debug.Log(this.LastPosition.magnitude);
            ConteneurRigibody.velocity = this.LastPosition.normalized * this.LastPosition.magnitude / 3.700f*2;
            Parent.GetComponent<The_Player_Script>().SpeedJump =  ConteneurRigibody.velocity;
            ConteneurRigibody.mass = 250;
            Charge = 0;
            isCharging = false;
        }
    }
}
