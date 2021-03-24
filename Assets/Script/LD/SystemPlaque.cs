﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPlaque : MonoBehaviour
{

    public List<plaqueScript> enablePlaques;
    [SerializeField] private List<plaqueScript> disablePlaques;

    [SerializeField] private float setActivTime;
    private float chrono;

    [HideInInspector] public bool systemActiv;

    // Start is called before the first frame update
    void Start()
    {
        disablePlaques = new List<plaqueScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!systemActiv)
        {
            if (chrono >= setActivTime)
            {
                systemActiv = true;

                int randomChoose = Random.Range(0, enablePlaques.Count);

                plaqueScript nowPlaque = enablePlaques[randomChoose];
                nowPlaque.activ = true;
                enablePlaques.Remove(nowPlaque);

                if(disablePlaques.Count > 0)
                {
                    enablePlaques.Add(disablePlaques[0]);
                    disablePlaques.Remove(disablePlaques[0]);
                }

                disablePlaques.Add(nowPlaque);

                chrono = 0;
            }
            else
            {
                chrono += Time.deltaTime;
            }
        }
    }
}
