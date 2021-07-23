using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartTrigger : MonoBehaviour
{

    public bool started = false;

    public UnityEvent onMatchStarted;

    private void OnTriggerEnter(Collider other)
    {
        if(!started)
        {
            onMatchStarted?.Invoke();
            started = true;
        }
    }
}
