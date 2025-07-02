using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floating_HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;

    public float healthBarHeight = 5f;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (slider != null)
        {
            slider.value = currentHealth / maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (camera != null)
        {
            transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
                             camera.transform.rotation * Vector3.up);
        }

        if (target != null)
        {
            transform.position = target.position + new Vector3(0, healthBarHeight, 0); // offset for above head
        }
    }
}
