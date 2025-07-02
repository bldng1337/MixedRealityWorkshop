using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionController : MonoBehaviour
{

    [SerializeField]
    public TextMeshPro label;


    RuneConnection current;
    private XRInputActions inputActions;

    private LineRenderer dbg;

    void Start()
    {
        dbg = GetComponent<LineRenderer>();

        // Set the color
        dbg.startColor = Color.white;
        dbg.endColor = Color.white;

        // Set the width
        dbg.startWidth = 0.01f;
        dbg.endWidth = 0.01f;


        inputActions = new XRInputActions();
        inputActions.CustomRight.Enable();
        inputActions.CustomRight.Trigger.performed += Connect;
        inputActions.CustomRight.Trigger.canceled += Disconnect;
    }

    void UpdateBeam()
    {
        const float range = 8;
        var dir = transform.rotation * Vector3.forward;
        if (current == null)
        {
            dbg.startWidth = 0.01f;
            dbg.endWidth = 0.01f;
            dbg.positionCount = 2;
            dbg.SetPosition(0, transform.position);
            dbg.SetPosition(1, transform.position + dir * range);
            return;
        }
        dbg.startWidth = 0.1f;
        dbg.endWidth = 0.1f;
        dbg.positionCount = 3;
        dbg.SetPosition(0, transform.position);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(transform.position, dir, out hitInfo, range))
        {
            dbg.SetPosition(1, hitInfo.point);
        }
        else
        {
            dbg.SetPosition(1, transform.position + dir * range);
        }
        dbg.SetPosition(2, current.transform.position);
    }

    void Update()
    {
        UpdateBeam();
        if (label == null) return;
        Debug.Log("Checking");
        var buffer = LookedAtBuffer();
        if (buffer != null)
        {
            Debug.Log("Got buffer");
            if (buffer.energy != null)
            {
                label.text = "Element: "+ buffer.energy.name+"\nAmount: "+buffer.energy.value+"\nChaos: "+buffer.energy.chaos;
            }else
            {
                label.text = "Empty Connection";
            }
        }
        else
        {
            label.text = "";
        }
    }

    


    private void Connect(InputAction.CallbackContext context)
    {
        current = LookedAt();
        if (current == null) return;
        if (current.GetConnectionType() != RuneConnection.Type.Output)
        {
            if (current.GetConnection() != null)
            {
                current=current.GetConnection();
                current.VoidConnection();
                return;
            }
            current = null;
            return;
        }
        //current.PullConnection(transform);
    }

    private void Disconnect(InputAction.CallbackContext context)
    {
        if (current == null) return;
        //current.ResetConnection();
        current.HandleConnect(LookedAt());
        current = null;
    }

    RuneConnection LookedAt()
    {
        const float range = 8;
        var dir = transform.rotation * Vector3.forward;
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(transform.position, dir, out hitInfo, range, 1 << 7);
        Debug.Log(hitInfo.collider);
        if (hitInfo.collider == null) return null;
        return hitInfo.collider.GetComponent<RuneConnection>();
    }

    EnergyBuffer LookedAtBuffer()
    {
        
        const float range = 8;
        var dir = transform.rotation * Vector3.forward;
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(transform.position, dir, out hitInfo, range, 1 << 7);
        if (hitInfo.collider == null) return null;
        var socket = hitInfo.collider.GetComponent<RuneSocket>();
        if (socket != null && socket.gameObject!=null)
        {
            return socket.gameObject.GetComponent<Rune>().mainBuffer;
        }
        var conn = hitInfo.collider.GetComponent<RuneConnection>();
        if (conn != null)
        {
            return conn.buffer;
        }
        return null;
        /*var con = LookedAt();
        Debug.Log(con);
        if (con!=null)
        {
            return con.buffer;
        }
        return null;*/
    }

}
