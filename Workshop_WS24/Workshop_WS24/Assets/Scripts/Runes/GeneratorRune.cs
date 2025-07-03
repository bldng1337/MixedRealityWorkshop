using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;

public class GeneratorRune : Rune
{
    [SerializeField] RuneConnection output;
    [SerializeField] EnergyType energyType = EnergyType.Fire;
    [SerializeField] float genamount = 0.1f;
    [SerializeField] float randomamount = 0f;

    Material material;
    void Start()
    {
        RegisterConnection(output);
        material = GetComponent<MeshRenderer>().material;
        mainBuffer.cap*=2;
    }

    
    void FixedUpdate()
    {
        if (output == null) return;
        var amount = genamount + Random.value * randomamount;
        mainBuffer.add(Energy.FromEnergyType(energyType, genamount, 0));
        mainBuffer.ApplyMaterial(material);
        output.Push(mainBuffer);

    }
}