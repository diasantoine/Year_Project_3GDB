using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamerState : State
{
    [Header("Son")]
    [FMODUnity.EventRef]
    public string Screamer_Touche = "";

    [Header("Poison")]
    [SerializeField] private float freqTick;
    public bool isPoisoned;
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
        PoisonDamage();
        HealthbarDecrease();

        if (HpNow <= 0)
        {
            if (!Fall)
            {
                if (gameObject.GetComponent<ScreamerAI>().state != ScreamerAI.State.dead)
                {
                    gameObject.GetComponent<ScreamerAI>().SwitchState(ScreamerAI.State.dead);
                    Destroy(healthBar.gameObject);

                }

            }
            else
            {
                Die();
            }


        }

        if (transform.position.y <= -10)
        {
            HpNow = 0;
        }     

    }

    public override void Damage(float dmg)
    {
        if(HpNow > 0)
        {
            base.Damage(dmg);

            if (isPoisoned)
            {
                nbTick = 0;
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(Screamer_Touche, "", 0, transform.position);

            }
        }

    }

    public void Die()
    {
        if (!isPoisoned)
        {
            this.GetComponent<ScreamerAI>().ImpulsionTahLesfous();

        }

        if(spawn != null)
        {
            if (spawn.ListEnnemy.Contains(this.gameObject))
            {
                spawn.ListEnnemy.Remove(this.gameObject);
                if (this.spawn.ListMaxScreamer.Contains(this.gameObject))
                {
                    this.spawn.ListMaxScreamer.Remove(this.gameObject);
                }
            }

        }

        float écart = -nbCadavre / 2;

        for (int i = 1; i <= nbCadavre; i++)
        {

            if (Fall)
            {
                if(player != null)
                {
                    Instantiate(cadavre, player.position, Quaternion.identity, GameObject.Find("CadavreParent").transform);
                    Debug.Log(detectDead.ressourceFloat);
                }
            }
            else
            {
                Instantiate(cadavre, transform.position + new Vector3(0, 0, écart * 1.25f),
                    Quaternion.identity, GameObject.Find("CadavreParent").transform);
            }
            écart++;
        }

        Destroy(gameObject, 1f);

    }

    private void PoisonDamage()
    {
        if(HpNow > 0)
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
