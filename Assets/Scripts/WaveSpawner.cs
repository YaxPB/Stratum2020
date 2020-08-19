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

    public float maxX;
    public Transform camLock;

    private void Start()
    {
        cf = FindObjectOfType<CameraFollow>();
        oh = FindObjectOfType<Overhead>();
        //oh.SetActive(false);
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("no spawn points foo");
        }

        camLock = boxy.gameObject.transform.GetChild(2);
        waveCountDown = timeBetweenWaves;
        maxX = cf.XMaxValue;
    }

    private void Update()
    {
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
            boxy.SetActive(false);
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
            boxy.SetActive(true);
            //manipulate camera values
            cf.XMinValue = camLock.transform.position.x;
            cf.XMaxValue = camLock.transform.position.x;
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
        if(collision.gameObject.CompareTag("Player"))
        {
            beginTheWaves = true;
        }
    }
}
