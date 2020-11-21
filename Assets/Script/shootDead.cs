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

    [SerializeField] private GameObject preProjo;
    [SerializeField] private Transform canon;

    [SerializeField] private float freqTir;
    private float chrono;

    private bool onShoot;

    RaycastHit floorHit;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
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
}
