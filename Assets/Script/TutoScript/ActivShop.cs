using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivShop : MonoBehaviour
{


    [SerializeField] private List<GameObject> shop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(shop.Count > 0)
            {
                shop[0].SetActive(true);
                shop.Remove(shop[0]);
            }
        }
    }
}
