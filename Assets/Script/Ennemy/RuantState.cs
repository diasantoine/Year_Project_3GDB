using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuantState : State
{

    [Header("Speciality")]
    [SerializeField] private float weakPoint;
    [SerializeField] private Renderer rd;
    [SerializeField] private float intensity;

    private float chrono;
    private Color colorIni;
    private float intensityIni;

    [FMODUnity.EventRef]
    public string Ruant_Touche_O = "";

    // Start is called before the first frame update
    void Start()
    {
        OnStartAll();
        colorIni = rd.material.GetColor("_EmissionColor");
        intensityIni = intensity;
    }

    // Update is called once per frame
    void Update()
    {
        DebugDeath();
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

    private void TouchedFeedback()
    {
        if(intensity > 1)
        {
            intensity -= 10f * Time.deltaTime;
            intensity = Mathf.Clamp(intensity, 1, intensityIni);
            rd.material.SetColor("_EmissionColor", colorIni * (intensity));
            Debug.Log(intensity);
        }
    }


    public override void Damage(float dmg)
    {
        base.Damage(dmg);
        intensity = intensityIni;
        FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_O, "", 0, transform.position);  
        var color = colorIni;
        rd.material.SetColor("_EmissionColor", color * (intensity + 0.5f));  //(intensity + Mathf.Sin(Time.time) * pulse));

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
            TouchedFeedback();
            if (chronoBar >= timeBar)
            {
                healthBarSec.value -= 3f * Time.deltaTime;

                if (healthBarSec.value <= healthBar.value)
                {
                    rd.material.SetColor("_EmissionColor", colorIni);
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

    private void DebugDeath()
    {
        if (gameObject.transform.position.y <= -10)
        {
            Fall = true;
            Die();
        }
    }
}
