using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BarrierTrigger : MonoBehaviour
{
    private int protectedLevel;
    [SerializeField] GateOpener gateOpener;
    [SerializeField] int gateID = -1;
    // Start is called before the first frame update
    void Start()
    {
        string objectName = gameObject.name;
        if (objectName.Length > 0)
        {
            // R�cup�re le dernier caract�re du nom
            char lastChar = objectName[objectName.Length - 1];

            // Essaie de convertir le dernier caract�re en int
            int number;
            if (int.TryParse(lastChar.ToString(), out number))
            {
                protectedLevel = number;
            }
            else
            {
                Debug.Log("Le dernier caract�re n'est pas un chiffre.");
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        //gateOpener.OpenGateAttempt(protectedLevel);
        if(other.gameObject.tag == "PlayerDot")
            {
                gateOpener.OpenGateAttempt(gateID);
            }
    }
}
