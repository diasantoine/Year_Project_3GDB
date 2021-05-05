using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFeedbackJump : MonoBehaviour
{
    [SerializeField] private GameObject Circle;

    [SerializeField] private GameObject Canon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, this.Canon.transform.localPosition.y, transform.position.z);
        
    }
}
