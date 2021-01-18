using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{

    public bool Ground; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.CompareTag("sol") || other.transform.CompareTag("Ennemy")) && !Ground)
        {
            Ground = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("sol") && !Ground)
        {
            Ground = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("sol") && Ground)
        {
            Ground = false;
        }
    }
}
