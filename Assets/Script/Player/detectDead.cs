using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class detectDead : MonoBehaviour
{

    [HideInInspector] public List<GameObject> deadList;
    public static float ressourceFloat;

    [FMODUnity.EventRef]
    public string SonRecolte = "";
    [SerializeField] private float maxRessource;
    [SerializeField] private Image uiRessource;

    private float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ressourceFloat = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Round(ressourceFloat);

        if (ressourceFloat < 0)
            ressourceFloat = 0;

        lerpSpeed = 5f * Time.deltaTime;

        RessourceBarFiller();
    }

    void RessourceBarFiller()
    {
        uiRessource.fillAmount = Mathf.Lerp(uiRessource.fillAmount, ressourceFloat / maxRessource, lerpSpeed);
     
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dead"))
        {
            if(ressourceFloat <= maxRessource - 1)
            {
                Destroy(other.gameObject);
                FMODUnity.RuntimeManager.PlayOneShot(SonRecolte, "", 0, transform.position);
                ressourceFloat = Mathf.Clamp(ressourceFloat, 0, maxRessource - 1);
                ressourceFloat++;
            }
        }
    }


    

    


}
