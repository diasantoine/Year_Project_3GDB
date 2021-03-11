using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour
{

    [SerializeField] private GameObject text;

    private SpawnSysteme spawn;

    private bool finish;
    [HideInInspector] public bool ArenaEnd;

    public float timeWait;
    private float chrono;    

    // Start is called before the first frame update
    void Start()
    {
        spawn = GetComponent<SpawnSysteme>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn.mobRestant <= 0 && !finish)
        {
            if(spawn.ListEnnemy.Count <= 0)
            {
                finish = true;
            }
        }

        if (finish)
        {
            if(chrono >= timeWait)
            {
                finish = false;
                chrono = 0;
                spawn.NextWave();
            }
            else
            {
                chrono += Time.deltaTime;
            }
        }

        if (ArenaEnd)
        {
            text.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
