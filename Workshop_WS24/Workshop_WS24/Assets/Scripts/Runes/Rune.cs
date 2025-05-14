


using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Rune : MonoBehaviour
{
    public EnergyBuffer mainBuffer=new EnergyBuffer();
    
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

    public abstract Energy SplitOff(float amount);

    public abstract void ApplyLineRenderer(LineRenderer lineRenderer);
}

public class Fire:Energy
{
    public Fire(float value, float chaos) : base(value, chaos)
    {
    }

    public override void ApplyLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.yellow;
    }

    override
    public Energy SplitOff(float amount)
    {
        var newenergy=new Fire(Mathf.Min(amount, value), chaos);
        value -= amount;
        if (value <= 0)
        {
            value = 0;
            chaos = 0;
        }
        return newenergy;
    }
}


public class EnergyBuffer
{
    public Energy energy;
    public float cap = 1;

    public void add(Energy other)
    {
        if (other == null) return;
        if(energy == null)
        {
            energy = other.SplitOff(Mathf.Min(other.value, cap));
        }
        if (other.GetType() != energy.GetType()) return;
        energy.value += Mathf.Min(other.value, cap-energy.value);
        other.value = Mathf.Max(other.value-cap,0);
        energy.chaos = other.chaos;
        other.chaos = 0;
    }
    public void add(EnergyBuffer other)
    {
        add(other.energy);
    }
    public void SplitOff(float amount)
    {
        if (amount < 0) return;
        energy.SplitOff(amount);
    }
}