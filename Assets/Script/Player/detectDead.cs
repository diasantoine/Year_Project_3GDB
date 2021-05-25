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
    [SerializeField] private Image Diode;

    private float lerpSpeed;
    private float one;

    // Start is called before the first frame update
    void Start()
    {
        ressourceFloat = 0;
        one = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Round(ressourceFloat);

        if(ressourceFloat > 0 && ressourceFloat < 5)
        {
            OffDiode();
        }
        else if(ressourceFloat >= 5)
        {
            Diode.color = new Vector4(1, 1, 1, 1);
        }

        if (ressourceFloat < 1)
        {
            ressourceFloat = 0;
            Diode.color = new Vector4(0, 0, 0, 0);

        }

        if (ressourceFloat > maxRessource)
            ressourceFloat = maxRessource;

        lerpSpeed = 5f * Time.deltaTime;

        RessourceBarFiller();
    }

    private void OffDiode()
    {
        var color = Diode.color.a;

        color += one * 2 * Time.deltaTime;
        Diode.color = new Vector4(1, 1, 1, color);
        Debug.Log(color);

        if (Diode.color.a >= 0.99f)
        {
            one = -1;
        }
        else if (Diode.color.a <= 0.01f)
        {
            one = 1;

        }

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
