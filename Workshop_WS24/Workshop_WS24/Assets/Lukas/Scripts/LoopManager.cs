using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;


public class LoopManager : MonoBehaviour
{
    public static Vector3[] nodePositions;
    private static Queue<Enemy> enemiesToRemove;
    private static Queue<int> enemyIDsToSummon;

    public Transform nodeParent;
    public bool loopEnd;

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
        InvokeRepeating("SummonTest", 0f, 3f);
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

                //DESPAWN ENEMY AT THE END OF THE PATH
                if (EntitySummoner.EnemiesInGame[i].nodeIndex >= nodePositions.Length)
                {
                    EnqueueEnemyToRemove(EntitySummoner.EnemiesInGame[i]);
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
            Debug.Log("Enemy reached the end of the path");
            Vector3 positionsToMoveTo = nodePositions[nodeIndex[index]];

            transform.position = Vector3.MoveTowards(transform.position, positionsToMoveTo, enemySpeeds[index] * deltaTime);

            if(transform.position == positionsToMoveTo)
            {
                nodeIndex[index]++;
            }
        }
 
    }
}
