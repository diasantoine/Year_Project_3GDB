using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaqueScript : MonoBehaviour
{


    public bool activ;
    [SerializeField] private SystemPlaque SP;

    [SerializeField] private float activTime;
    private float chrono;

    public enum Type
    {
        NORMAL,
        HOT,
        COLD,
        TOXIC,
        PISTON

    }

    public Type type;

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
            default:
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
            default:
                break;
        }
    }

    private void Update()
    {
        if (activ)
        {
            if(type == Type.COLD || type == Type.HOT)
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
}
