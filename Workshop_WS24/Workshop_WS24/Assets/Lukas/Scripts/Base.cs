using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public static Base Instance { get; private set; }

    public float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] Floating_HealthBar healthBar;
    Rigidbody2D rb;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Base instances found!");
            Destroy(this);
            return;
        }
        Instance = this;

        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<Floating_HealthBar>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Base took {amount} damage. Remaining: {currentHealth}");
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

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
