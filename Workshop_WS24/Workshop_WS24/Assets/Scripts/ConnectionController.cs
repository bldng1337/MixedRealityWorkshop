using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionController : MonoBehaviour
{
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

    void Update()
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
        if(Physics.Raycast(transform.position, dir, out hitInfo, range))
        {
            dbg.SetPosition(1, hitInfo.point);
        }
        else
        {
            dbg.SetPosition(1, transform.position + dir * range);
        }
        dbg.SetPosition(2, current.transform.position);
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
        dbg.positionCount = 2;
        dbg.SetPosition(0, transform.position);
        dbg.SetPosition(1, transform.position + dir * range);
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(transform.position, dir, out hitInfo, range, 1 << 7);
        if (hitInfo.collider == null) return null;
        return hitInfo.collider.GetComponent<RuneConnection>();
    }


}
