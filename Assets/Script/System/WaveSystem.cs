using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour
{

    [SerializeField] private GameSystem GS;
    [SerializeField] private The_Player_Script player;

    [Header("Son")]
    [FMODUnity.EventRef]
    public string VagueTimer = "";

    FMOD.Studio.EventInstance vagueTimer;


    private bool isPlaying;

    [Header("Text")]
    [SerializeField] private Text text;
    [SerializeField] private Text timeText;

    private SpawnSysteme spawn;

    private bool finish;
    [HideInInspector] public bool ArenaEnd;
    [HideInInspector] public int minutes;
    [HideInInspector] public float secondes;

    [Header("Timer")]
    public float timeAfterWave;
    private float chrono;
   
    // Start is called before the first frame update
    void Start()
    {
        vagueTimer = FMODUnity.RuntimeManager.CreateInstance(VagueTimer);
        isPlaying = false;
        chrono = timeAfterWave + 1;
        spawn = GetComponent<SpawnSysteme>();
        finish = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        WaitTimeWaveSystem();
        End();
    }

    private void MinutesSecond()
    {
        if(secondes <= 0)
        {
            minutes--;
            secondes = 60;
        }
        else
        {
            secondes -= Time.deltaTime;
        }

        if(Mathf.RoundToInt(secondes) >= 10)
        {
            timeText.text = string.Format("{0} : {1}", minutes, Mathf.RoundToInt(secondes));

        }
        else
        {
            timeText.text = string.Format("{0} : 0{1}", minutes, Mathf.RoundToInt(secondes));

        }

        DixSecVague();
    }

    private void DixSecVague()
    {
        if(minutes == 0 && secondes <= 10)
        {
            if(!isPlaying)
            {
                vagueTimer.start();
                isPlaying = true;
            }
        }
    }

    private void WaitTimeWaveSystem()
    {
        if (!finish)
        {
            if (minutes <= 0 && secondes <= 0)
            {
                finish = true;

            }
            else
            {
                WaitDeathMobWaveSystem();
                MinutesSecond();

                text.text = "VAGUE " + spawn.IndexWave;

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
                vagueTimer.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);


                if (player.playerLife < 3)
                {
                    player.playerLife++;
                }
            }
        }       
    }

    private void End()
    {
        if (finish && !ArenaEnd)
        {
            if (chrono < 0)
            {
                chrono = timeAfterWave;               
                spawn.NextWave();
                finish = false;
                isPlaying = false;
               
            }
            else
            {
                chrono -= Time.deltaTime;
                timeText.text = string.Format("{0}", (int)chrono);
                text.text = "PROCHAINE VAGUE";

            }
        }

        if (ArenaEnd)
        {
            if(spawn.mobRestant <= 0 && spawn.ListEnnemy.Count <= 0)
            {
                timeText.gameObject.transform.parent.gameObject.SetActive(false);
                text.gameObject.transform.parent.gameObject.SetActive(false);
                text.text = "";
                GS.NextArena();
                
            }
            text.text = "ENNEMY";
            timeText.text = string.Format("{0}", spawn.ListEnnemy.Count);
        }
    }
}
