using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class detectDead : MonoBehaviour
{

    public List<GameObject> deadList;
    public static int ressourceInt;

    [SerializeField] private Vector3 tailleTake;

    [SerializeField] private Transform parent;

    [FMODUnity.EventRef]
    public string SonRecolte = "";

    // Start is called before the first frame update
    void Start()
    {
        ressourceInt = 0;
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
            if(ressourceInt <= 100 - 1)
            {
                Destroy(other.gameObject);
                FMODUnity.RuntimeManager.PlayOneShot(SonRecolte, "", 0, transform.position);
                ressourceInt = Mathf.Clamp(ressourceInt, 0, 100 - 1);
                ressourceInt++;
            }
        }
    }


    

    


}
