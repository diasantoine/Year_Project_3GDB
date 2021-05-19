using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastraState : State
{

    [FMODUnity.EventRef]
    public string Lastra_Touche = "";

    [Header("Poison")]
    [SerializeField] private float freqTick;
    [HideInInspector] public bool isPoisoned;
    private float chronoPoison;
    [HideInInspector] public float dpsTick;
    private int nbTick;
    private int tickMax = 3;

    void Start()
    {
        this.OnStartAll();
    }

    void Update()
    {
        PoisonDamage();
        HealthbarDecrease();

        if (transform.position.y <= -10)
        {
            HpNow = 0;
        }
        if (HpNow <= 0)
        {
            float écart = -nbCadavre / 2;

            Destroy(gameObject);
            for (int i = 1; i <= nbCadavre; i++)
            {
                if (spawn.ListEnnemy.Contains(this.gameObject))
                {
                    spawn.ListEnnemy.Remove(this.gameObject);
                    if (this.spawn.ListMaxLastra.Contains(this.gameObject))
                    {
                        this.spawn.ListMaxLastra.Remove(this.gameObject);
                    }
                }

                if (Fall)
                {
                    Instantiate(cadavre, player.position, Quaternion.identity, GameObject.Find("CadavreParent").transform);
                    Debug.Log(detectDead.ressourceFloat);
                }
                else
                {
                    Instantiate(cadavre, transform.position + new Vector3(0, 0, écart * 1.25f),
                        Quaternion.identity, GameObject.Find("CadavreParent").transform);
                }
                écart++;
            }
        }
    }

    public override void Damage(float dmg)
    {
        base.Damage(dmg);
        if (!isPoisoned)
        {
            FMODUnity.RuntimeManager.PlayOneShot(Lastra_Touche, "", 0, transform.position);
        }
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
