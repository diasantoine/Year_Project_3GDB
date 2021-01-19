using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRessource : MonoBehaviour
{
    // Start is called before the first frame update
    private int CountChanged;
    void Start()
    {
        CountChanged = detectDead.ressourceInt;
    }

    // Update is called once per frame
    void Update()
    {

        int ressource = detectDead.ressourceInt;

        if (ressource != CountChanged)
        {
            if (ressource == 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                CountChanged = ressource;
            }
            else if(ressource > 0)
            {
                float ConteneurYScale = Mathf.Clamp(0.01f * ressource, 0, 0.93f);
                transform.localScale = new Vector3(transform.localScale.x, ConteneurYScale, transform.localScale.z);
                CountChanged = ressource;
            }
        }
    }
}
