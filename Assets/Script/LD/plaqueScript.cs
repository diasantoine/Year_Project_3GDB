using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaqueScript : MonoBehaviour
{

    //Variable for COLD & HOT
    [SerializeField] private SystemPlaque SP;
    public bool activ;
    [SerializeField] private float activTime;
    private float chrono;

    //Variable for TOXIC
    [SerializeField] private int maxRessourceGot;
    [SerializeField] private int regenRessource;
    [HideInInspector] public int ressourceGot;
    [HideInInspector] public bool regenUP;
    private float chronoRess;

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
        if(type == Type.TOXIC)
        {
            ressourceGot = maxRessourceGot;
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
                break;
            case Type.COLD:
                isActive();
                break;
            case Type.TOXIC:
                if(ressourceGot <= 25)
                {
                    regenUP = true;
                }

                if (regenUP)
                {
                    if(ressourceGot >= maxRessourceGot)
                    {
                        regenUP = false;
                        ressourceGot = maxRessourceGot;
                    }
                    else
                    {
                        if(chrono >= 1)
                        {
                            ressourceGot += regenRessource;
                            chrono = 0;

                        }
                        else
                        {
                            chrono += Time.deltaTime;
                        }
                    }
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

    private void isActive()
    {
        if (activ)
        {
            if (chrono >= activTime)
            {
                activ = false;
                SP.systemActiv = false;
                chrono = 0;
            }
            else
            {
                chrono += Time.deltaTime;
            }
        }
    }
}
