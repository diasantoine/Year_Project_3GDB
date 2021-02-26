using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{

    [HideInInspector] public bool goGoGo;
    [HideInInspector] public int charged;

    [SerializeField] private int hitDmg;

    // Start is called before the first frame update
    void Start()
    {
        goGoGo = false;
        charged = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (goGoGo)
        {
            Vector3 localZ = transform.parent.localScale;
            localZ.z = charged * localZ.z;
            transform.parent.localScale = new Vector3(transform.parent.localScale.x, transform.parent.localScale.y, localZ.z);
            Destroy(transform.parent.gameObject, 0.5f);
            goGoGo = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {
            Debug.Log("touched");
        }
    }
}
