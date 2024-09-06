using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTrigger : MonoBehaviour
{
    [SerializeField] GateOpener opener;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        opener.OpenGateAttempt(5);
    }
}
