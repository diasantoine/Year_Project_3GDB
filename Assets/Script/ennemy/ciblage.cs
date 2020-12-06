using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ciblage : MonoBehaviour
{
    [SerializeField] private GameObject preDead;

    private float hpNow;

    [SerializeField] private float hpMax;

    [SerializeField] private int numberCadav;

    // Start is called before the first frame update
    void Start()
    {
        hpNow = hpMax;
    }

    // Update is called once per frame
    void Update()
    {

        if(hpNow <= 0)
        {
            float écart = -numberCadav/2;

            Destroy(gameObject);
            for(int i = 1; i <= numberCadav; i++)
            {              
                Instantiate(preDead, transform.position + new Vector3(0, 0, écart * 1.25f), Quaternion.identity, GameObject.Find("CadavreParent").transform);
                écart++;
            }
        }
    }

    public void damage(float hit)
    {
        hpNow -= hit;
    }
}
