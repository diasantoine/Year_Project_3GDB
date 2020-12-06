using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnnemyTahLesFous : MonoBehaviour
{

    [SerializeField] private GameObject ennemyPrefab;

    [SerializeField] private float freqSpawn;
    private float chrono;

    [SerializeField] private int xTime;
    [SerializeField] private int zTime;
    [SerializeField] private float écart;

    private bool spawning;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning)
        {
            SpawningEnnemy();
            spawning = false;
        }
        else
        {
            chrono += Time.deltaTime;
            
            if(chrono >= freqSpawn)
            {
                spawning = true;
                chrono = 0;
            }
        }
    }

    private void SpawningEnnemy()
    {
        //Carré avec tout dedans
        /*for (int x = 0; x < xTime; x++)
        {
            for (int z = 0; z < zTime; z++)
            {
                if (x == 0 || x == xTime)
                {
                    GameObject newEnnemy = Instantiate(ennemyPrefab);
                    newEnnemy.transform.parent = transform;
                    newEnnemy.transform.position = new Vector3(x * écart, 1.25f, z * écart);
                }

            }
        }*/

        for (int i = 0; i < 2; i++)
        {
            for (int x = 0; x < xTime; x++)
            {
                GameObject newEnnemy = Instantiate(ennemyPrefab);
                newEnnemy.transform.parent = transform;
                newEnnemy.transform.position = new Vector3(x * écart, 1.25f, i * xTime * écart);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            for (int z = 0; z < zTime; z++)
            {
                GameObject newEnnemy = Instantiate(ennemyPrefab);
                newEnnemy.transform.parent = transform;
                newEnnemy.transform.position = new Vector3(i * zTime * écart, 1.25f, 1.25f + z * écart);
            }
        }
    }
}
