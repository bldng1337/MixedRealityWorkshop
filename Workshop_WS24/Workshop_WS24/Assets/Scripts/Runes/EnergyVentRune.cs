using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;



public class EnergyVentRune : Rune
{

    [SerializeField] RuneConnection input;

    Material material;
    void Start()
    {
        RegisterConnection(input);
        material = GetComponent<MeshRenderer>().material;
    }


    void FixedUpdate()
    {
        input.buffer.ApplyMaterial(material);
        input.buffer.SplitOff(0.3f);
    }
}