using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {SPAWNING,WAITING,COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    
    // Keeping track of total number of waves per Spawner
    [SerializeField] private int numWaves;
    // Keeping track of total number of enemies per CombatZone
    [SerializeField] private int numEnemies;

    // The collider that activates a given CombatZone / WaveSpawner area
    private Collider2D activationBox;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private SpawnState state = SpawnState.COUNTING;
    // Keeping track of when each wave is completed in a given Spawner
    [SerializeField] private bool completed;
    private bool beginTheWaves;

    public GameObject boxy;
    public GameObject foos;
    Overhead oh;
    CameraFollow cf;

    // Reference to the LeftBorder of an enclosed CombatZone
    public Collider2D borderL;
    // Reference to the RightBorder of an enclosed CombatZone
    public Collider2D borderR;
    // Firewall particle effects attached to LeftBorder
    private ParticleSystem[] leftFlames;
    // Firewall particle effects attached to RightBorder
    private ParticleSystem[] rightFlames;
    private bool allWavesComplete = false;


    private float maxX;
    // Added minX to allow larger "locked" combat area
    private float minX;

    // Access to nextZone animation moved from CombatZone
    public Animator nextArrow;

    private void Start()
    {
        // Grab number of waves from start
        numWaves = waves.Length;
        // Make sure both borders start off disabled
        borderL.enabled = false;
        borderR.enabled = false;
        leftFlames = borderL.GetComponentsInChildren<ParticleSystem>();
        rightFlames = borderR.GetComponentsInChildren<ParticleSystem>();
        activationBox = GetComponent<Collider2D>();

        foreach(Transform sp in spawnPoints)
        {
            foreach(Wave leWave in waves)
            {
                numEnemies += leWave.count;
            }
        }
        
        cf = FindObjectOfType<CameraFollow>();
        oh = FindObjectOfType<Overhead>();
        //oh.SetActive(false);
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("no spawn points foo");
        }

        waveCountDown = timeBetweenWaves;
        maxX = cf.XMaxValue;
        minX = cf.XMinValue;
    }

    private void Update()
    {
        // Need a way to check if all enemies in a given "zone" are defeated
        if (allWavesComplete)
        {
            nextArrow.enabled = true;
            nextArrow.SetTrigger("allClear");
            StartCoroutine(SelfDestruct());
            return;
        }

        if (beginTheWaves)
        {
            if (state == SpawnState.WAITING)
            {
                if (!EnemyIsAlive())
                {
                    WaveCompleted();
                }
            }

            if (waveCountDown <= 0 && completed)
            {
                if (state != SpawnState.SPAWNING)
                {
                    StopAllCoroutines();
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }

            if (state == SpawnState.COUNTING)
            {
                waveCountDown -= Time.deltaTime;
            }
        }
    }

    IEnumerator SelfDestruct()
    {
        borderL.enabled = false;
        borderR.enabled = false;
        boxy.gameObject.SetActive(false);

        yield return new WaitForSeconds(5f);
        nextArrow.enabled = false;
        Destroy(this.gameObject);
    }

    void WaveCompleted()
    {
        Debug.Log("wave completed");
        waveCountDown = timeBetweenWaves;
        completed = true;

        // Decrement every time a wave is completed
        numWaves--;
        state = SpawnState.COUNTING;
        
        // Check if all waves have been completed
        if(numWaves <= 0)
        {
            allWavesComplete = true;
            cf.XMaxValue = maxX;
            cf.XMinValue = minX;
            return;
        }
    }

    bool EnemyIsAlive()
    {
        //searchCountDown = 1f;
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            Debug.Log("all done");

            return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        completed = false;
        // foos.SetActive(false);

        Debug.Log("Spawning wave:" + _wave.name);
        state = SpawnState.SPAWNING;

        foreach(Transform sp in spawnPoints)
        {
            for (int i = 0; i < _wave.count; i++)
            {
                SpawnEnemy(_wave.enemy);
                yield return new WaitForSeconds(1f / _wave.rate);
            }
        }
        state = SpawnState.WAITING;
        if(nextWave + 1 >= waves.Length)
        {
            yield break;
        }
        nextWave++;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !allWavesComplete)
        {
            if(oh != null)
                oh.SetOverhead(this, numWaves);
            Debug.Log("Total number of enemies on this floor: " + numEnemies);
            StartCoroutine(SpawnWave(waves[nextWave])); 
            borderL.enabled = true;     // Turns on left wall of combat area
            borderR.enabled = true;     // Turns on right wall of combat area

            if (boxy != null)
            {
                //manipulate camera values
                cf.XMinValue = borderL.transform.position.x + 5;
                cf.XMaxValue = borderR.transform.position.x - 5;
            }

            // Activates magical flame walls to confine player
            foreach (ParticleSystem ps in leftFlames)
            {
                ps.Play();
            }
            foreach (ParticleSystem ps in rightFlames)
            {
                ps.Play();
            }

            activationBox.enabled = false;
            beginTheWaves = true;
        }
    }
}
