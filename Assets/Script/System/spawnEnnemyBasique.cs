using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnnemyBasique : MonoBehaviour
{

    [SerializeField] private GameObject ennemyPre;
    [SerializeField] private choiceSkill CS;

    [SerializeField] List<GameObject> ennemyPreList;
    [Range(0, 1)] [SerializeField] private float chanceForMini;
    [Range(0, 1)] [SerializeField] private float chanceForBig;

    [SerializeField] private Transform parentEnnemy;
    [SerializeField] private Transform player;
    [SerializeField] private int ennemyPerSpawn;

    [SerializeField] private float freqSpawn;
    private float freqChrno;

    [SerializeField] private float waitTime;
    private float waitChrono;

    public List<Transform> spawnPoz;

    public int maxEnnemy;
    private int ennemySpawningRemaining;
    [HideInInspector] public int numberEnnemy;


    [HideInInspector] public bool vagueEnCours;
    [HideInInspector] public bool vagueFini;

    // Start is called before the first frame update
    void Start()
    {
       
        vagueEnCours = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (spawnPoz.Count < 0 || maxEnnemy == 0)
        {
            maxEnnemy = ennemyPerSpawn * spawnPoz.Count;
            ennemySpawningRemaining = maxEnnemy;
        }

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
            }

            if (numberEnnemy <= 0 && ennemySpawningRemaining <= 0)
            {
                vagueEnCours = false;
                vagueFini = true;
                Debug.Log("Manche Finie");
                CS.nVague++;
                freqChrno = 0;
            }
        }
        else
        {
            if(!vagueFini)
            {
                if (waitChrono >= waitTime)
                {
                    RefreshNumberEnnemy();

                    waitChrono = 0;
                    CS.nVague++;
                    vagueEnCours = true;
                }
                else
                {
                    waitChrono += Time.deltaTime;
                }
            }
            else
            {
                if (GameObject.Find("LDSalle") == null)
                {
                    vagueFini = false;
                }
            }
        }

    }

    private void RefreshNumberEnnemy()
    {

        maxEnnemy = ennemyPerSpawn * spawnPoz.Count;
        ennemySpawningRemaining = maxEnnemy;

        /*float maxEnnemyFloat = maxEnnemy;

        maxEnnemyFloat *= 1.25f;
        maxEnnemy = Mathf.RoundToInt(maxEnnemyFloat);
        ennemySpawningRemaining = maxEnnemy;
        Debug.Log(maxEnnemy);*/

    }

    void Spawning()
    {
        for (int i = 0; i < spawnPoz.Count && ennemySpawningRemaining > 0; i++)
        {

            var random = Random.value;

            if(random <= chanceForMini)
            {
                InitialyzeEnnemy(i, ennemyPreList[0]);
            }
            else if(random >= chanceForBig)
            {
                InitialyzeEnnemy(i, ennemyPreList[2]);
            }
            else
            {
                InitialyzeEnnemy(i, ennemyPreList[1]);
            }

            /*GameObject newEnnemy = Instantiate(ennemyPre);
            newEnnemy.transform.parent = parentEnnemy;
            newEnnemy.transform.position = spawnPoz[i].position + new Vector3(0, 1.25f, 0);
            newEnnemy.GetComponent<ennemyState>().SEB = gameObject.GetComponent<spawnEnnemyBasique>();
            newEnnemy.GetComponent<ennemyAI>().player = player;
            newEnnemy.GetComponent<ennemyState>().player = player;*/

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
