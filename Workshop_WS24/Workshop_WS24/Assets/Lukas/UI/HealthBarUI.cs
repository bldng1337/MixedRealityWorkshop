using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    //[SerializeField] private HealthBar slider;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, 0);
    private Transform target;
    private float maxHealth = 100f;

    public void Initialize(Transform followTarget, float maxHP)
    {
        target = followTarget;
        maxHealth = maxHP;
        //slider.value = 1f;
    }

    public void SetHealth(float currentHealth)
    {
        //slider.value = currentHealth / maxHealth;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Camera.main.transform.rotation; // always face camera
        }
    }
}
