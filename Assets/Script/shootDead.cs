using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootDead : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string TireSon = "";

    [SerializeField] private detectDead detectD;

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
    private bool isCharging;

    RaycastHit floorHit;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        isCharging = false;

    }

    // Update is called once per frame
    void Update()
    {
        Ray rayon = cam.ScreenPointToRay(Input.mousePosition);

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

        TirNormal(rayon);

        if (Input.GetMouseButtonDown(1))
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
            }
           
        }

        if (isCharging)
        {
            ChargementTir();
        }

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

                RaycastHit floorHit;
                chrono = 0;

                var projectile = Instantiate(preProjo, canon.position, Quaternion.identity);
                FMODUnity.RuntimeManager.PlayOneShot(TireSon, transform.position);

                if (Physics.Raycast(rayon, out floorHit, Mathf.Infinity))
                {
                    Vector3 playerToMouse = floorHit.point - canon.position;
                    projectile.GetComponent<DeadProjo>().Shoot(playerToMouse);

                }

            }
        }
        else
        {
            chrono += Time.deltaTime;
        }
    }

    void ChargementTir()
    {

        if(detectD.deadList.Count > 0)
        {
            
            for (int i = 0; i < detectD.deadList.Count; i++)
            {
                
                takeCadavre TC = detectD.deadList[0].GetComponent<takeCadavre>();

                if(TC.isMunitions)
                {
                    TC.isMunitions = false;
                    TC.charge = true;
                    TC.pierre = pierre.transform;

                    detectD.deadList.Remove(detectD.deadList[0]);
                }

            }
        }

    }

    void TirCharge(Ray rayon)
    {
        RaycastHit floorHit;
        chrono = 0;

        if (Physics.Raycast(rayon, out floorHit, Mathf.Infinity))
        {
            Vector3 playerToMouse = floorHit.point - canon.position;
            pierre.GetComponent<TirCharge>().tipar = true;
            pierre.GetComponent<Rigidbody>().isKinematic = false;
            pierre.transform.parent = null;
            pierre.GetComponent<TirCharge>().Shoot(playerToMouse);

        }
    }
}
