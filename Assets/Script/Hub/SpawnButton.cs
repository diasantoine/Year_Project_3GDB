using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{

    [SerializeField] private GameObject button;
    private bool pause;

    private void Awake()
    {
        pause = false;
    }

    private void Update()
    {
        if (pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                spawnPause();
            }
        }
    }

    public void spawnPause()
    {
        if (!pause)
        {
            button.SetActive(true);
            Pause.isPause = true;
            pause = true;

        }
        else
        {
            button.SetActive(false);   
            Pause.isPause = false;
            pause = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spawnPause();
        }
    }
}
