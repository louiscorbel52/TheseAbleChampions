using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BarrierTrigger : MonoBehaviour
{
    private int protectedLevel;
    [SerializeField] GateOpener gateOpener;
    // Start is called before the first frame update
    void Start()
    {
        string objectName = gameObject.name;
        if (objectName.Length > 0)
        {
            // Récupère le dernier caractère du nom
            char lastChar = objectName[objectName.Length - 1];

            // Essaie de convertir le dernier caractère en int
            int number;
            if (int.TryParse(lastChar.ToString(), out number))
            {
                protectedLevel = number;
            }
            else
            {
                Debug.Log("Le dernier caractère n'est pas un chiffre.");
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        gateOpener.OpenGateAttempt(protectedLevel);
    }
}
