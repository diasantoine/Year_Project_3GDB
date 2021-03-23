using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plaqueScript : MonoBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
