


using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Rune : MonoBehaviour
{
    public EnergyBuffer mainBuffer = new EnergyBuffer();

    protected void RegisterConnection(RuneConnection con)
    {
        con.rune = this;
    }
}


public abstract class Energy
{
    public float value;
    public float chaos;
    public Energy(float value, float chaos)
    {
        this.value = value;
        this.chaos = chaos;
    }

    public abstract Color color
    {
        get;
    }

    public Energy SplitOff(float amount)
    {
        var newenergy = FromType(this.GetType(), Mathf.Min(amount, value), chaos);
        value -= amount;
        if (value <= 0)
        {
            value = 0;
            chaos = 0;
        }
        return newenergy;
    }

    public abstract void ApplyLineRenderer(LineRenderer lineRenderer);

    public static Energy FromType(Type t, float value, float chaos)
    {
        if (t == typeof(Fire)) return new Fire(value, chaos);
        if (t == typeof(Water)) return new Water(value, chaos);
        if (t == typeof(Steam)) return new Steam(value, chaos);
        return null;
    }
    public abstract void Attack(Enemy e);
}

public class Fire : Energy
{
    public Fire(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => Color.red;

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {

        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
    }

    public override void Attack(Enemy e)
    {

    }
}

public class Water : Energy
{
    public Water(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => Color.blue;

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
    }

    public override void Attack(Enemy e)
    {

    }
}

public class Steam : Energy
{
    public Steam(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => new Color(0.6f, 0.6f, 0.9f);

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    public override void Attack(Enemy e)
    {

    }
}

public class EnergyBuffer
{
    public Energy energy;
    public float cap = 2;

    public void add(Energy other)
    {
        if (other == null) return;
        if (energy == null || energy.value == 0)
        {
            energy = other.SplitOff(Mathf.Min(other.value, cap));
            return;
        }
        if (other.GetType() != energy.GetType()) return;
        energy.value += Mathf.Min(other.value / (1 + other.chaos), cap - energy.value);
        other.value = Mathf.Max(other.value - cap, 0);
        energy.chaos += other.chaos;
        other.chaos = 0;
    }
    public void add(EnergyBuffer other)
    {
        add(other.energy);
    }
    public Energy SplitOff(float amount)
    {
        return energy.SplitOff(amount);
    }
    public void ApplyMaterial(Material material)
    {
        if (material == null) return;
        if (energy == null)
        {
            material.SetFloat("_energy", 0);
            material.SetFloat("_chaos", 0);
            material.SetColor("_energy_color", Color.white);
            return;
        }
        material.SetFloat("_energy", energy.value);
        material.SetFloat("_chaos", energy.chaos);
        material.SetColor("_energy_color", energy.color);
    }
}