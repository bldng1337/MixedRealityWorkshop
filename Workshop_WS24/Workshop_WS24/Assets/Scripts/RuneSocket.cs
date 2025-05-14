using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RuneSocket : MonoBehaviour
{
    public GameObject rune;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SocketRune(GameObject rune)
    {
        if (this.rune != null)
        {
            Destroy(this.rune);
            this.rune = null;
        }
        this.rune = Instantiate(rune, transform);
    }
}
