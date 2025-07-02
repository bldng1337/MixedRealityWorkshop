using System;
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

    public float damageToBase = 10f;
    public float attackInterval = 1.5f;
    private float attackTimer;
    private bool isAtBase = false;
    private Animator animator;

    public List<Weakness> weaknesses = new List<Weakness>();
    [SerializeField] Floating_HealthBar healthBar;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<Floating_HealthBar>();
    }
    public void Init()
    {
        currentHealth = maxHealth;
        transform.position = LoopManager.nodePositions[0];
        nodeIndex = 0;
        attackTimer = attackInterval;
        isAtBase = false;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(Energy damageType)
    {
        float finalDamage = damageType.value * GetWeaknessMultiplier(damageType.type);
        currentHealth -= finalDamage;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

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

    void Update()
    {
        if (!isAtBase) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            AttackBase();
            attackTimer = attackInterval;
        }
    }

    void AttackBase()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Trigger your attack animation

        }

        var baseRef = Base.Instance;
        if (baseRef != null)
        {
            baseRef.TakeDamage(damageToBase);
        }
        
        currentHealth -= 1f;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            LoopManager.EnqueueEnemyToRemove(this);
        }
    }

    public void OnReachBase()
    {
        isAtBase = true;
        // Optionally stop movement or animations
    }
}
