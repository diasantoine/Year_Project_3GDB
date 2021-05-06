using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFeedbackJump : MonoBehaviour
{
    [SerializeField] private GameObject Circle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.localScale = new Vector3(this.Circle.transform.localScale.x * 12, this.Circle.transform.localScale.y * 7.7f, 0);
    }
}
