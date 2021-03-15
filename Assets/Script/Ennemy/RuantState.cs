using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuantState : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthBarSec;
    [SerializeField] private float timeBar;
    private float chrono;
    private bool touched;

    public Transform player;
    [SerializeField] private GameObject preRessource;

    [SerializeField] private float HpMax;
    [SerializeField] private int nbCadav;
    private float hpNow;

    private bool Fall;
    public bool isWeak;

    [HideInInspector] public spawnEnnemyBasique SEB;
    public SpawnSysteme spawn;

    [SerializeField] private float weakPoint;

    [FMODUnity.EventRef]
    public string Ruant_Touche_O = "";
    // FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_O, transform.position); // son degat

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;

        hpNow = HpMax;
        healthBar.maxValue = HpMax;
        healthBar.value = healthBar.maxValue;

        healthBarSec.maxValue = healthBar.maxValue;
        healthBarSec.value = healthBar.maxValue;

        chrono = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Weakling();

        if(hpNow <= 0)
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
        }

        float écart = -nbCadav / 2;


        for (int i = 1; i <= nbCadav; i++)
        {
            if (Fall)
            {
                Instantiate(preRessource, player.position, Quaternion.identity, GameObject.Find("CadavreParent").transform);
            }
            else
            {
                Instantiate(preRessource, transform.position + new Vector3(0, 0, écart * 1.25f),
                    Quaternion.identity, GameObject.Find("CadavreParent").transform);
            }
            écart++;
        }

        if (SEB != null)
        {
            SEB.numberEnnemy--;

        }
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("DeathFall"))
        {
            hpNow = 0;
            Fall = true;
        }
    }

    public void takeDmg(float damage)
    {
        hpNow -= damage;
        FMODUnity.RuntimeManager.PlayOneShot(Ruant_Touche_O, transform.position); // son degat
        healthBar.value = hpNow;
        touched = true;
        chrono = 0;
    }

    void HealthbarDecrease()
    {
        if (touched)
        {
            if (chrono >= timeBar)
            {
                healthBarSec.value -= 1.5f * Time.deltaTime;

                if (healthBarSec.value <= healthBar.value)
                {
                    chrono = 0;
                    touched = false;
                }
            }
            else
            {
                chrono += Time.deltaTime;
            }
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
}
