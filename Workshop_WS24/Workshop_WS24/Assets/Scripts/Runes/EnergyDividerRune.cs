using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;



public class EnergyDividerRune : Rune
{

    [SerializeField] RuneConnection input;

    [SerializeField] RuneConnection output;

    [SerializeField] RuneConnection output1;


    Material material;
    void Start()
    {
        RegisterConnection(output);
        RegisterConnection(input);
        RegisterConnection(output1);
        material = GetComponent<MeshRenderer>().material;
    }


    void FixedUpdate()
    {
        input.buffer.ApplyMaterial(material);
        output.PushEnergy(input.buffer.SplitOff(input.buffer.energy.value/2));
        output1.Push(input.buffer);
    }
}