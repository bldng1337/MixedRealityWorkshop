using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTriggerZone : MonoBehaviour
{
     public LoopManager loopManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            loopManager.SetWaveReady(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            loopManager.SetWaveReady(false);
        }
    }
}
