using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRessource : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private detectDead ListCadavre;
    private int CountChanged;
    void Start()
    {
        CountChanged = ListCadavre.deadList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (ListCadavre.deadList.Count != CountChanged)
        {
            if (ListCadavre.deadList.Count == 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                CountChanged = ListCadavre.deadList.Count;
            }
            else if(ListCadavre.deadList.Count > 0)
            {
                float ConteneurYScale = Mathf.Clamp(0.01f * ListCadavre.deadList.Count, 0, 0.93f);
                transform.localScale = new Vector3(transform.localScale.x, ConteneurYScale, transform.localScale.z);
                CountChanged = ListCadavre.deadList.Count;
            }
        }
    }
}
