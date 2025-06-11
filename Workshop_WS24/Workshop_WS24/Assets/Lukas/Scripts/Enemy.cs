using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // <-- needed for Inspector support!
public struct Weakness
{
    public EnergyType type;
    public float multiplier;
}

public class Enemy : MonoBehaviour
{
    public int nodeIndex;
    public float maxHealth;
    public float currentHealth;
    public float speed;
    public int ID;

    public List<Weakness> weaknesses = new List<Weakness>();

    public void Init()
    {
        currentHealth = maxHealth;
        transform.position = LoopManager.nodePositions[0];
        nodeIndex = 0;
    }

    public void TakeDamage(Energy damageType)
    {
        float finalDamage = damageType.value * GetWeaknessMultiplier(damageType.type);
        currentHealth -= finalDamage;

        if (currentHealth <= 0f)
        {
            LoopManager.EnqueueEnemyToRemove(this);
        }
    }

    public float GetWeaknessMultiplier(EnergyType incomingType)
    {
        foreach (Weakness weakness in weaknesses)
        {
            if (weakness.type == incomingType)
            {
                return weakness.multiplier;
            }
        }
        return 1.0f; // Neutral if no match
    }
}
