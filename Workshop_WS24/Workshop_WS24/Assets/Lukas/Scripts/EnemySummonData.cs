using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySummonData", menuName = "Create EnemySummonData")]

public class EnemySummonData : ScriptableObject
{
    public GameObject enemyPrefab;
    public int enemyID;
}
