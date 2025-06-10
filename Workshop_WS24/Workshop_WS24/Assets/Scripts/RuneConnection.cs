using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneConnection : MonoBehaviour
{
    public enum Type
    {
        Input,
        Output,
    }

    [SerializeField] Type type;

    RuneConnection connected;

    private LineRenderer connect;

    public Rune rune;

    Transform connect_to;

    public EnergyBuffer buffer;

    Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        switch (type)
        {
            case Type.Input:
                buffer = new EnergyBuffer();
                break;
            case Type.Output:
                connect = GetComponent<LineRenderer>();
                connect.positionCount = 0;
                material.SetFloat("_energy", 0);
                material.SetFloat("_chaos", 0);
                material.SetColor("_energy_color", Color.white);
                break;
        }
    }

    public void Push(EnergyBuffer buffer)
    {
        if (type != Type.Output) return;
        if (connected == null) return;
        if (buffer == null) return;
        connected.buffer.add(buffer);
        connected.buffer.energy.ApplyLineRenderer(connect);
    }

    // Update is called once per frame
    void Update()
    {
        if (buffer != null && material != null)
        {
            buffer.ApplyMaterial(material);
        }
        else
        {
            material.SetFloat("_energy", 1);
            material.SetFloat("_chaos", 0);
            material.SetColor("_energy_color", Color.white);
        }
        if (connect == null) return;
        if (connect_to != null)
        {
            connect.positionCount = 2;
            connect.SetPosition(0, transform.position);
            connect.SetPosition(1, connect_to.position);
        }
        else
        {
            connect.positionCount = 0;
        }
    }

    public Type GetConnectionType()
    {
        return type;
    }

    public void PullConnection(Transform transform)
    {
        VoidConnection();
        connect_to = transform;
    }

    public void ResetConnection()
    {
        if (connected != null || connect == null)
        {
            return;
        }
        connect_to = null;
    }

    public void VoidConnection()
    {
        if (connected != null)
        {
            connected.connected = null;
            connected.connect_to = null;
        }
        connect_to = null;
        connected = null;
    }

    public bool HandleConnect(RuneConnection other)
    {
        if (other == null) return false;
        if (other.type == type) return false;
        other.VoidConnection();
        other.connected = this;
        connected = other;
        if (type == Type.Output)
        {
            connect_to = other.transform;
        }
        else
        {
            other.connect_to = transform;
        }
        return true;
    }

    public RuneConnection GetConnection()
    {
        return connected;
    }
}
