using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class detectDead : MonoBehaviour
{

    public List<GameObject> deadList;

    [SerializeField] private Image zone;

    [SerializeField] private Vector3 tailleTake;

    //[SerializeField] private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(deadList.Count > 0)
        {
            zone.color = new Vector4(0, 1, 0, 0.2f);
        }
        else
        {
            zone.color = new Vector4(0.5f, 0.5f, 0.5f, 0.2f);
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dead"))
        {
            if(other.gameObject != deadList.Contains(other.gameObject) && !other.gameObject.GetComponent<takeCadavre>().charge)
            {
                deadList.Add(other.gameObject);
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                other.gameObject.GetComponent<takeCadavre>().gotcha = true;
                //other.gameObject.GetComponent<takeCadavre>().player = parent;
                other.gameObject.GetComponent<takeCadavre>().deadD = gameObject.GetComponent<detectDead>();
                other.gameObject.transform.localScale = tailleTake;
            }
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Dead"))
        {
            deadList.Remove(other.gameObject);
        }
    }*/

    

    


}
