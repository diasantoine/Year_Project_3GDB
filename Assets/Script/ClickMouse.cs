using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMouse : MonoBehaviour
{
    [SerializeField] private GameObject Canon;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Canon.transform.position.y, transform.position.z);
    }
}
