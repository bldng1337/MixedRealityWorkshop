using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int nodeIndex;
    public float maxHealth;
    public float currentHealth;
    public float speed;
    public int ID;

    public void Init()
    {
        // Initialize enemy properties here
        // For example, set health, speed, etc.

        currentHealth = maxHealth;
        transform.position = LoopManager.nodePositions[0];
        nodeIndex = 0;
    }
}
