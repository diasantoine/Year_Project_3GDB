using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour
{

    [SerializeField] private GameObject text;
    [SerializeField] private Text timeText;

    private SpawnSysteme spawn;

    private bool finish;
    [HideInInspector] public bool ArenaEnd;

    [Header("Timer")]
    [SerializeField] private float timeWave;
    private float chronoWave;
    public float timeAfterWave;
    private float chrono;
    //[SerializeField] private float timeWaveSecondes;
    //[SerializeField] private float timeWaveMinutes;
   

    // Start is called before the first frame update
    void Start()
    {
        chronoWave = timeWave;

        spawn = GetComponent<SpawnSysteme>();
        finish = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        WaitTimeWaveSystem();
        End();
    }

    private void WaitTimeWaveSystem()
    {
        if (!finish)
        {
            if (chronoWave <= 0)
            {
                finish = true;
                
            }
            else
            {
                WaitDeathMobWaveSystem();

                chronoWave -= Time.deltaTime;
                if(chronoWave >= 10)
                {
                    timeText.text = string.Format("{0} Remaining", (int)chronoWave);

                }
                else
                {
                    timeText.text = string.Format("0{0} Remaining", (int)chronoWave);

                }
            }
        }
    }


    private void WaitDeathMobWaveSystem()
    {
        if (spawn.mobRestant <= 0 && !finish)
        {
            if (spawn.ListEnnemy.Count <= 0)
            {
                finish = true;
            }
        }       
    }

    private void End()
    {
        if (finish && !ArenaEnd)
        {
            if (chrono >= timeAfterWave)
            {
                chrono = 0;
                chronoWave = timeWave;
                spawn.NextWave();
                finish = false;

                if (chronoWave >= 10)
                {
                    timeText.text = string.Format("{0} Remaining", (int)timeWave);

                }
                else
                {
                    timeText.text = string.Format("0{0} Remaining", (int)timeWave);

                }
            }
            else
            {
                chrono += Time.deltaTime;

                if (chrono >= 10)
                {
                    timeText.text = string.Format("{0}", (int)chrono);

                }
                else
                {
                    timeText.text = string.Format("0{0}", (int)chrono);

                }
            }
        }

        if (ArenaEnd)
        {
            if(spawn.mobRestant <= 0 && spawn.ListEnnemy.Count <= 0)
            {
                timeText.gameObject.SetActive(false);
                text.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SceneManager.LoadScene(0);
                }
            }

            timeText.text = string.Format("{0} Ennemy", spawn.ListEnnemy.Count);
        }
    }
}
