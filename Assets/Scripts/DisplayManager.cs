using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] GameObject popUpEnsemble;
    [SerializeField] GameObject popUpTexte;
    
    // Start is called before the first frame update
    void Start()
    {
        popUpEnsemble.SetActive(false);
    }

    public void DisplayProcess(int currentLevel)
    {
        Time.timeScale = 0; //met le jeu en pause
        popUpTexte.GetComponent<TMP_Text>().text = "texte athlete niveau " + currentLevel; //change le texte

        popUpEnsemble.SetActive(true); //affiche le texte
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // V�rifie si le clic est un clic gauche (bouton 0)
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            popUpEnsemble.SetActive(false); //retire le texte
            Time.timeScale = 1; // reprebnd le jeu
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            popUpEnsemble.SetActive(false); //retire le texte
            Time.timeScale = 1; // reprend le jeu
        }
    }
}
