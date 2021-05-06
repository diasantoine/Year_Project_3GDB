using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootDead : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string TireSon = "";

    RaycastHit hit;

    private Camera cam;

    private GameObject ennemyCible;
    private GameObject pierre;

    [SerializeField] private GameObject preProjo;
    [SerializeField] private GameObject preProjoChargé;

    [SerializeField] private Transform canon;
    [SerializeField] private Transform canonCharge;

    [SerializeField] private float freqTir;
    private float chrono;

    private bool onShoot;
    bool isCharging;

    RaycastHit floorHit;

    private float MultipleSpeed = 1;
    [SerializeField] private int NombreDeProjectile;
    private bool Empoisonnement = false;
    private bool Rocket = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        isCharging = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Pause.isPause && !this.GetComponent<The_Player_Script>().OnJump && !this.GetComponent<The_Player_Script>().OnCounter)
        {
            Ray rayon = cam.ScreenPointToRay(Input.mousePosition);
            TirNormal(rayon);
        }


        /*if (Input.GetMouseButtonDown(0) && onShoot == false)
        {
            if (Physics.Raycast(rayon, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.CompareTag("Ennemy"))
                {
                    if(ennemyCible != null && ennemyCible != hit.collider.gameObject)
                    {
                        ennemyCible.GetComponent<ciblage>().ciblé = false;
                    }

                    ennemyCible = hit.collider.gameObject;
                    ennemyCible.GetComponent<ciblage>().ciblé = true;
                    
                }
            }


        }*/

        /*if (Input.GetMouseButtonDown(1))
        {
            isCharging = true;
            pierre = Instantiate(preProjoChargé, canonCharge.position, Quaternion.identity, canonCharge.transform);
            pierre.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            if(pierre.GetComponent<TirCharge>().nCharge > 0)
            {
                isCharging = false;
                TirCharge(rayon);
            }
            else
            {
                Destroy(pierre);
                isCharging = false;
            }
           
        }

        if(pierre != null)
        {
            if (isCharging && pierre.GetComponent<TirCharge>().nCharge < pierre.GetComponent<TirCharge>().nChargeMax)
            {
                ChargementTir();
            }
        }*/


        /*if(detectD.deadList.Count > 0)
        {
         
            if (Input.GetMouseButtonDown(0))
            {
              
                RaycastHit floorHit;
                
                var projectile = Instantiate(preProjo, detectD.deadList[0].transform.position, Quaternion.identity);

                FMODUnity.RuntimeManager.PlayOneShot(TireSon, transform.position);

                if (Physics.Raycast(rayon , out floorHit, Mathf.Infinity))
                {
                    Vector3 playerToMouse = floorHit.point - detectD.deadList[0].transform.position;
                    projectile.GetComponent<DeadProjo>().Shoot(playerToMouse);

                }

                Destroy(detectD.deadList[0]);
                detectD.deadList.Remove(detectD.deadList[0]);
            }
        }*/

        /*if (onShoot)
        {
            for (int i = 0; i < detectD.deadList.Count; i++)
            {
                var projectile = Instantiate(preProjo, detectD.deadList[i].transform.position, Quaternion.identity);
                projectile.GetComponent<DeadProjo>().cible = ennemyCible.transform;
                Destroy(detectD.deadList[i]);
                detectD.deadList.Remove(detectD.deadList[i]);
            }

            if(detectD.deadList.Count <= 0)
            {
                onShoot = false;
            }
        }*/

    }

    private void TirNormal(Ray rayon)
    {
        if (chrono >= freqTir)
        {
            if (Input.GetMouseButton(0))
            {
                GetComponent<The_Player_Script>().IsNotUsingNormalWeapon = false;
                GetComponent<The_Player_Script>().PercentageWeaponHeat += 1;
                foreach (GameObject WeaponPart in  
                    GetComponent<The_Player_Script>().ListOfYourPlayer[
                        GetComponent<The_Player_Script>().YourPlayerChoosed].ListWeaponPart)
                {
                    WeaponPart.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                        new Color(GetComponent<The_Player_Script>().PercentageWeaponHeat/100f,0 ,0));
                }
                RaycastHit floorHit;
                chrono = 0;
                for (int i = 0; i < NombreDeProjectile; i++)
                {
                    if (Physics.Raycast(rayon, out floorHit, Mathf.Infinity, LayerMask.GetMask("ClicMouse")))
                    {
                        if (i != 0)
                        {
                            if (i%2 == 0)
                            {
                                GameObject projectile = Instantiate(preProjo, canon.position + new Vector3(i/10,0,0), Quaternion.identity);
                                Vector3 playerToMouse =  floorHit.point - canon.position;
                                playerToMouse.x += 1;
                                playerToMouse.z += 1;
                                projectile.GetComponent<DeadProjo>().vitesse *= MultipleSpeed;
                                projectile.GetComponent<DeadProjo>().Empoisonnement = Empoisonnement;
                                projectile.GetComponent<DeadProjo>().Rocket = Rocket;
                                projectile.GetComponent<DeadProjo>().Shoot(playerToMouse);
                            }
                            else
                            {
                                GameObject projectile = Instantiate(preProjo, canon.position + new Vector3(-i/10,0,0), Quaternion.identity);
                                Vector3 playerToMouse =  floorHit.point - canon.position;
                                playerToMouse.x -= 1;
                                playerToMouse.z -= 1;
                                projectile.GetComponent<DeadProjo>().vitesse *= MultipleSpeed;
                                projectile.GetComponent<DeadProjo>().Empoisonnement = Empoisonnement;
                                projectile.GetComponent<DeadProjo>().Rocket = Rocket;
                                projectile.GetComponent<DeadProjo>().Shoot(playerToMouse);
                            }
                        }
                        else
                        {
                            GameObject projectile = Instantiate(preProjo, canon.position , Quaternion.identity);
                            Vector3 playerToMouse = floorHit.point - canon.position;
                            projectile.GetComponent<DeadProjo>().vitesse *= MultipleSpeed;
                            projectile.GetComponent<DeadProjo>().Empoisonnement = Empoisonnement;
                            projectile.GetComponent<DeadProjo>().Rocket = Rocket;
                            projectile.GetComponent<DeadProjo>().Shoot(playerToMouse);
                        }
                    }
                }
                FMODUnity.RuntimeManager.PlayOneShot(TireSon, "", 0, transform.position);
            }
            else
            {
                GetComponent<The_Player_Script>().IsNotUsingNormalWeapon = true;
            }
        }
        else
        {
            chrono += Time.deltaTime;
        }
    }
    
    
    
    public void TirNormalUpgrade(string TypeUpgrade)
    {
        switch (TypeUpgrade)
        {
            case "FrequenceDeTir":
                freqTir *= 0.9f;
                break;
            case "Speed":
                MultipleSpeed *= 1.1f;
                break;
            case "NombreDeProjectile":
                NombreDeProjectile = 3;
                break;
            case "Empoisonnement":
                Empoisonnement = true;
                break;
            case "Rocket":
                Rocket = true;
                break;
            default:
                break;
        }
    }
}
