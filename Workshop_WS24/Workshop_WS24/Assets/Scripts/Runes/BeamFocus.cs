using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class BeamFocus : Rune
{
    [SerializeField] RuneConnection input;
    [SerializeField] float range = 30;

    private LineRenderer beam;
    Material material;

    void Start()
    {
        RegisterConnection(input);
        beam = GetComponent<LineRenderer>();
        material = GetComponent<MeshRenderer>().material;
        // Set the color
        //beam.startColor = Color.red;
        //beam.endColor = Color.green;

        // Set the width
        beam.startWidth = 0.1f;
        beam.endWidth = 0.1f;
    }


    void FixedUpdate()
    {
        Debug.Log(beam);
        beam.positionCount = 0;
        mainBuffer.add(input.buffer);
        mainBuffer.ApplyMaterial(material);
        if (mainBuffer.energy == null) return;
        //Debug.Log("Main Buffer: " + mainBuffer.energy.value);
        if (mainBuffer.energy.value <= 0) return;
        //var energy = mainBuffer.SplitOff(Math.Max(mainBuffer.energy.value / 2f, 0.1f));
        //Debug.Log("Enemies: " + EntitySummoner.EnemiesInGame.Count);
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("These are the Enemies: " + (taggedObjects.Length));
        var closest = taggedObjects.Select((a) => a.GetComponent<Enemy>())
            .Select((x) => new Tuple<Enemy, float>(x, DistTransform(x.transform)))
            .Aggregate((a, b) => a.Item2 < b.Item2 ? a : b);
        Debug.Log("Closest Dist"+closest.Item2);

        if (closest.Item2 > range) return;
        Debug.Log("Shoot");
        var shootenergy=mainBuffer.SplitOff(mainBuffer.energy.value*0.1f);
        shootenergy.ApplyLineRenderer(beam);
        beam.positionCount = 2;
        beam.SetPosition(0, transform.position);
        beam.SetPosition(1, closest.Item1.transform.position + Vector3.up);
        var enemy = closest.Item1;
        shootenergy.value /= 2;
        enemy.TakeDamage(shootenergy);
    }


    float DistTransform(Transform t)
    {
        return (t.position - transform.position).sqrMagnitude;
    }
}
