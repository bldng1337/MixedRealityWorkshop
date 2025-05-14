using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;



public class CombineRune : Rune
{
    
    [SerializeField] RuneConnection input;

    [SerializeField] RuneConnection input1;

    //[SerializeField] RuneConnection input2;

    [SerializeField] RuneConnection output;

    class Recipe
    {
        public Type[] ingridients;
        public Type output;
        public float[] ratio;
        public float chaos;

        public Recipe(Type[] ingridients, float[] ratio, Type output, float chaos)
        {
            this.ingridients = ingridients;
            this.ratio = ratio;
            this.output = output;
            this.chaos = chaos;
        }

        public bool CanApply(Energy[] energies)
        {
            if (energies.Length != ingridients.Length) return false;
            return ingridients.All((x) => energies.Any((y) => x == y.GetType()));
        }

        public Energy Apply(Energy[] energies)
        {
            float chaos = 0;
            float[] amounts = new float[energies.Length];
            for (int i = 0; i < ingridients.Length; i++)
            {
                Energy energy = energies.Where((x) => x.GetType() == ingridients[i]).First();
                chaos += energy.chaos;
                amounts[i] = energy.value;
            }
            float minamountscaled = amounts.Select((x, i) => x / ratio[i]).Min();
            chaos += amounts.Select((x, i) => x - (minamountscaled * ratio[i])).Sum() * (this.chaos / 100);
            float value = amounts.Select((x, i) => (minamountscaled * ratio[i])).Sum();
            return Energy.FromType(output,value,chaos);
        }

    }


    private static readonly Recipe[] recipies = new Recipe[] { 
        new Recipe(new Type[] {
            typeof(Water), typeof(Fire)
        },new float[] {
            1,1
        },typeof(Steam),5),
    };


    void Start()
    {
        RegisterConnection(output);
        RegisterConnection(input);
        RegisterConnection(input1);
        mainBuffer.cap = 999;
    }


    void FixedUpdate()
    {
        if(input.buffer==null || input1.buffer==null) return;
        Energy[] energies= new Energy[] {
            input.buffer.energy,
            input1.buffer.energy,
        };
        if(energies.Any((x)=>x==null)) return;
        var recipe=recipies.Where((x) => x.CanApply(energies)).FirstOrDefault();
        if (recipe == null) return;
        if (mainBuffer.energy!=null && mainBuffer.energy.GetType() != recipe.output) return;
        mainBuffer.add(recipe.Apply(energies));
        foreach (var energy in energies)
        {
            energy.value = 0;
            energy.chaos = 0;
        }
        if (output == null) return;
        output.Push(mainBuffer);
    }
}