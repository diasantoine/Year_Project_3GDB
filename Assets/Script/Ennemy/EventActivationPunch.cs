using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivationPunch : MonoBehaviour
{
    public bool PunchActivate;

    public void ActivationPunch()
    {
        this.PunchActivate = true;
    }

    public void DesactivationPunch()
    {
        this.PunchActivate = false;
    }
}
