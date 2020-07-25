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
    bool completed;
    bool beginTheWaves;

    public GameObject foos;

    private void Start()
    {
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("no spawn points foo");
        }

        waveCountDown = timeBetweenWaves;
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
        if(nextWave + 1 > waves.Length - 1)
        {
            //nextWave = 0;
            completed = true;
            Debug.Log("all done");
            foos.SetActive(true);
        }

        if (!completed)
        {
            Debug.Log("next wave");
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
        Debug.Log("Spawning wave:" + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i<_wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f/_wave.rate);
        }

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
