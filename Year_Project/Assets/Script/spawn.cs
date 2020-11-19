using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{

    public GameObject preEnnemy;
    public GameObject preCadavre;

    RaycastHit hit;

    private Camera cam;

    private float chrono;

    [SerializeField] private float chronoMax;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayon = cam.ScreenPointToRay(Input.mousePosition);

        if(chrono >= chronoMax)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (Physics.Raycast(rayon, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.CompareTag("sol"))
                    {
                        Instantiate(preEnnemy, new Vector3(hit.point.x, preEnnemy.transform.position.y, hit.point.z), Quaternion.identity, GameObject.Find("EnnemiParent").transform);
                        chrono = 0;
                    }
                }
            }

            if (Input.GetKey(KeyCode.F))
            {
                if (Physics.Raycast(rayon, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.CompareTag("sol"))
                    {
                        Instantiate(preCadavre, hit.point + new Vector3(0, 0.2f, 0), Quaternion.identity, GameObject.Find("CadavreParent").transform);
                        chrono = 0;

                    }
                }
            }
        }

        chrono += Time.deltaTime;
    }
}
