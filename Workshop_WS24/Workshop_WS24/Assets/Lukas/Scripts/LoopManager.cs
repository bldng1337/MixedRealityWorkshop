using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

[System.Serializable]
    public class Wave
    {
        public List<int> enemyIDs; // each int represents an enemy type
    }


public class LoopManager : MonoBehaviour
{

     public bool waveReady = false;
    // Vr input
    public InputActionReference nextWaveAction;

     void OnEnable()
    {
        if (nextWaveAction != null)
        {
            nextWaveAction.action.performed += OnStartWavePressed;
            nextWaveAction.action.Enable();
        }
        
    }

    void OnDisable()
{
    if (nextWaveAction != null)
    {
        nextWaveAction.action.performed -= OnStartWavePressed;
        nextWaveAction.action.Disable();
    }
}

    void OnStartWavePressed(InputAction.CallbackContext context)
    {
        if (!waveReady) return;
        StartNextWave();
    }


        public void SetWaveReady(bool ready)
    {
        waveReady = ready;
    }


    public List<Wave> waves; // Set up in Inspector
    private int currentWaveIndex = 0;
   
    private bool waveInProgress = false;
    public static Vector3[] nodePositions;
    private static Queue<Enemy> enemiesToRemove;
    private static Queue<int> enemyIDsToSummon;

    public Transform nodeParent;
    public bool loopEnd;
    public GameObject winScreen;

    public int waveThreshold = 5;


    private void Start()
    {
        enemyIDsToSummon = new Queue<int>();
        enemiesToRemove = new Queue<Enemy>();
        EntitySummoner.Init();

        nodePositions = new Vector3[nodeParent.childCount];

        for (int i = 0; i < nodePositions.Length; i++)
        {
            nodePositions[i] = nodeParent.GetChild(i).position;
        }

        StartCoroutine(GameLooper());
        
        if (winScreen != null)
            winScreen.SetActive(false);

    }
    
    void Update()
{
}

    public void StartNextWave()
    {
        if (waveInProgress || currentWaveIndex >= waves.Count) return;

        Wave currentWave = waves[currentWaveIndex];
        StartCoroutine(SpawnWave(currentWave));
        currentWaveIndex++;

        if (currentWaveIndex >= waveThreshold)
        {
            if (winScreen != null)
                winScreen.SetActive(true);
        }
    }

IEnumerator SpawnWave(Wave wave)
{
    waveInProgress = true;

    foreach (int id in wave.enemyIDs)
    {
        EnqueueEnemyIDToSummon(id);
        yield return new WaitForSeconds(0.5f); // Delay between spawns
    }

    waveInProgress = false;
}

    void SummonTest()
    {
        EnqueueEnemyIDToSummon(0);
    }


    IEnumerator GameLooper()
    {
        while (!loopEnd)
        {
            //Spawn enemies
            if (enemyIDsToSummon.Count > 0)
            {
                for (int i = 0; i < enemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SpawnEnemy(enemyIDsToSummon.Dequeue());
                }
            }
            //Spawn towers

            //Move enemies
            NativeArray<Vector3> nodesToUse = new NativeArray<Vector3>(nodePositions, Allocator.TempJob);
            NativeArray<float> enemySpeeds = new NativeArray<float>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
            NativeArray<int> nodeIndices = new NativeArray<int>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray enemyAccess = new TransformAccessArray(EntitySummoner.EnemiesInGameTransforms.ToArray(), 2);

            for (int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
            {
                enemySpeeds[i] = EntitySummoner.EnemiesInGame[i].speed;
                nodeIndices[i] = EntitySummoner.EnemiesInGame[i].nodeIndex;
            }

            MoveEnemiesJob moveJob = new MoveEnemiesJob
            {
                nodePositions = nodesToUse,
                enemySpeeds = enemySpeeds,
                nodeIndex = nodeIndices,
                deltaTime = Time.deltaTime
            };

            JobHandle MoveJobHandle = moveJob.Schedule(enemyAccess);
            MoveJobHandle.Complete();

            for (int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
            {
                EntitySummoner.EnemiesInGame[i].nodeIndex = nodeIndices[i];

                //ENEMY AT THE END OF THE PATH
                if (EntitySummoner.EnemiesInGame[i].nodeIndex >= nodePositions.Length)
                {
                    // Old: EnqueueEnemyToRemove
                    EntitySummoner.EnemiesInGame[i].OnReachBase(); // new
                }

            }

            enemySpeeds.Dispose();
            nodeIndices.Dispose();
            enemyAccess.Dispose();
            nodesToUse.Dispose();

            //Tick towers

            //Apply Effects

            //Apply Damage

            //Remove Enemies
            if (enemiesToRemove.Count > 0)
            {
                for (int i = 0; i < enemiesToRemove.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(enemiesToRemove.Dequeue());
                }
            }
            //Remove Towers


            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        enemyIDsToSummon.Enqueue(ID);
    }

    public static void EnqueueEnemyToRemove(Enemy enemyToRemove)
    {
        enemiesToRemove.Enqueue(enemyToRemove);
    }
}

public struct MoveEnemiesJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> nodePositions;

    [NativeDisableParallelForRestriction]
    public NativeArray<float> enemySpeeds;

    [NativeDisableParallelForRestriction]
    public NativeArray<int> nodeIndex;

    public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        if (nodeIndex[index] < nodePositions.Length)
        {
            Vector3 positionsToMoveTo = nodePositions[nodeIndex[index]];
            Vector3 direction = positionsToMoveTo - transform.position;

            // Move the enemy towards the target position
            transform.position = Vector3.MoveTowards(transform.position, positionsToMoveTo, enemySpeeds[index] * deltaTime);

            // Rotate the enemy to face the direction they are moving
            if (direction != Vector3.zero) // Avoid errors when direction is zero
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            
            if(transform.position == positionsToMoveTo)
            {
                nodeIndex[index]++;
            }
        }
 
    }
}
