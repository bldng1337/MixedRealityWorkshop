


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
public enum EnergyType
{
    Fire,
    Water,
    Earth,
    Steam,
    Magma,
    Wood,
    Life
}



public abstract class Energy
{
    public float value;
    public float chaos;
    public abstract EnergyType type
    {
        get;
    }
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
        if (t == typeof(Earth)) return new Earth(value, chaos);
        if (t == typeof(Wood)) return new Wood(value, chaos);
        if (t == typeof(Magma)) return new Magma(value, chaos);
        if (t == typeof(Life)) return new Life(value, chaos);
        return null;
    }

    public static Energy FromEnergyType(EnergyType type, float value, float chaos)
    {
        switch (type)
        {
            case EnergyType.Fire:
                return new Fire(value, chaos);
            case EnergyType.Water:
                return new Water(value, chaos);
            case EnergyType.Steam:
                return new Steam(value, chaos);
            case EnergyType.Earth:
                return new Earth(value, chaos);
            case EnergyType.Wood:
                return new Wood(value, chaos);
            case EnergyType.Magma:
                return new Magma(value, chaos);
            case EnergyType.Life:
                return new Life(value, chaos);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    public abstract void Attack(Enemy e);
}

public class Wood : Energy
{
    public Wood(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => Color.green;

    public override EnergyType type => EnergyType.Wood;

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {

        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    public override void Attack(Enemy e)
    {

    }
}

public class Earth : Energy
{
    public Earth(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => new Color(0.588f,0.294f,0);

    public override EnergyType type => EnergyType.Earth;

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {

        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public override void Attack(Enemy e)
    {

    }
}

public class Magma : Energy
{
    public Magma(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => Color.Lerp(Color.red,Color.black,0.5f);

    public override EnergyType type => EnergyType.Fire;

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {

        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public override void Attack(Enemy e)
    {

    }
}

public class Life : Energy
{
    public Life(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => Color.yellow;

    public override EnergyType type => EnergyType.Fire;

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

public class Fire : Energy
{
    public Fire(float value, float chaos) : base(value, chaos)
    {
    }

    public override Color color => Color.red;

    public override EnergyType type => EnergyType.Fire;

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {

        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
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

    public override EnergyType type => EnergyType.Water;

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

    public override EnergyType type => EnergyType.Steam;

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.startWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.endWidth = Mathf.Min(0.3f, Mathf.Max(0.05f, value));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
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