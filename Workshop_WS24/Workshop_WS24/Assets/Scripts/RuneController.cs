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
        dbg = gameObject.AddComponent<LineRenderer>();
        dbg.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        dbg.startColor = Color.magenta;
        dbg.endColor = Color.magenta;

        // Set the width
        dbg.startWidth = 0.001f;
        dbg.endWidth = 0.001f;
        Select(0);
    }

    private void SwitchRune(InputAction.CallbackContext context)
    {
        Debug.Log("SwitchRune "+ selected);
        Select(selected+1);
    }

    private void PlaceRune(InputAction.CallbackContext context)
    {
        //Debug.Log("PlaceRune");
        if (selected < 0) return;
        var socket = LookedAt();
        if (socket == null) return;
        socket.SocketRune(runes[selected]);
    }



    void Select(int slot)
    {
        //Debug.Log($"Select {slot}");
        if (slot == selected) return;
        if (slot >= runes.Length)
        {
            Debug.Log("Yee");
            slot = 0;
        }
        //Debug.Log("Selected " + slot);
        selected = slot;
        if (rune != null)
        {
            Destroy(rune);
        }
        //Debug.Log("Got Rune");
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
        dbg.positionCount = 2;
        dbg.SetPosition(0, transform.position);
        dbg.SetPosition(1, transform.position+dir*range);
        Physics.Raycast(transform.position, dir, out hitInfo, range, 1<<6);
        if (hitInfo.collider == null)
            return null;
        return hitInfo.collider.GetComponent<RuneSocket>();
    }


    void Update()
    {
        //LookedAt();
        //Debug.Log(LookedAt());
    }
}
