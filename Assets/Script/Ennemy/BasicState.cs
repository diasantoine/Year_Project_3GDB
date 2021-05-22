using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicState : State
{

    [FMODUnity.EventRef]
    public string Basic_Touche = "";

    [Header("Poison")]
    [SerializeField] private float freqTick;
    [HideInInspector] public bool isPoisoned;
    private float chronoPoison;
    [HideInInspector] public float dpsTick;
    private int nbTick;
    private int tickMax = 3;

    private void Start()
    {

        OnStartAll();   

        chronoPoison = 0;
        nbTick = 0;

    }

    // Update is called once per frame
    void Update()
    {
        DebugDeath();
        PoisonDamage();
        HealthbarDecrease();

        if (HpNow <= 0)
        {
            if (!Fall)
            {
                if (gameObject.GetComponent<BasicAI>().state != BasicAI.State.DEATH)
                {
                    gameObject.GetComponent<BasicAI>().SwitchState(BasicAI.State.DEATH);
                    Destroy(healthBar.gameObject);

                }

            }
            else
            {
                Die();
            }


        }

    }

    public override void Damage(float dmg)
    {
        base.Damage(dmg);
        if (!isPoisoned)
        {
            FMODUnity.RuntimeManager.PlayOneShot(Basic_Touche, "", 0, transform.position);
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

    private void PoisonDamage()
    {
        if (isPoisoned)
        {
            if (chronoPoison >= freqTick)
            {

                Damage(dpsTick);
                nbTick++;
                chronoPoison = 0;

                if (nbTick >= tickMax)
                {
                    nbTick = 0;
                    isPoisoned = false;
                }

            }
            else
            {
                chronoPoison += Time.deltaTime;
            }
        }
    }

    void HealthbarDecrease()
    {
        if (touched)
        {
            if (chronoBar >= timeBar)
            {
                healthBarSec.value -= 3f * Time.deltaTime;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("DeathFall"))
        {
            Fall = true;
            HpNow = 0;
        }
    }

    private void DebugDeath()
    {
        if (gameObject.transform.position.y <= -10)
        {
            Fall = true;
            Die();
        }
    }
}
