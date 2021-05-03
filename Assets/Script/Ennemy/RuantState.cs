using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuantState : State
{

    [Header("Speciality")]
    [SerializeField] private float weakPoint;
    [SerializeField] private Renderer rd;

    [FMODUnity.EventRef]
    public string Ruant_Touche_O = "";

    // Start is called before the first frame update
    void Start()
    {
        OnStartAll();
    }

    // Update is called once per frame
    void Update()
    {
        Weakling();
        HealthbarDecrease();

        if(HpNow <= 0)
        {
            if (!Fall)
            {
                if(gameObject.GetComponent<RuantAI>().state != RuantAI.State.DEATH)
                {
                    gameObject.GetComponent<RuantAI>().SwitchState(RuantAI.State.DEATH);
                    Destroy(healthBar.gameObject);

                }

            }
            else
            {
                Die();
            }

            
        }

    }

    public void Die()
    {
        if (spawn.ListEnnemy.Contains(this.gameObject))
        {
            spawn.ListEnnemy.Remove(this.gameObject);
            if (this.spawn.ListMaxRuant.Contains(this.gameObject))
            {
                this.spawn.ListMaxRuant.Remove(this.gameObject);
            }
        }

        float écart = -nbCadavre / 2;


        for (int i = 1; i <= nbCadavre; i++)
        {
            if (Fall)
            {
                Instantiate(cadavre, player.position, Quaternion.identity, GameObject.Find("CadavreParent").transform);
            }
            else
            {
                Instantiate(cadavre, transform.position + new Vector3(0, 0, écart * 1.25f),
                    Quaternion.identity, GameObject.Find("CadavreParent").transform);
            }
            écart++;
        }

        Destroy(gameObject);
    }


    public override void Damage(float dmg)
    {
        base.Damage(dmg);
        FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_O, "", 0, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("DeathFall"))
        {
            Fall = true;
            HpNow = 0;
        }
    }

    private void Weakling()
    {
        Vector3 playerDirection = player.position - transform.position;

        playerDirection = playerDirection.normalized;

        Debug.DrawRay(transform.position, playerDirection, Color.blue);
        Debug.DrawRay(transform.position, transform.forward, Color.magenta);

        float dot = Vector3.Dot(playerDirection, -transform.forward);

        if (dot > weakPoint)
        {
            isWeak = true;
        }
        else
        {
            isWeak = false;
        }
    }

    void HealthbarDecrease()
    {
        if (touched)
        {
            if (chronoBar >= timeBar)
            {
                healthBarSec.value -= 1.5f * Time.deltaTime;

                if (healthBarSec.value <= healthBar.value)
                {
                    chronoBar = 0;
                    touched = false;
                }
            }
            else
            {
                chronoBar += Time.deltaTime;
            }
        }

    }
}
