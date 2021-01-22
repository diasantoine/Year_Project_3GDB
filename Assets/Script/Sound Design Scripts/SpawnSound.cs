using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSound : MonoBehaviour
{

    public GameObject sonSouris;
    public float timeMax;
    private float Chrono;

    public float rayonMax;
    public float timeToLast;

    // Start is called before the first frame update
    void Start()
    {
        Chrono = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Chrono >= timeMax)
        {
            GameObject PreSonSouris = Instantiate(sonSouris);
            PreSonSouris.transform.parent = transform;
            PreSonSouris.transform.localPosition = new Vector3(Random.Range(-rayonMax, rayonMax), 0, Random.Range(-rayonMax, rayonMax));
            PreSonSouris.transform.parent = null;
            Destroy(PreSonSouris, timeToLast);
            Chrono = 0;
        }
        else
        {
            Chrono += Time.deltaTime;
        }
    }
}
