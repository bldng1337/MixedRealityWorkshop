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
        dbg = gameObject.AddComponent<LineRenderer>();
        dbg.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        dbg.startColor = Color.red;
        dbg.endColor = Color.green;

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
        if (current==null) return;
        dbg.positionCount = 2;
        dbg.SetPosition(0, transform.position);
        dbg.SetPosition(1, current.transform.position);
    }


    private void Connect(InputAction.CallbackContext context)
    {
        current = LookedAt();
        if (current == null) return;
        if (current.GetConnectionType() != RuneConnection.Type.Output)
        {
            current = null;
            return;
        }
        current.PullConnection(transform);
    }

    private void Disconnect(InputAction.CallbackContext context)
    {
        if (current == null) return;
        current.ResetConnection();
        current.HandleConnect(LookedAt());
        current = null;
    }

    RuneConnection LookedAt()
    {
        const float range = 4;
        var dir = transform.rotation * Vector3.forward;
        RaycastHit hitInfo = new RaycastHit();
        dbg.positionCount = 2;
        dbg.SetPosition(0, transform.position);
        dbg.SetPosition(1, transform.position + dir * range);
        Physics.Raycast(transform.position, dir, out hitInfo, range, 1 << 7);
        if (hitInfo.collider == null) return null;
        return hitInfo.collider.GetComponent<RuneConnection>();
    }


}
