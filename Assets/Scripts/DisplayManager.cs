using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] GameObject assiaPopup;
    [SerializeField] GameObject oleksandrPopup;
    [SerializeField] GameObject assiaEndGamePopup;
    [SerializeField] GameObject oleksandrEndGamePopup;
    
    // Start is called before the first frame update
    void Start()
    {
        assiaPopup.SetActive(false);
        oleksandrPopup.SetActive(false);
        assiaEndGamePopup.SetActive(false);
        oleksandrEndGamePopup.SetActive(false);
    }
/// <summary>
/// Display popup for the athlete based on the level
/// </summary>
    public void DisplayProcess()
    {
        Time.timeScale = 0; //met le jeu en pause

        if (GateOpener.Instance.currentLevel < 5)
        {
            if (GateOpener.Instance.athleteID == 2)
            {
                assiaPopup.SetActive(true);
            }
            else if (GateOpener.Instance.athleteID == 1)
            {
                oleksandrPopup.SetActive(true);
            }
        }

        if (GateOpener.Instance.currentLevel == 5)
        {
            if (GateOpener.Instance.athleteID == 2)
            {
                assiaEndGamePopup.SetActive(true);
            }
            else if (GateOpener.Instance.athleteID == 1)
            {
                oleksandrEndGamePopup.SetActive(true);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // V�rifie si le clic est un clic gauche (bouton 0)
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (GateOpener.Instance.currentLevel < 5)
            {
                if (GateOpener.Instance.athleteID == 2)
                {
                    assiaPopup.SetActive(false); //retire le texte
                    Time.timeScale = 1; // reprend le jeu
                }
                else if (GateOpener.Instance.athleteID == 1)
                {
                    oleksandrPopup.SetActive(false); //retire le texte
                    Time.timeScale = 1; // reprend le jeu
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GateOpener.Instance.currentLevel < 5)
            {
                if (GateOpener.Instance.athleteID == 2)
                {
                    assiaPopup.SetActive(false); //retire le texte
                    Time.timeScale = 1; // reprend le jeu
                }
                else if (GateOpener.Instance.athleteID == 1)
                {
                    oleksandrPopup.SetActive(false); //retire le texte
                    Time.timeScale = 1; // reprend le jeu
                }
            }
        }
    }

    // Method to restart the scene
    public void RestartScene()
    {
        Debug.Log("Restarting scene...");
        SaveState();
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveState()
    {
        PlayerPrefs.SetInt("CurrentAthleteID", GateOpener.Instance.athleteID);
        PlayerPrefs.Save();
    }

}
