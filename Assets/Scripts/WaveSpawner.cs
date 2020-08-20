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

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountDown;

    private float searchCountDown = 1f;

    private SpawnState state = SpawnState.COUNTING;
    public bool completed { get; private set; }
    bool beginTheWaves;
    //public bool allSpawned { get; private set; }

    public GameObject boxy;
    public GameObject foos;
    Overhead oh;
    CameraFollow cf;

    public Collider2D borderL;
    public Collider2D borderR;
    private ParticleSystem[] leftFlames;
    private ParticleSystem[] rightFlames;
    private bool allWavesComplete = false;


    private float maxX;
    // Added minX to allow larger "locked" combat area
    private float minX;

    // Access to nextZone animation moved from CombatZone
    public Animator nextArrow;

    private void Start()
    {
        borderL.enabled = false;
        borderR.enabled = false;
        leftFlames = borderL.GetComponentsInChildren<ParticleSystem>();
        rightFlames = borderR.GetComponentsInChildren<ParticleSystem>();

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
            Destroy(borderL.gameObject);
            Destroy(borderR.gameObject);

            Destroy(nextArrow.gameObject, 30f);
        }
        if (beginTheWaves)
        {
            if (state == SpawnState.WAITING)
            {
                if (!EnemyIsAlive())
                {
                    WaveCompleted();
                }
                else
                    return;
            }

            if (waveCountDown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else if (!completed)
            {
                waveCountDown -= Time.deltaTime;
            }
        }
    }

    void WaveCompleted()
    {
        Debug.Log("wave completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        //waves.length will stop wave looping but is beyond index
        //Wave check to stop spawning
        if(nextWave + 1 > waves.Length - 1)
        {
            completed = true;
            Debug.Log("all done");
            foos.SetActive(true);
            
            cf.XMaxValue = maxX;
        }

        if (!completed)
        {
            //Debug.Log("next wave");
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            //searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                Debug.Log("all done");
                
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        foos.SetActive(false);
        if(boxy != null)
        {
            //manipulate camera values
            cf.XMinValue = borderL.transform.position.x + 5;
            cf.XMaxValue = borderR.transform.position.x - 5;
        }
        Debug.Log("Spawning wave:" + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i<_wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f/_wave.rate);
        }

        oh.SetOverhead(this, nextWave);

        state = SpawnState.WAITING;

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
            beginTheWaves = true;
            borderL.enabled = true;     // Turns on left wall of combat area
            borderR.enabled = true;     // Turns on right wall of combat area

            // Activates magical flame walls to confine player
            foreach (ParticleSystem ps in leftFlames)
            {
                ps.Play();
            }
            foreach (ParticleSystem ps in rightFlames)
            {
                ps.Play();
            }
        }
    }
}
