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
    [SerializeField] private float RadiusCollider;
    
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
        if(detectDead.ressourceFloat >= canUseRessource)
        {
            if (detectDead.ressourceFloat >= this.ListPalier[0] && !this.isCharging && !this.Parent.GetComponent<The_Player_Script>().OnJump)
            {
                theProjo = gameObject;
                this.isCharging = true;
            }
        }
       
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
                if (detectDead.ressourceFloat >= PalierRessources)
                {
                    this.IndexWinner = this.ListPalier.IndexOf(PalierRessources);
                }
            }
            Debug.Log(this.IndexWinner);
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
                if ( new Vector3(Hit.point.x - this.Parent.transform.position.x, 0, Hit.point.z - this.Parent.transform.position.z).magnitude <= this.PorteMaximale / (this.ListPalier.Count - this.IndexWinner))
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
                    //Vector3 NewOrigin = this.Parent.transform.position + this.LastPosition * (this.Avatar.GetComponent<CapsuleCollider>().radius * this.Avatar.transform.localScale.magnitude / this.LastPosition.magnitude);
                    if (Physics.Raycast(this.Parent.transform.position, this.LastPosition, out RaycastHit Hitt, this.LastPosition.magnitude, LayerMask.GetMask("Wall") ))
                    {
                        if (Hitt.collider.CompareTag("Mur"))
                        {
                            Debug.Log("wall");
                            this.LastPosition = Hitt.point - this.Parent.transform.position;
                            this.LastPosition.y = 0;
                            //this.LastPosition *= 0.8f;
                            //this.LastPosition *= 1 -  this.Avatar.GetComponent<CapsuleCollider>().radius * this.Avatar.transform.lossyScale.magnitude / this.LastPosition.magnitude;
                        }
                    }
                    Debug.DrawRay(this.Parent.transform.position, this.LastPosition, Color.blue, 500f);
                }
                else
                {
                    Vector3 HitPosition = Hit.point - this.Parent.transform.position;
                    HitPosition.y = 0;
                    HitPosition = HitPosition.normalized;
                    HitPosition = HitPosition * (this.PorteMaximale / (this.ListPalier.Count - this.IndexWinner));
                    LastPosition = HitPosition;
                    //this.LastPosition.y = 0;
                    Debug.DrawRay(this.Parent.transform.position,this.LastPosition, Color.red, 500f);
                }
            }
            foreach (int PalierRessources in this.ListPalier)
            {
                if (this.LastPosition.magnitude >= this.PorteMaximale /  (this.ListPalier.Count - this.ListPalier.IndexOf(PalierRessources)))
                {
                    this.NumberOfRessourcesUsed = PalierRessources;
                    Debug.Log("touché");
                }
            }
            Debug.Log(this.NumberOfRessourcesUsed);
            detectDead.ressourceFloat -= this.NumberOfRessourcesUsed;
            //float Distance = Vector3.Distance(LastPosition, this.Parent.transform.position);
            //float Distance = this.LastPosition.magnitude;
            //Vector3 playerToMouse = LastPosition - this.Parent.transform.position; 
            //Vector3 playerToMouse = LastPosition;
            //Debug.DrawRay(LastPosition,transform.forward, Color.black, 500f);
            //playerToMouse.y = 0;
           // playerToMouse = playerToMouse.normalized;
            Parent.GetComponent<The_Player_Script>().OnJump = true;
            //Parent.GetComponent<The_Player_Script>().DistanceJump = Distance;
            Parent.GetComponent<The_Player_Script>().DistanceJump = this.LastPosition.magnitude - RadiusCollider;
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
            ConteneurRigibody.velocity =( this.LastPosition / 2.500f) * 2;
            Parent.GetComponent<The_Player_Script>().SpeedJump =  ConteneurRigibody.velocity;
            ConteneurRigibody.mass = 250;
            Charge = 0;
            isCharging = false;
        }
    }
}
