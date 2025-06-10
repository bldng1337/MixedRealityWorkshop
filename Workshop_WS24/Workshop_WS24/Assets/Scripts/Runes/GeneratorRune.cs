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
    [SerializeField] EnergyType energyType = EnergyType.Fire;
    [SerializeField] float genamount = 0.1f;
    [SerializeField] float randomamount = 0f;

    Material material;
    void Start()
    {
        RegisterConnection(output);
        material = GetComponent<MeshRenderer>().material;
        mainBuffer.cap = 200;
    }

    
    void FixedUpdate()
    {
        if (output == null) return;
        switch (energyType)
        {
            case EnergyType.Fire:
                mainBuffer.add(new Fire(genamount, 0));
                break;
            case EnergyType.Water:
                mainBuffer.add(new Water(genamount, 0));
                break;
        }
        mainBuffer.ApplyMaterial(material);
        output.Push(mainBuffer);

    }
}