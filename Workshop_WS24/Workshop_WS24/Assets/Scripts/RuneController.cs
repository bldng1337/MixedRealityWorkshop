using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RuneController : MonoBehaviour
{

    [SerializeField] GameObject[] runes;

    private int selected = 0;

    void Start()
    {

        
        //Physics.Raycast()
    }


    RuneSocket LookedAt()
    {
        const float range= 300;
        Transform transform = GetComponent<Transform>();
        var dir= Vector3.up;//transform.rotation * 
        RaycastHit hitInfo=new RaycastHit();
        Debug.DrawRay(transform.position, dir * range,Color.red);
        //Debug.Log(transform.position);
        //Physics.Raycast(transform.position, dir, out hitInfo, range, LayerMask.NameToLayer("RuneSocket"));
        //if (hitInfo.collider == null)
            return null;
        //return hitInfo.collider.GetComponent<RuneSocket>();
    }


    void Update()
    {
        LookedAt();
        //Debug.Log(LookedAt());
    }
}
