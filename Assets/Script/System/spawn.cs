using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{

    public GameObject preEnnemy;
    public GameObject preCadavre;

    RaycastHit hit;

    [SerializeField] private Camera cam;

    private float chrono;

    [SerializeField] private float chronoMax;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        //cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayon = cam.ScreenPointToRay(Input.mousePosition);

        if(chrono >= chronoMax)
        {
            /*if (Input.GetKey(KeyCode.E))
            {
                if (Physics.Raycast(rayon, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.CompareTag("sol"))
                    {
                        GameObject ConteneurGameobject = Instantiate(preEnnemy, new Vector3(hit.point.x, preEnnemy.transform.position.y, hit.point.z), Quaternion.identity, GameObject.Find("EnnemiParent").transform);
                        ConteneurGameobject.GetComponent<ennemyState>().SEB = GameObject.Find("SpawnEnnemyParent").GetComponent<spawnEnnemyBasique>();
                        chrono = 0;
                        ConteneurGameobject.GetComponent<ennemyAI>().player = player.transform; 
                        ConteneurGameobject.GetComponent<ennemyState>().player = player.transform;
                    }
                }
            }*/

            if (Input.GetKey(KeyCode.F))
            {
                if (Physics.Raycast(rayon, out hit, Mathf.Infinity, LayerMask.GetMask("Sol")))
                {
                    Instantiate(preCadavre, hit.point + new Vector3(0, 0.2f, 0), Quaternion.identity, GameObject.Find("CadavreParent").transform);
                    chrono = 0;
                }
            }
        }
        else
        {
            chrono += Time.deltaTime;

        }
    }
}
