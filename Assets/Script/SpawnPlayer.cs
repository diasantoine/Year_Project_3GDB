using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [SerializeField] private Transform player;
    private spawnEnnemyBasique SEB;

    [SerializeField] private List<Transform> spawnList;

    // Start is called before the first frame update
    void Start()
    {
        player.position = gameObject.transform.position;
        SEB = GameObject.Find("SpawnSystem").GetComponent<spawnEnnemyBasique>();

        if (spawnList.Count > 0)
        {
            for (int i = 0; i < spawnList.Count; i++)
            {
                SEB.spawnPoz.Add(spawnList[i]);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
