using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseCharge : skill
{

    [SerializeField] private GameObject preProjo;
    [SerializeField] private Transform canonCharge;

    
    private RaycastHit hit;
    private Camera cam;
    private bool isCharging;
    private GameObject bombe;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public override void UsingSkill()
    {
        isCharging = true;
        bombe = Instantiate(preProjo, canonCharge.position, Quaternion.identity, transform.parent);
        bombe.GetComponent<Rigidbody>().isKinematic = true;
    }

}
