using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneThing : MonoBehaviour
{

    [SerializeField] private float timeQuit;
    [SerializeField] private Animator anim;  
    [SerializeField] private Slider slider;

    private string choice = "";
    private int index;


    public void LoadLevel(int scene)
    {
        anim.SetBool("Fade", true);
        choice = "Level";
        index = scene;
    }

    public void QuitGame()
    {
        anim.SetBool("Fade", true);
        choice = "Quit";
    }

    private void Update()
    {
        switch (choice)
        {
            case "Level":
                if (timeQuit <= 0)
                {
                    StartCoroutine(LoadLevelGood(index));
                    slider.gameObject.SetActive(true);

                }
                else
                {
                    timeQuit -= Time.deltaTime;
                }
                break;
            case "Quit":
                if (timeQuit <= 0)
                {
                    Application.Quit();
                    Debug.Log("Quit");

                }
                else
                {
                    timeQuit -= Time.deltaTime;
                }
                break;
        }

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
