using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticulePoint : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 position = Input.mousePosition;

        transform.position = position;
    }
}
