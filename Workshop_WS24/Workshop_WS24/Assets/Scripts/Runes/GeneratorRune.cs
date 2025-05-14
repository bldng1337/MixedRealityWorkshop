using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class GeneratorRune : Rune
{
    enum EnergyType
    {
        Fire,
        Water
    }
    [SerializeField] RuneConnection output;
    [SerializeField] EnergyType energyType= EnergyType.Fire;
    [SerializeField] float genamount = 0.01f;
    [SerializeField] float randomamount = 0f;

    void Start()
    {
        RegisterConnection(output);
    }

    
    void FixedUpdate()
    {
        switch (energyType)
        {
            case EnergyType.Fire:
                mainBuffer.add(new Fire(genamount, 0));
                break;
            case EnergyType.Water:
                mainBuffer.add(new Water(genamount, 0));
                break;
        }
        if (output == null) return;
        output.Push(mainBuffer);
    }
}