using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{

    [SerializeField] private bool Room;
    [SerializeField] private Collider col;

    private bool roomAcces;

    public List<Transform> spawnEnnemy;

    private spawnEnnemyBasique spawnScript;

    // Start is called before the first frame update
    void Start()
    {

        spawnScript = GameObject.Find("SpawnSystem").GetComponent<spawnEnnemyBasique>();

        if (Room)
        {
            roomAcces = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Room)
        {
            if (roomAcces)
            {

                if(spawnEnnemy.Count > 0)
                {
                    for (int i = 0; i < spawnEnnemy.Count; i++)
                    {
                        spawnScript.spawnPoz.Add(spawnEnnemy[0]);
                        spawnEnnemy.Remove(spawnEnnemy[0]);
                    }
                }
            }
        }
        else
        {            
            if (spawnScript.vagueFini)
            {
                col.isTrigger = true;
                gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                col.isTrigger = false;
                gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Room)
            {
                col.enabled = false;
                spawnScript.vagueFini = false;
                roomAcces = true;
            }
            else
            {
                col.enabled = false;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}