using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private Text bottomLeftText;

    [Header("Settings")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] public float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] public int nrOfWaves = 15;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();


    // Local variables:

    public int currentWave = 1;
    private float timeSinceLastSpawn;

    public int enemiesAlive;
    public int enemiesLeftToSpawn;

    private bool isSpawning = false;


    // Update functions:

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            bottomLeftText.text = "Enemies left: " + enemiesLeftToSpawn;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }


    // Game management:

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.path[0].position, Quaternion.identity, enemyContainer.transform);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        bottomLeftText.text = "Enemies left: " + enemiesLeftToSpawn;
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        if (timeBetweenWaves > 0.5f)
        {
            timeBetweenWaves -= 0.5f;
        }

        if (enemiesPerSecond > 0.1f)
        {
            enemiesPerSecond -= 0.1f;
        }

        if (currentWave <= nrOfWaves) 
        {
            StartCoroutine(StartWave());
        }
    }


    // Commands to be sent to interpreter: 
    public void StartCommand()
    {
        Debug.Log("Wave started");
        StartCoroutine(StartWave());
    }

    public void ResetSettings()
    {
        isSpawning = false;
        baseEnemies = 0;
        enemiesPerSecond = 0;
        timeBetweenWaves = 5f;
        nrOfWaves = 15;
    }
}
