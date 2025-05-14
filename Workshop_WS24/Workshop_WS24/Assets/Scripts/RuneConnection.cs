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

    void Start()
    {
        switch (type)
        {
            case Type.Input:
                buffer=new EnergyBuffer();
                break;
            case Type.Output:
                connect = GetComponent<LineRenderer>();
                connect.positionCount = 0;
                break;
        }
    }

    public void Push(EnergyBuffer buffer)
    {
        if (type != Type.Output) return;
        if (connected == null) return;
        connected.buffer.add(buffer);
    }

    // Update is called once per frame
    void Update()
    {
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
        if (connected != null || connect==null)
        {
            return;
        }
        connect_to= transform;
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
        if(other == null) return false;
        if (other.type == type) return false;
        other.connected = this;
        connected = other;
        if(type == Type.Output)
        {
            connect_to = other.transform;
        }
        else
        {
            other.connect_to = transform;
        }
        return true;
    }
}
