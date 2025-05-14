using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;
using Vector3 = UnityEngine.Vector3;

public class BeamFocus : Rune
{
    [SerializeField] RuneConnection input;
    [SerializeField] float range=30;

    private LineRenderer beam;

    void Start()
    {
        RegisterConnection(input);
        beam = gameObject.AddComponent<LineRenderer>();
        beam.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        //beam.startColor = Color.red;
        //beam.endColor = Color.green;

        // Set the width
        beam.startWidth = 0.01f;
        beam.endWidth = 0.01f;
    }

    
    void FixedUpdate()
    {
        beam.positionCount = 0;
        mainBuffer.add(input.buffer);
        if (mainBuffer.energy == null) return;
        Debug.Log("Main Buffer: " + mainBuffer.energy.value);
        if (mainBuffer.energy.value <= 0) return;
        var energy=mainBuffer.SplitOff(Math.Max(mainBuffer.energy.value / 2f,0.1f));
        //Debug.Log("Enemies: " + EntitySummoner.EnemiesInGame.Count);
        var closest= EntitySummoner.EnemiesInGame
            .Select((x) => new Tuple<Enemy,float>(x, DistTransform(x.transform)))
            .Aggregate((a, b) => a.Item2<b.Item2 ? a : b);
        //Debug.Log("Closest Dist"+closest.Item2);
        if(closest.Item2 > range) return;
        Debug.Log("Shoot");
        mainBuffer.energy.ApplyLineRenderer(beam);
        beam.positionCount = 2;
        beam.SetPosition(0, transform.position);
        beam.SetPosition(1, closest.Item1.transform.position+Vector3.up);
    }


    float DistTransform(Transform t)
    {
        return (t.position - transform.position).sqrMagnitude;
    }
}