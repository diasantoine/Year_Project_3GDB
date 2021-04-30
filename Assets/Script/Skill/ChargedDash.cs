using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Material BRO;
    private int LastCharge = 0;
    private Vector3 StockLastPosition = new Vector3();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LineDraw()
    {
        float t = 0;
        float time = freqCharge*ChargeMax;
        Vector3 orig = lineRenderer.GetPosition(0);
        Vector3 orig2 = lineRenderer.GetPosition(1);
        lineRenderer.SetPosition(1, orig);
        Vector3 newpos;
        for (; t < time; t += Time.deltaTime)
        {
            newpos = Vector3.Lerp(orig, orig2, t / time);
            lineRenderer.SetPosition(1, newpos);
            yield return null;
        }
        lineRenderer.SetPosition(1, orig2);
    }

    public override void UsingSkill()
    {
        theProjo = gameObject;
        isCharging = true;
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
                lineRenderer.SetPosition(0,Canon.transform.position);
                Vector3 HitPosition = Hit.point;
                HitPosition -= Canon.transform.position;
                HitPosition = HitPosition.normalized;
                HitPosition *= PorteMaximale*(Charge/ChargeMax);
                LastPosition = Canon.transform.position + HitPosition;
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
            if (AfterShock)
            {
                Parent.GetComponent<The_Player_Script>().Aftershock = AfterShock;
            }
            if (Physics.Raycast(rayon, out RaycastHit Hit,Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
            {
                lineRenderer.SetPosition(0, Canon.transform.position);
                Vector3 HitPosition = Hit.point;
                HitPosition -= Canon.transform.position;
                HitPosition = HitPosition.normalized;
                HitPosition *= PorteMaximale*(Charge/ChargeMax);
                LastPosition = Canon.transform.position + HitPosition;
                lineRenderer.SetPosition(1,LastPosition);
                lineRenderer.material = BRO;
                lineRenderer.startWidth = 2;
            }
            float Distance = Vector3.Distance(LastPosition, Canon.transform.position);
            Vector3 playerToMouse = LastPosition - Canon.transform.position; 
            Debug.DrawRay(LastPosition,transform.forward, Color.black, 500f);
            playerToMouse.y = 0;
            playerToMouse = playerToMouse.normalized;
            Parent.GetComponent<The_Player_Script>().OnDash = true;
            Parent.GetComponent<The_Player_Script>().DistanceDash = Distance;
            Parent.GetComponent<The_Player_Script>().PointOrigineDash = Canon.transform.position;
            Avatar.layer = 12;
            Parent.GetComponent<CapsuleCollider>().enabled = enabled;
            Destroy(Parent.GetComponent<LineRenderer>());
            Parent.tag = "Dash";
            ConteneurRigibody.useGravity = false;
            ConteneurRigibody.velocity = playerToMouse * DashSpeed;
            ConteneurRigibody.mass = 250;
            Charge = 0;
            isCharging = false;
        }
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
