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
                Vector2 ConteneurHeigh =  GetComponent<RectTransform>().sizeDelta;
                ConteneurHeigh = new Vector2(ConteneurHeigh.x,0);
                GetComponent<RectTransform>().sizeDelta = ConteneurHeigh;
            }
            else
            {
                Vector2 ConteneurHeigh =  GetComponent<RectTransform>().sizeDelta;
                ConteneurHeigh = new Vector2(ConteneurHeigh.x,50* ListCadavre.deadList.Count);
                GetComponent<RectTransform>().sizeDelta = ConteneurHeigh;
            }
        }
    }
}
