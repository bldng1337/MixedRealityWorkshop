using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;

public class GeneratorRune : Rune
{
    enum EnergyType
    {
        Fire,
    }
    [SerializeField] RuneConnection output;

    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        this.mainBuffer.add(new Fire(0.01f, 0));
        if (output == null) return;
        output.Push(mainBuffer);
    }
}