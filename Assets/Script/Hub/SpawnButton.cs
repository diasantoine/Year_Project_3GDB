using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{

    [SerializeField] private SceneThing sceneManager;
    [SerializeField] private GameObject button;
    [SerializeField] private Transform exitSpawn;
    private bool pause;
    private Transform player;

    public string function;

    private void Awake()
    {
        pause = false;
        player = GameObject.Find("Player").transform;
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
            player.transform.position = exitSpawn.position;
            pause = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (function)
            {
                case "Quit":
                    sceneManager.QuitGame();
                    break;
                case "Play":
                    sceneManager.LoadLevel(1);
                    break;
            }           
        }
    }
}
