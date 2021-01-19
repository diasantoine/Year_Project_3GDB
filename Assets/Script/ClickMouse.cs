using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMouse : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Player.transform.position.y, transform.position.z);
    }
}
