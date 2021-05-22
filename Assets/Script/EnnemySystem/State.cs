using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour
{
    [Header("HealthSystem")]
    [SerializeField] private protected float HpMax;
    private protected float HpNow;
    [SerializeField] private protected Slider healthBar;
    [SerializeField] private protected Slider healthBarSec;
    [SerializeField] private protected float timeBar;
    private protected float chronoBar;
    private protected bool touched;

    [Header("Death")]
    [SerializeField] private protected GameObject cadavre;
    [SerializeField] private protected int nbCadavre;
    private protected bool Fall = false;

    [Header("Essential")]
    public Transform player;
    public SpawnSysteme spawn;
    public bool isWeak;


    private protected void OnStartAll()
    {
        player = GameObject.Find("Player").transform;

        chronoBar = 0;
        HpNow = HpMax;

        healthBar.maxValue = HpMax;
        healthBar.value = healthBar.maxValue;

        healthBarSec.maxValue = healthBar.maxValue;
        healthBarSec.value = healthBar.maxValue;
    }

    public virtual void Damage(float dmg)
    {
        HpNow -= dmg;
        healthBar.gameObject.SetActive(true);
        healthBar.value = HpNow;
        touched = true;
        chronoBar = 0;
    }


}
