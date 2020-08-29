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
        public GameObject enemy;
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
    // public GameObject foos;
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

        /*foreach(Transform sp in spawnPoints)
        {
            foreach(Wave leWave in waves)
            {
                numEnemies += leWave.count;
            }
        }*/
        
        cf = FindObjectOfType<CameraFollow>();
        oh = FindObjectOfType<Overhead>();
        //oh.SetActive(false);
        if(spawnPoints.Length == 0)
        {
            Debug.LogError("no spawn points foo");
        }

        // Set waveCountDown to the value of timeBetweenWaves at the start
        waveCountDown = timeBetweenWaves;
        // Store the camFollow min and max x-values for later reference
        maxX = cf.XMaxValue;
        minX = cf.XMinValue;
    }

    private void Update()
    {
        // Need a way to check if all enemies in a given "zone" are defeated
        if (allWavesComplete)
        {
            // Enables the nextZone sprite and animation
            nextArrow.enabled = true;
            nextArrow.SetTrigger("allClear");
            // Coroutine that gradually destroys completed combatZones to prevent accidental repetition or NullReferenceExceptions
            StartCoroutine(SelfDestruct());
            return;
        }

        // Once wave spawning is triggered
        if (beginTheWaves)
        {
            if (state == SpawnState.WAITING)
            {
                // Check if all enemies in a given wave are defeated/still exist
                if (!EnemyIsAlive())
                {
                    // Signal that the wave has been completed
                    WaveCompleted();
                } // if all enemies in a given wave are defeated
            } // if current spawn state is WAITING

            // Added second conditional check to consolidate multiple ifs
            if (waveCountDown <= 0 && completed)
            {
                if (state != SpawnState.SPAWNING)
                {
                    // Make sure any currently running Coroutines stop before spawning a new wave
                    StopAllCoroutines();
                    // Spawn the next wave of enemies if there are any left
                    StartCoroutine(SpawnWave(waves[nextWave]));
                } // if current spawn state is SPAWNING
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

        yield return new WaitForSeconds(2f);
        nextArrow.enabled = false;
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    void WaveCompleted()
    {
        Debug.Log("wave completed");
        // Reset the waveCountDown timer
        waveCountDown = timeBetweenWaves;
        // represents the state of the current wave (completed or not)
        completed = true;

        // Decrement every time a wave is completed
        numWaves--;
        state = SpawnState.COUNTING;
        
        // Check if all waves have been completed
        if(numWaves <= 0)
        {
            allWavesComplete = true;
            // Reset CamFollow max and min x-coordinates
            cf.XMaxValue = maxX;
            cf.XMinValue = minX;
            return;
        }
    }

    bool EnemyIsAlive()
    {
        // Removed timer that was preventing the Enemy check
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            Debug.Log("all done");
            return false;
        } // if there are no more active Enemy objects in the current scene
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        // Reset completed every time a new wave is spawning
        completed = false;

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
        // So the waveCountDown knows when to start counting down
        state = SpawnState.WAITING;
        // Prevents an out of bounds index exception
        if(nextWave + 1 >= waves.Length)
        {
            yield break;
        }
        // Only increment nextWave if there is another wave to increment by
        nextWave++;
        yield break;
    }

    void SpawnEnemy(GameObject waveEnemy)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            if(enemy.transform.position == _sp.position)
            {
                _sp.position += transform.right * 2;
            }
        }
        Instantiate(waveEnemy, _sp.position, _sp.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !allWavesComplete)
        {
            AudioManagerBG.SwitchTrack("combat");
            PlayerCombat.instance.gameObject.SendMessage("TimeToFight", true);
            if (numWaves <= 0)
            {
                Debug.Log("There are no waves!");
                StartCoroutine(SelfDestruct());
                return;
            }
            
            if (oh != null)
            {
                oh.SetOverhead(this, numWaves);
            }
            Debug.Log("Total number of enemies on this floor: " + numEnemies);
            // Immediately spawn first wave upon walking into activationBox
            StartCoroutine(SpawnWave(waves[nextWave])); 
            borderL.enabled = true;     // Turns on left wall of combat area
            borderR.enabled = true;     // Turns on right wall of combat area

            if (boxy != null)
            {
                //manipulate camera values
                cf.XMinValue = borderL.transform.position.x + 5;
                //cf.XMaxValue = borderR.transform.position.x - 5;
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
            // Disables activationBox so we don't accidentally re-trigger the WaveSpawner
            // (Also handled through SelfDestruct Coroutine)
            activationBox.enabled = false;
            beginTheWaves = true;
        }
    }
}
