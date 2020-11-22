using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCadavre : MonoBehaviour
{

    public bool gotcha;

    [HideInInspector] public bool isMunitions;
    [HideInInspector] public bool charge;


    public Transform player;
    public Transform pierre;


    [SerializeField] private float threshold;
    [SerializeField] private float vitesse;

    public detectDead deadD;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gotcha)
        {
            if(player != null)
            {
                Vector3 direction = player.position - transform.position;

                if (direction.magnitude < threshold)
                {
                    isMunitions = true;
                    gotcha = false;
                    gameObject.transform.parent = player.transform;
                    gameObject.layer = 10;

                }

                direction = direction.normalized;

                transform.position += direction * vitesse * Time.deltaTime;
            }
        }

        if (isMunitions)
        {
            gameObject.transform.RotateAround(player.position, Vector3.up, 45f * Time.deltaTime);
            gameObject.transform.LookAt(player);
        }
    }

    private void FixedUpdate()
    {
        if (charge)
        {

            if(pierre != null)
            {
                Vector3 direction = pierre.position - transform.position;

                direction = direction.normalized;

                transform.position += direction * vitesse * Time.deltaTime;
            }
            else
            {
                gotcha = true;
                charge = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (charge)
        {
            if (other.gameObject.CompareTag("chargeTrigger"))
            {
                Destroy(gameObject);
            }
        }
    }
}
