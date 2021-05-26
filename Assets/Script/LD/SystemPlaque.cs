using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemPlaque : MonoBehaviour
{

    public List<plaqueScript> enablePlaques;
    [SerializeField] private List<plaqueScript> disablePlaques;

    [SerializeField] private float setActivTime;
    private float chrono;

    [HideInInspector] public bool systemActiv;
    [SerializeField] private float intensity;

    private int randomChoose;
    private bool plaqueChoosed;

    public float pulse;
    private plaqueScript nowPlaque;

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

            if(chrono >= setActivTime * 0.25f)
            {
                if (!plaqueChoosed)
                {
                    randomChoose = Random.Range(0, enablePlaques.Count);
                    nowPlaque = enablePlaques[randomChoose];
                    nowPlaque.choosedPlaque = true;

                    plaqueChoosed = true;
                }                         

            }
            if (chrono >= setActivTime)
            {
                systemActiv = true;
                nowPlaque.choosedPlaque = false;
                nowPlaque.activ = true;
               
                nowPlaque.EmiRD.material.SetColor("_EmissionColor", nowPlaque.baseColor * intensity);  //(intensity + Mathf.Sin(Time.time) * pulse));
                if(nowPlaque.Particle != null)
                {
                    nowPlaque.Particle.SetActive(true);

                }

                enablePlaques.Remove(nowPlaque);

                if(disablePlaques.Count > 0)
                {
                    enablePlaques.Add(disablePlaques[0]);
                    disablePlaques.Remove(disablePlaques[0]);
                }

                disablePlaques.Add(nowPlaque);
                plaqueChoosed = false;

                chrono = 0;
            }
            else
            {
                chrono += Time.deltaTime;
            }

           
        }
    }
}
