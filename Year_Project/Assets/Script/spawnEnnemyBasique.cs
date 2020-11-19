using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnnemyBasique : MonoBehaviour
{

    [SerializeField] private GameObject ennemyPre;
    [SerializeField] private Transform parentEnnemy;

    [SerializeField] private float freqSpawn;
    private float chrono;

    public List<Transform> spawnPoz;

    public int maxEnnemy;
    [HideInInspector] public int numberEnnemy;

    [SerializeField] private float vitesseRotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        if (chrono >= freqSpawn)
        {
            Spawning();
            chrono = 0;
        }
        else
        {
            chrono += Time.deltaTime;
            transform.Rotate(0, vitesseRotation * Time.deltaTime, 0);
        }
    }

    void Spawning()
    {
        for (int i = 0; i < spawnPoz.Count && numberEnnemy <= maxEnnemy; i++)
        {
            GameObject newEnnemy = Instantiate(ennemyPre);
            newEnnemy.transform.parent = parentEnnemy;
            newEnnemy.transform.position = spawnPoz[i].position + new Vector3(0, 1.25f, 0);
            newEnnemy.GetComponent<ennemyState>().SEB = gameObject.GetComponent<spawnEnnemyBasique>();
            numberEnnemy++;
        }
    }
}
