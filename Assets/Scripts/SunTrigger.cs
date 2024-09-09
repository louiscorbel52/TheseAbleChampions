using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTrigger : MonoBehaviour
{
    [SerializeField] GateOpener opener;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (this.name == "TriggerBarrierOleksandr")
        {

            opener.OpenGateAttempt(3);
        }
        if (this.name == "Sun")
        {
            opener.OpenGateAttempt(5);

        }
    }
}
