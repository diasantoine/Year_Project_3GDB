using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirCharge : MonoBehaviour
{

    [SerializeField] private float vitesse;
    [SerializeField] private float dégat;
    [SerializeField] private float radiusExploBase;

    [SerializeField] private GameObject exploFeedback;

    [HideInInspector] public int nCharge;
    [HideInInspector] public bool tipar;

    private Vector3 moveDirection;
    private Rigidbody RB;

    private Vector3 hitPoint;

    // Start is called before the first frame update
    void Start()
    {
        tipar = false;
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (tipar && !other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            hitPoint = other.gameObject.transform.position;
            Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);

            GameObject newExplo = Instantiate(exploFeedback, hitPoint, Quaternion.identity);
            newExplo.transform.localScale = new Vector3(radiusExploBase + transform.localScale.x, radiusExploBase + transform.localScale.x, radiusExploBase + transform.localScale.x) * 2; 

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].GetComponent<ennemyState>() != null)
                {
                    hit[i].GetComponent<ennemyState>().damage(dégat);
                }
            }

            Destroy(newExplo, 0.1f);
            Destroy(gameObject);

        }

    }

    public void Shoot(Vector3 dir)
    {
        if (tipar)
        {
            gameObject.layer = 10;

            moveDirection = dir;
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;
            RB.AddForce(moveDirection * vitesse, ForceMode.Impulse);
        }

    }

    /*private void OnDrawGizmos()
    {
        if(hitPoint != null)
        {
            Gizmos.DrawSphere(hitPoint, radiusExploBase + transform.localScale.x);
        }
    }*/
}
