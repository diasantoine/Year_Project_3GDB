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
        public int NumberBasic;
        public int NumberRuant;
        public int NumberScreamer;
        public int NumberLastra;

    
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
    [Header("Number_Ennemy_Alive")]
    public List<GameObject> ListEnnemy = new List<GameObject>();
    public int mobRestant;
    
    // Start is called before the first frame update

    private void Awake()
    {
        ParentBasic = GameObject.Find("ParentBasic").transform;
        ParentRuant = GameObject.Find("ParentRuant").transform;
        ParentScreamer = GameObject.Find("ParentScreamer").transform;
        ParentLastra = GameObject.Find("ParentLastra").transform;

        DictionnaryEnnemy[Ennemy.Basic] = Resources.Load<GameObject>("MonstreArbre");
        DictionnaryEnnemy[Ennemy.Ruant] = Resources.Load<GameObject>("Ruant");
        DictionnaryEnnemy[Ennemy.Screamer] = Resources.Load<GameObject>("EnnemiMoyen");
        DictionnaryEnnemy[Ennemy.Lastra] = Resources.Load<GameObject>("EnnemiPetit");

    }

    void Start()
    {
        NextWave();
    }

    public void NextWave()
    {
        if(ListWave.Count > IndexWave)
        {
            mobRestant = ListWave[IndexWave].NumberRuant + ListWave[IndexWave].NumberScreamer + ListWave[IndexWave].NumberBasic + ListWave[IndexWave].NumberLastra;
            Debug.Log(mobRestant);

            foreach (Ennemy Ennemy_Type in Enum.GetValues(typeof(Ennemy)))
            {
                StartCoroutine(SpawnEnnemy(Ennemy_Type));
            }
            ++IndexWave;
        }
        else
        {
            gameObject.GetComponent<WaveSystem>().ArenaEnd = true;
        }
    }

    // Update is called once per frame

    IEnumerator SpawnEnnemy(Ennemy EnnemySelectioned)
    {
        WaveStruct Wave = ListWave[IndexWave];
        switch (EnnemySelectioned)
        {
            case Ennemy.Basic:
                for (int i = 0; i < Wave.NumberBasic; i++)
                {
                    yield return new WaitForSeconds(Wave.CD_Spawn_Basic);// le temps de respawn
                    RandomPosition = Random.Range(0, Wave.ListSpawnBasic.Count);
                    GameObject Trh = Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnBasic[RandomPosition].position, Quaternion.identity, ParentBasic);
                    Trh.GetComponent<ennemyState>().spawn = GetComponent<SpawnSysteme>();                 
                    ListEnnemy.Add(Trh);
                    mobRestant--;

                }
                break;
            case Ennemy.Ruant:
                for (int i = 0; i < Wave.NumberRuant; i++)
                {
                    yield return new WaitForSeconds(Wave.CD_Spawn_Ruant);
                    RandomPosition = Random.Range(0, Wave.ListSpawnRuant.Count);
                    GameObject Rut = Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnRuant[RandomPosition].position, Quaternion.identity, ParentRuant);
                    Rut.GetComponent<RuantState>().spawn = GetComponent<SpawnSysteme>();
                    ListEnnemy.Add(Rut);
                    mobRestant--;

                }
                // if (Wave.CD_Spawn_Ruant > 0)
                // {
                //     Wave.CD_Spawn_Ruant -= Time.deltaTime;
                // }
                // else
                // {
                //     RandomPosition = Random.Range(0, Wave.ListSpawnRuant.Count);
                //     Wave.CD_Spawn_Ruant = CD_Spawn_Stock_Ruant;
                //     Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnRuant[RandomPosition].position,
                //         Quaternion.identity, ParentRuant);
                // }
                break;
            case Ennemy.Screamer:
                for (int i = 0; i < Wave.NumberScreamer; i++)
                {
                    yield return new WaitForSeconds(Wave.CD_Spawn_Screamer);
                    RandomPosition = Random.Range(0, Wave.ListSpawnScreamer.Count);
                    GameObject Scm = Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnScreamer[RandomPosition].position, Quaternion.identity, ParentScreamer);
                    Scm.GetComponent<ScreamerScript>().spawn = GetComponent<SpawnSysteme>();
                    ListEnnemy.Add(Scm);
                    mobRestant--;

                }
                break;
            case Ennemy.Lastra:
                for (int i = 0; i < Wave.NumberLastra; i++)
                {
                    yield return new WaitForSeconds(Wave.CD_Spawn_Lastra);
                    RandomPosition = Random.Range(0, Wave.ListSpawnLastra.Count);
                    ListEnnemy.Add(Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnLastra[RandomPosition].position,
                        Quaternion.identity, ParentLastra));
                    mobRestant--;

                }
                break;
            default:
                break;
        }
    }
}
