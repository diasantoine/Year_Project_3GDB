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

        [Header("EnnemyMaxNumber")] 
        public int MaxBasic;
        public int MaxRuant;
        public int MaxScreamer;
        public int MaxLastra;
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
    
    [HideInInspector] public List<GameObject> ListMaxBasic;
    [HideInInspector] public List<GameObject> ListMaxRuant;
    [HideInInspector] public List<GameObject> ListMaxScreamer;
    [HideInInspector] public List<GameObject> ListMaxLastra;

    private int NumberOfBasicThisWawe = 0;
    private int NumberOfRuantThisWawe = 0;
    private int NumberOfScreamerThisWawe = 0;
    private int NumberOfLastraThisWawe = 0;

    private int NumberOfBasicNotSpawned;
    private int NumberOfRuantNotSpawned;
    private int NumberOfScreamerNotSpawned;
    private int NumberOfLastraNotSpawned;



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

        DictionnaryEnnemy[Ennemy.Basic] = Resources.Load<GameObject>("Basic");
        DictionnaryEnnemy[Ennemy.Ruant] = Resources.Load<GameObject>("Ruant");
        DictionnaryEnnemy[Ennemy.Screamer] = Resources.Load<GameObject>("Screamer");
        DictionnaryEnnemy[Ennemy.Lastra] = Resources.Load<GameObject>("Lastra");
    }

    void Start()
    {
        NextWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.NextWave();
        }

        
    }

    public void NextWave()
    {
        if(ListWave.Count > IndexWave)
        {
            if (this.mobRestant != 0)
            {
                foreach (Ennemy Ennemy_Type in Enum.GetValues(typeof(Ennemy)))
                {
                    this.StopCoroutine(SpawnEnnemy(Ennemy_Type));
                }
                this.mobRestant += ListWave[IndexWave].NumberRuant + ListWave[IndexWave].NumberScreamer + ListWave[IndexWave].NumberBasic + ListWave[IndexWave].NumberLastra;
                this.NumberOfBasicThisWawe = this.ListWave[this.IndexWave].NumberBasic + this.NumberOfBasicNotSpawned;
                this.NumberOfRuantThisWawe = this.ListWave[this.IndexWave].NumberRuant + this.NumberOfRuantNotSpawned;
                this.NumberOfScreamerThisWawe = this.ListWave[this.IndexWave].NumberScreamer + this.NumberOfScreamerNotSpawned;
                this.NumberOfLastraThisWawe = this.ListWave[this.IndexWave].NumberLastra + this.NumberOfLastraNotSpawned;
                //Debug.Log(this.mobRestant + " " + this.NumberOfBasicThisWawe +" " + this.NumberOfBasicNotSpawned);
            }
            else
            {
                mobRestant = ListWave[IndexWave].NumberRuant + ListWave[IndexWave].NumberScreamer + ListWave[IndexWave].NumberBasic + ListWave[IndexWave].NumberLastra;
                this.NumberOfBasicThisWawe = this.ListWave[this.IndexWave].NumberBasic;
                this.NumberOfRuantThisWawe = this.ListWave[this.IndexWave].NumberRuant;
                this.NumberOfScreamerThisWawe = this.ListWave[this.IndexWave].NumberScreamer;
                this.NumberOfLastraThisWawe = this.ListWave[this.IndexWave].NumberLastra;
            }
            foreach (Ennemy Ennemy_Type in Enum.GetValues(typeof(Ennemy)))
            {
                StartCoroutine(SpawnEnnemy(Ennemy_Type));
            }
            ++IndexWave;
        }
        else
        {
            Debug.Log("Fin d'arene");
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
                for (int i = 0; i < this.NumberOfBasicThisWawe; i++)
                {
                    yield return new WaitUntil(() => this.ListMaxBasic.Count < Wave.MaxBasic);
                    yield return new WaitForSeconds(Wave.CD_Spawn_Basic);// le temps de respawn
                    RandomPosition = Random.Range(0, Wave.ListSpawnBasic.Count);
                    GameObject Trh = Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnBasic[RandomPosition].position, Quaternion.identity, ParentBasic);
                    Trh.GetComponent<State>().spawn = GetComponent<SpawnSysteme>();                 
                    ListEnnemy.Add(Trh);
                    this.ListMaxBasic.Add(Trh);
                    mobRestant--;
                    this.NumberOfBasicNotSpawned = Wave.NumberBasic - i - 1;
                }
                break;
            case Ennemy.Ruant:
                for (int i = 0; i < this.NumberOfRuantThisWawe; i++)
                {
                    yield return new WaitUntil(() => this.ListMaxRuant.Count < Wave.MaxRuant);
                    yield return new WaitForSeconds(Wave.CD_Spawn_Ruant);
                    RandomPosition = Random.Range(0, Wave.ListSpawnRuant.Count);
                    GameObject Rut = Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnRuant[RandomPosition].position, Quaternion.identity, ParentRuant);
                    Rut.GetComponent<RuantState>().spawn = GetComponent<SpawnSysteme>();
                    ListEnnemy.Add(Rut);
                    this.ListMaxRuant.Add(Rut);
                    mobRestant--;
                    this.NumberOfRuantNotSpawned = Wave.NumberRuant - i - 1;
                }
                break;
            case Ennemy.Screamer:
                for (int i = 0; i < this.NumberOfScreamerThisWawe; i++)
                {
                    yield return new WaitUntil(() => this.ListMaxScreamer.Count < Wave.MaxScreamer);
                    yield return new WaitForSeconds(Wave.CD_Spawn_Screamer);
                    RandomPosition = Random.Range(0, Wave.ListSpawnScreamer.Count);
                    GameObject Scm = Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnScreamer[RandomPosition].position, Quaternion.identity, ParentScreamer);
                    Scm.GetComponent<ScreamerAI>().spawn = GetComponent<SpawnSysteme>();
                    ListEnnemy.Add(Scm);
                    this.ListMaxScreamer.Add(Scm);
                    mobRestant--;
                    this.NumberOfScreamerNotSpawned = Wave.NumberScreamer - i - 1;
                }
                break;
            case Ennemy.Lastra:
                for (int i = 0; i < this.NumberOfLastraThisWawe; i++)
                {
                    yield return new WaitUntil(() => this.ListMaxLastra.Count < Wave.MaxLastra);
                    yield return new WaitForSeconds(Wave.CD_Spawn_Lastra);
                    RandomPosition = Random.Range(0, Wave.ListSpawnLastra.Count);
                    GameObject Lst = Instantiate(DictionnaryEnnemy[EnnemySelectioned], Wave.ListSpawnLastra[RandomPosition].position, Quaternion.identity, ParentLastra);
                    Lst.GetComponent<LastraAI>().spawn = this.GetComponent<SpawnSysteme>();
                    ListEnnemy.Add(Lst);
                    this.ListMaxLastra.Add(Lst);
                    mobRestant--;
                    this.NumberOfLastraNotSpawned = Wave.NumberLastra - i - 1;// car on commence à 0, ce qui veut dire que même quand il y a plus de lastra a spawn ça ferait 1 ce calcul sans le -1
                }
                break;
            default:
                break;
        }
    }
}
