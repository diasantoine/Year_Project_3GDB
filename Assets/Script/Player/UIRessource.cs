using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRessource : MonoBehaviour
{
    // Start is called before the first frame update
    private int CountChanged;
    private Vector3 ConteneurScale;
    [SerializeField] private Renderer Liquid;
    void Start()
    {
        CountChanged = detectDead.ressourceInt;
        ConteneurScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        int ressource = detectDead.ressourceInt;

        if (ressource != CountChanged)
        {
            if (ressource == 0)
            {
                Liquid.material.SetFloat("_FillAmount",1.7f);
                //transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                CountChanged = ressource;
            }
            else if(ressource > 0)
            {
                Liquid.material.SetFloat("_FillAmount",1.7f - ressource*0.01f);
                //float ConteneurYScale = Mathf.Clamp(0.01f * ressource, 0, 0.93f);
                //transform.localScale = new Vector3(transform.localScale.x, ConteneurYScale, transform.localScale.z);
                CountChanged = ressource;
            }
        }
    }
}
