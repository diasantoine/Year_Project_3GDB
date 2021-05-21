using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBasic : MonoBehaviour
{
    [SerializeField] private BasicAI ContainerBasicAI;

    [SerializeField] private float TimeWhereTheColliderActivate;

    private float ContainerTimeWhereTheColliderActivate;

    private float TimeBeforeColliderDesactivate;
    // Start is called before the first frame update
    void Start()
    {
        this.ContainerTimeWhereTheColliderActivate = this.TimeWhereTheColliderActivate;
        this.TimeBeforeColliderDesactivate = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ContainerBasicAI.InPunch &&  !this.GetComponent<Collider>().enabled)
        {
            if (this.TimeWhereTheColliderActivate <= 0)
            {
                this.TimeWhereTheColliderActivate = this.ContainerTimeWhereTheColliderActivate;
                this.GetComponent<Collider>().enabled = true;
                StartCoroutine(this.DesactivationCollider());
            }
            else
            {
                this.TimeWhereTheColliderActivate -= Time.deltaTime;
            }
        }
        else
        {
            this.TimeWhereTheColliderActivate = this.ContainerTimeWhereTheColliderActivate;
        }
    }

    IEnumerator DesactivationCollider()
    {
        yield return new WaitForSeconds(this.TimeBeforeColliderDesactivate);
        this.GetComponent<Collider>().enabled = false;
        this.ContainerBasicAI.InPunch = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player") && this.ContainerBasicAI.InPunch)
        {
            if (!other.transform.parent.GetComponent<The_Player_Script>().JustHit)
            {
                this.ContainerBasicAI.PatateDansLeJoueur();
            }
        }
    }
}
