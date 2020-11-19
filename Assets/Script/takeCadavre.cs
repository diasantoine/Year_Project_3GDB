using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeCadavre : MonoBehaviour
{

    public bool gotcha;

    private bool isMunitions;

    public Transform player;

    [SerializeField] private float threshold;
    [SerializeField] private float vitesse;
    

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
}
