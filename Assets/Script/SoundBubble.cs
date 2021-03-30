using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBubble : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Bubbling_Mud = "";

    public ParticleSystem ps;
    // FMODUnity.RuntimeManager.PlayOneShot(Bubbling_Mud, transform.position);

    private void Update()
    {
        if(ps.time >= ps.main.duration - 0.005f)
        {
            Debug.Log("test");
            FMODUnity.RuntimeManager.PlayOneShot(Bubbling_Mud, transform.position);
        }
    }
}
