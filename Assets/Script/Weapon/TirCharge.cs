using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TirCharge : MonoBehaviour
{

    [SerializeField] private float vitesse;
    [SerializeField] private float dégat;
    [SerializeField] private float radiusExploBase;

    [SerializeField] private float ExploForce;

    [SerializeField] private GameObject exploFeedback;

    [HideInInspector] public int nCharge;
    public int nChargeMax;
    [HideInInspector] public bool tipar;

    private Vector3 moveDirection;
    private Rigidbody RB;

    private Vector3 hitPoint;

    [FMODUnity.EventRef]
    public string TirCharge_Impact = "";

    // Start is called before the first frame update
    void Start()
    {
        tipar = false;
        RB = GetComponent<Rigidbody>();

        if(SceneManager.GetActiveScene().name == "SceneTuto")
        {
            ExploForce = 50;
        }
        else
        {
            ExploForce = 120;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (tipar && other.CompareTag("sol") || other.CompareTag("Mur"))
        {
            //ExplosionTahLesFous(other);
            ImpulsionTahLesfous(other);
            FMODUnity.RuntimeManager.PlayOneShot(TirCharge_Impact, transform.position);
        }

    }

    public void Shoot(Vector3 dir)
    {
        if (tipar)
        {
            gameObject.layer = 10;

            moveDirection = dir;
            moveDirection.y = 0;
            //moveDirection = moveDirection.normalized;
            RB.AddForce(moveDirection, ForceMode.Impulse);
        }

    }

    void ImpulsionTahLesfous(Collider col)
    {
        hitPoint = transform.position;
        ExploForce = ExploForce + nCharge * 10;
        Collider[] hit = Physics.OverlapSphere(hitPoint, radiusExploBase + transform.localScale.x);

        GameObject newExplo = Instantiate(exploFeedback, hitPoint + new Vector3(0, 0.5f, 0), Quaternion.identity);
        newExplo.transform.localScale = new Vector3(radiusExploBase + transform.localScale.x, radiusExploBase + transform.localScale.x, radiusExploBase + transform.localScale.x);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].GetComponent<ennemyAI>() != null)
            {
                hit[i].GetComponent<ennemyAI>().ExplosionImpact(hitPoint, radiusExploBase + transform.localScale.x, ExploForce);
            }
        }

        Destroy(newExplo, 0.2f);
        Destroy(gameObject);
    }

    void ExplosionTahLesFous(Collider col)
    {
        Debug.Log(col.gameObject.name);
        hitPoint = col.gameObject.transform.position;
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
