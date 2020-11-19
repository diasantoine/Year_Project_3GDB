using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemyState : MonoBehaviour
{

    private Transform player;

    public bool moving;
    [HideInInspector] public spawnEnnemyBasique SEB;

    [SerializeField] private GameObject preDead;

    private float hpNow;

    [SerializeField] private float hpMax;
    [SerializeField] private float vitesse;

    [SerializeField] private int numberCadav;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        hpNow = hpMax;

        numberCadav = Random.Range(1, 4);

    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector3 distance = player.position - transform.position;
            distance = distance.normalized;

            transform.position += distance * vitesse * Time.deltaTime;
            transform.LookAt(player.position);

        }

        if (hpNow <= 0)
        {
            float écart = -numberCadav / 2;

            Destroy(gameObject);
            for (int i = 1; i <= numberCadav; i++)
            {
                Instantiate(preDead, transform.position + new Vector3(0, 0, écart * 1.25f), Quaternion.identity, GameObject.Find("CadavreParent").transform);
                écart++;
            }

            SEB.numberEnnemy--;
        }
    }

    public void damage(float hit)
    {
        hpNow -= hit;
    }
}
