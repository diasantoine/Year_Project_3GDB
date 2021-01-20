using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSalle : MonoBehaviour
{

    [SerializeField] private Transform player;

    [SerializeField] private Transform spawn;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = spawn.position;
        }
    }
}
