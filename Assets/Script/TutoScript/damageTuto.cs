using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damageTuto : MonoBehaviour
{

    [SerializeField] private int hpMax;

    private float hpNow;
    [SerializeField] private GameObject preDead;

    [SerializeField] private int numberCadav;
    [SerializeField] private Transform player;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthBarSec;

    private bool touched;

    [SerializeField] private float timeBar;
    private float chrono;

    // Start is called before the first frame update
    void Start()
    {
        hpNow = hpMax;

        chrono = 0;
        hpNow = hpMax;

        healthBar.maxValue = hpMax;
        healthBar.value = healthBar.maxValue;

        healthBarSec.maxValue = healthBar.maxValue;
        healthBarSec.value = healthBar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        HealthbarDecrease();

        if (hpNow <= 0)
        {
            float écart = -numberCadav / 2;

            Destroy(gameObject);
            for (int i = 1; i <= numberCadav; i++)
            {
                if (transform.position.y <= -10)
                {
                    Instantiate(preDead, new Vector3(player.position.x + 4, player.position.y, player.position.z)
                                         + new Vector3(0, 0, écart * 1.25f),
                        Quaternion.identity, GameObject.Find("CadavreParent").transform);
                }
                else
                {
                    Instantiate(preDead, transform.position + new Vector3(0, 0, écart * 1.25f),
                        Quaternion.identity, GameObject.Find("CadavreParent").transform);
                }
                écart++;
            }

        }
    }

    public void damage(float hit)
    {
        hpNow -= hit;
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
}
