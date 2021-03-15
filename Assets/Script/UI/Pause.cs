using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public static bool isPause = false;

    
    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void StopGame()
    {
        if (isPause)
        {
            Resume();
        }
        else
        {
            OnPause();
        }
    }


    void OnPause()
    {
        Time.timeScale = 0f;
        isPause = true;

    }

    void Resume()
    {
        Time.timeScale = 1f;
        isPause = false;
    }
}
