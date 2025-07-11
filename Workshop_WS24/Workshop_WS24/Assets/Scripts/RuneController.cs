using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.Unicode;

public class RuneController : MonoBehaviour
{

    [SerializeField] GameObject[] runes;
    private XRInputActions inputActions;

    private int selected = -1;
    private GameObject rune;
    private LineRenderer dbg;
    

    void Start()
    {
        inputActions = new XRInputActions();
        inputActions.CustomLeft.Enable();
        inputActions.CustomLeft.Secondary.performed += PlaceRune;
        inputActions.CustomLeft.Grip.performed += SwitchRune;
        dbg = GetComponent<LineRenderer>();

        // Set the color
        dbg.startColor = Color.white;
        dbg.endColor = Color.white;

        // Set the width
        dbg.startWidth = 0.01f;
        dbg.endWidth = 0.01f;
        Select(0);
    }

    private void SwitchRune(InputAction.CallbackContext context)
    {
        Select(selected+1);
    }

    private void PlaceRune(InputAction.CallbackContext context)
    {
        if (selected < 0) return;
        var socket = LookedAt();
        if (socket == null) return;
        socket.SocketRune(runes[selected]);
    }



    void Select(int slot)
    {
        if (slot == selected) return;
        if (slot >= runes.Length)
        {
            slot = 0;
        }
        selected = slot;
        if (rune != null)
        {
            Destroy(rune);
        }
        rune = Instantiate(runes[selected], gameObject.transform);
        rune.transform.localPosition = Vector3.forward * 0.1f;
        rune.transform.localScale = rune.transform.localScale * 0.1f;
        rune.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
    }


    RuneSocket LookedAt()
    {
        const float range= 8;
        Transform transform = GetComponent<Transform>();
        var dir= transform.rotation*Vector3.forward;
        RaycastHit hitInfo=new RaycastHit();
        Physics.Raycast(transform.position, dir, out hitInfo, range, 1<<6);
        if (hitInfo.collider == null)
            return null;
        return hitInfo.collider.GetComponent<RuneSocket>();
    }


    void Update()
    {
        const float range = 8;
        Transform transform = GetComponent<Transform>();
        var dir = transform.rotation * Vector3.forward;
        dbg.positionCount = 2;
        dbg.SetPosition(0, transform.position);
        dbg.SetPosition(1, transform.position + dir * range);
        //LookedAt();
        Debug.Log("Looked at: "+LookedAt());
    }
}
