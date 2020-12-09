using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnnemyBasique : MonoBehaviour
{

    [SerializeField] private GameObject ennemyPre;

    [SerializeField] private List<GameObject> ennemyPreList;
    [Range(0, 1)] [SerializeField] private float chanceForMini;
    [Range(0, 1)] [SerializeField] private float chanceForBig;

    [SerializeField] private Transform parentEnnemy;
    [SerializeField] private Transform player;


    [SerializeField] private float freqSpawn;
    private float freqChrno;

    [SerializeField] private float waitTime;
    private float waitChrono;

    public List<Transform> spawnPoz;

    public int maxEnnemy;
    private int ennemySpawningRemaining;
    [HideInInspector] public int numberEnnemy;

    //[SerializeField] private float vitesseRotation;

    private bool vagueEnCours;

    // Start is called before the first frame update
    void Start()
    {
        ennemySpawningRemaining = maxEnnemy;
        vagueEnCours = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (vagueEnCours)
        {
            if (freqChrno >= freqSpawn)
            {
                Spawning();
                freqChrno = 0;
            }
            else
            {
                freqChrno += Time.deltaTime;
                //transform.Rotate(0, vitesseRotation * Time.deltaTime, 0);
            }

            if(numberEnnemy <= 0 && ennemySpawningRemaining <= 0)
            {
                vagueEnCours = false;
                Debug.Log("Manche Finie");
                freqChrno = 0;
            }
        }
        else
        {

            if(waitChrono >= waitTime)
            {
                RefreshNumberEnnemy();

                waitChrono = 0;
                vagueEnCours = true;
            }
            else
            {
                waitChrono += Time.deltaTime;
            }
        }

    }

    private void RefreshNumberEnnemy()
    {
        float maxEnnemyFloat = maxEnnemy;

        maxEnnemyFloat *= 1.25f;
        maxEnnemy = Mathf.RoundToInt(maxEnnemyFloat);
        ennemySpawningRemaining = maxEnnemy;
        Debug.Log(maxEnnemy);
    }

    void Spawning()
    {
        for (int i = 0; i < spawnPoz.Count && ennemySpawningRemaining > 0; i++)
        {
            /*if(Random.value <= chanceForMini)
            {
                InitialyzeEnnemy(i, ennemyPreList[0]);
            }
            else if(Random.value >= chanceForBig)
            {
                InitialyzeEnnemy(i, ennemyPreList[2]);
            }
            else
            {
                InitialyzeEnnemy(i, ennemyPreList[1]);
            }*/

            GameObject newEnnemy = Instantiate(ennemyPre);
            newEnnemy.transform.parent = parentEnnemy;
            newEnnemy.transform.position = spawnPoz[i].position + new Vector3(0, 1.25f, 0);
            newEnnemy.GetComponent<ennemyState>().SEB = gameObject.GetComponent<spawnEnnemyBasique>();
            newEnnemy.GetComponent<ennemyAI>().player = player;
            newEnnemy.GetComponent<ennemyState>().player = player;

            ennemySpawningRemaining--;
            numberEnnemy++;
        }
    }

    private void InitialyzeEnnemy(int i, GameObject ennemyPre)
    {
        GameObject newEnnemy = Instantiate(ennemyPre);
        newEnnemy.transform.parent = parentEnnemy;
        newEnnemy.transform.position = spawnPoz[i].position + new Vector3(0, 0, 0);
        newEnnemy.GetComponent<ennemyState>().SEB = gameObject.GetComponent<spawnEnnemyBasique>();
        newEnnemy.GetComponent<ennemyAI>().player = player;
        newEnnemy.GetComponent<ennemyState>().player = player;
    }
}
