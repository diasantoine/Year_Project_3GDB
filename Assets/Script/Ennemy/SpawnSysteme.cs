using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpawnSysteme : MonoBehaviour
{
    public enum Ennemy
    {
        Basic,
        Ruant,
        Screamer,
        Lastra
    }
    
    [System.Serializable] 
    public struct WaveStruct
    {
        [Header("EnnemyType")]
        public List<Ennemy> ListEnnemy;
    
    
        [Header("EnnemyPosition")]
        public List<Transform> ListSpawnBasic;
        public List<Transform> ListSpawnRuant;
        public List<Transform> ListSpawnScreamer;
        public List<Transform> ListSpawnLastra;
        
        [Header("EnnemyStat")]

        public float CD_Spawn_Basic;
        public float CD_Spawn_Ruant;
        public float CD_Spawn_Screamer;
        public float CD_Spawn_Lastra;

    }
    
    [SerializeField] private List<WaveStruct> ListWave = new List<WaveStruct>();
    
    private Dictionary<Ennemy,GameObject> DictionnaryEnnemy = new Dictionary<Ennemy, GameObject>();
    
    private Transform ParentBasic;
    private Transform ParentRuant;
    private Transform ParentScreamer;
    private Transform ParentLastra;

    private float CD_Spawn_Stock_Basic;
    private float CD_Spawn_Stock_Ruant;
    private float CD_Spawn_Stock_Screamer;
    private float CD_Spawn_Stock_Lastra;

    private int RandomPosition = 0;

    [Header("WaveIndex")]
    public int IndexWave = 0;
    
    // Start is called before the first frame update

    private void Awake()
    {
        ParentBasic = GameObject.Find("ParentBasic").transform;
        ParentRuant = GameObject.Find("ParentRuant").transform;
        ParentScreamer = GameObject.Find("ParentScreamer").transform;
        ParentLastra = GameObject.Find("ParentLastra").transform;

        DictionnaryEnnemy[Ennemy.Basic] = Resources.Load<GameObject>("MonstreArbre");
        // DictionnaryEnnemy[Ennemy.Ruant] = Resources.Load<GameObject>("3D/Ennemy/EnnemiGrand");
        // DictionnaryEnnemy[Ennemy.Screamer] = Resources.Load<GameObject>("3D/Ennemy/EnnemiGrand");
        // DictionnaryEnnemy[Ennemy.Lastra] = Resources.Load<GameObject>("3D/Ennemy/EnnemiGrand");

    }

    void Start()
    {
        StartCoroutine(NextWave());
    }

    // Update is called once per frame

    IEnumerator NextWave()
    {
         WaveStruct Wave = ListWave[IndexWave];
        ++IndexWave;
        foreach (var EnnemySelectioned in Wave.ListEnnemy)
        {
            switch (EnnemySelectioned)
            {
                case Ennemy.Basic:
                    //Debug.Log(Wave);
                    yield return new WaitForSeconds(Wave.CD_Spawn_Basic);
                    //Wave.CD_Spawn_Basic = CD_Spawn_Stock_Basic; ta geule
                    Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnBasic[RandomPosition].position,
                        Quaternion.identity, ParentBasic);
                    break;
                case Ennemy.Ruant:
                    yield return new WaitForSeconds(Wave.CD_Spawn_Basic);
                    if (Wave.CD_Spawn_Ruant > 0)
                    {
                        Wave.CD_Spawn_Ruant -= Time.deltaTime;
                    }
                    else
                    {
                        RandomPosition = Random.Range(0, Wave.ListSpawnRuant.Count);
                        Wave.CD_Spawn_Ruant = CD_Spawn_Stock_Ruant;
                        Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnRuant[RandomPosition].position,
                            Quaternion.identity, ParentRuant);
                    }

                    break;
                case Ennemy.Screamer:
                    if (Wave.CD_Spawn_Screamer > 0)
                    {
                        Wave.CD_Spawn_Screamer -= Time.deltaTime;
                    }
                    else
                    {
                        RandomPosition = Random.Range(0, Wave.ListSpawnScreamer.Count);
                        Wave.CD_Spawn_Screamer = CD_Spawn_Stock_Screamer;
                        Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnScreamer[RandomPosition].position,
                            Quaternion.identity, ParentScreamer);
                    }

                    break;
                case Ennemy.Lastra:
                    if (Wave.CD_Spawn_Lastra > 0)
                    {
                        Wave.CD_Spawn_Lastra -= Time.deltaTime;
                    }
                    else
                    {
                        RandomPosition = Random.Range(0, Wave.ListSpawnLastra.Count);
                        Wave.CD_Spawn_Lastra = CD_Spawn_Stock_Lastra;
                        Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnLastra[RandomPosition].position,
                            Quaternion.identity, ParentLastra);
                    }

                    break;
                default:
                    break;
            }
        }
    }
    void Update()
    {
     
    }
}
