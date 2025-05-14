using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static List<Transform> EnemiesInGameTransforms;
    public static Dictionary<int, GameObject> enemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> enemyPool;

    private static bool isInitialized;

    // Start is called before the first frame update
    public static void Init()
    {
        // Initialize the dictionaries and list
        if(!isInitialized)
        {
            enemyPrefabs = new Dictionary<int, GameObject>();
            enemyPool = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGameTransforms = new List<Transform>();
            EnemiesInGame = new List<Enemy>();

            EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData>("Enemies");
            Debug.Log(Enemies[0].name);

            foreach (EnemySummonData enemy in Enemies)
            {
                enemyPrefabs.Add(enemy.enemyID, enemy.enemyPrefab);
                enemyPool.Add(enemy.enemyID, new Queue<Enemy>());
            }
            isInitialized = true;
        }
        else
        {
            Debug.Log("EntitySummoner Already Initialized");
        }
    }

    public static Enemy SpawnEnemy(int enemyID)
    {
        Enemy summonedEnemy = null;

        if (enemyPrefabs.ContainsKey(enemyID))
        {
            Queue<Enemy> referncedQueue = enemyPool[enemyID];

            if (referncedQueue.Count > 0)
            {
                // Dequeue an enemy from the pool and initialize it
                summonedEnemy = referncedQueue.Dequeue();
                summonedEnemy.Init();

                summonedEnemy.gameObject.SetActive(true);
            }
            else
            {
                // Instantiate a new enemy if the pool is empty and initialize it
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyID], LoopManager.nodePositions[0], Quaternion.identity);
                summonedEnemy = newEnemy.GetComponent<Enemy>();
                summonedEnemy.Init();
            }
        }
        else
        {
            Debug.LogError($"Entity Summoner: Enemy ID {enemyID} not found");
            return null;
        }
        
        EnemiesInGameTransforms.Add(summonedEnemy.transform);
        EnemiesInGame.Add(summonedEnemy);
        summonedEnemy.ID = enemyID;
        return summonedEnemy;
    }

    public static void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyPool[enemyToRemove.ID].Enqueue(enemyToRemove);
        enemyToRemove.gameObject.SetActive(false);
        EnemiesInGameTransforms.Remove(enemyToRemove.transform);
        EnemiesInGame.Remove(enemyToRemove);
    }
}
