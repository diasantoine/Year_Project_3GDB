using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string Ambiance = "";

    FMOD.Studio.EventInstance ambiance;

    [SerializeField] private Pause pause;

    [Header("Menu")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject nextArenaMenu;

    [Header("NextScene")]
    [SerializeField] private float timeQuit;
    [SerializeField] private Animator anim;
    [SerializeField] private Slider slider;

    [Header("Debug")]
    [SerializeField] private int sceneHere;
    private bool doOnce = false;

    private int sceneIndex;
    private bool over;
    private bool DebugRestart;
    private bool Restart;

    private void Awake()
    {
        if (!doOnce)
        {
            ambiance = FMODUnity.RuntimeManager.CreateInstance(Ambiance);
            ambiance.start();
            doOnce = true;
        }

    }

    private void Start()
    {
        ambiance.release();

    }

    private void Update()
    {
        if (over)
        {
            if (Input.anyKey)
            {
                Restart = true;
                
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            DebugRestart = true;
        }

        if (DebugRestart)
        {
            LoadLevel(sceneHere);
        }

        if (Restart)
        {
            LoadLevel(sceneIndex);
        }
    }

    public void LoadLevel(int scene)
    {
        anim.SetBool("Fade", true);

        if(timeQuit <= 0)
        {
            ambianceStop();
            slider.gameObject.SetActive(true);
            StartCoroutine(LoadLevelGood(scene));
            Debug.Log("oh");

        }
        else
        {
            timeQuit -= Time.deltaTime;
        }
    }

    public void NextArena()
    {
        //pause.StopGame();
        sceneIndex = 2;
        nextArenaMenu.SetActive(true);
        over = true;

    }

    public void GameOver()
    {
        //pause.StopGame();
        sceneIndex = 0;
        gameOverMenu.SetActive(true);
        over = true;
    }

    public void ambianceStop()
    {
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/");
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //ambiance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambiance.release();
    }

    IEnumerator LoadLevelGood(int scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            slider.value = progress;

            yield return null;
        }

    }
}
