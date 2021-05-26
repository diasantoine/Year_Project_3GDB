using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaqueScript : MonoBehaviour
{

    //Variable for COLD & HOT
    [SerializeField] private SystemPlaque SP;
    public bool activ;
    [HideInInspector] public bool choosedPlaque;
    [SerializeField] private float activTime;
    private float chronoActiv;
    public Renderer EmiRD;
    public GameObject Particle;


    //Variable for TOXIC
    [SerializeField] private int maxRessourceGot;
    [SerializeField] private int regenRessource;
    [SerializeField] private Light waterLight;
    [HideInInspector] public int ressourceGot;
    [HideInInspector] public bool regenUP;
    private float chronoRess;

    public Color baseColor { get; private set; }
    private float chronoActivation;
    private float intensity;

    public enum Type
    {
        NORMAL,
        HOT,
        COLD,
        TOXIC,
        PISTON

    }

    public Type type;

    private void Start()
    {
        chronoActivation = 0.25f;
        chronoRess = 4;
        intensity = 3.5f;

        if(type == Type.TOXIC)
        {
            ressourceGot = maxRessourceGot;
        }

        if(type == Type.COLD || type == Type.HOT)
        {
            baseColor = EmiRD.material.GetColor("_EmissionColor");
            //Debug.Log(baseColor);
        }
    }


    public void SwitchType(Type newType)
    {
        OnExitType();
        type = newType;
        OnEnterType();
    }

    void OnEnterType()
    {
        switch (type)
        {
            case Type.NORMAL:
                break;
            case Type.HOT:
                break;
            case Type.COLD:
                break;
            case Type.TOXIC:
                break;
            case Type.PISTON:
                break;
        }
    }

    void OnExitType()
    {
        switch (type)
        {
            case Type.NORMAL:
                break;
            case Type.HOT:
                break;
            case Type.COLD:
                break;
            case Type.TOXIC:
                break;
            case Type.PISTON:
                break;

        }
    }

    void OnUpdateType()
    {
        switch (type)
        {
            case Type.NORMAL:
                break;
            case Type.HOT:
                isActive();
                Activation();

                break;
            case Type.COLD:
                isActive();
                Activation();
                break;
            case Type.TOXIC:
                if(ressourceGot < 10 && !regenUP)
                {
                    regenUP = true;
                    ressourceGot = 5;
                    waterLight.intensity = 1;
                    Particle.GetComponent<ParticleSystem>().Stop();
                }

                if (regenUP)
                {
                    if(ressourceGot >= maxRessourceGot)
                    {
                        regenUP = false;
                        ressourceGot = maxRessourceGot;
                        Particle.GetComponent<ParticleSystem>().Play();

                    }
                    else
                    {
                        if(chronoRess >= 2)
                        {
                            ressourceGot += regenRessource;
                            chronoRess = 0;

                        }
                        else
                        {
                            chronoRess += Time.deltaTime;
                        }
                    }
                }
                else
                {
                    Rechargement();
                }
                break;
            case Type.PISTON:
                break;
            
        }
    }

    private void Update()
    {
        OnUpdateType();
    }

    private void Rechargement()
    {
        waterLight.intensity = ressourceGot / 5;
        waterLight.intensity = Mathf.Clamp(waterLight.intensity, 1, 10);
    }

    private void Activation()
    {
        if (choosedPlaque)
        {
            if (chronoActivation <= 0)
            {
                if (intensity == 3.5f)
                {
                    EmiRD.material.SetColor("_EmissionColor", baseColor * intensity);
                    intensity = 1f;

                }
                else if(intensity == 1f)
                {
                    EmiRD.material.SetColor("_EmissionColor", baseColor * intensity);
                    intensity = 3.5f;
                }

                chronoActivation = 0.25f;
            }
            else
            {
                chronoActivation -= Time.deltaTime;
            }
        }
    }

    private void isActive()
    {
        if (activ)
        {
            if (chronoActiv >= activTime)
            {
                activ = false;
                SP.systemActiv = false;
                EmiRD.material.SetColor("_EmissionColor", baseColor);
                intensity = 3.5f;
                chronoActivation = 0.25f;
                if(Particle != null)
                {
                    Particle.SetActive(false);

                }
                chronoActiv = 0;
            }
            else
            {
                chronoActiv += Time.deltaTime;
            }
        }
    }
}
