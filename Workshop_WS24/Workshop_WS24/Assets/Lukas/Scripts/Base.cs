using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public static Base Instance { get; private set; }

    public float maxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Base instances found!");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Base took {amount} damage. Remaining: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Base destroyed! Game Over.");
        // Add game over logic here
    }
}
