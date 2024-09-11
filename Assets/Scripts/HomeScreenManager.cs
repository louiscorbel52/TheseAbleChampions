using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this to use SceneManager

public class HomeScreenManager : MonoBehaviour
{
    public GameObject popupCanvas; // Reference to the PopupCanvas
    public GameObject popupCanvasFromSettings; // Reference to the PopupCanvas

    public GameObject canvas; // Reference to the Canvas
    public GameObject settingsCanvas; // Reference to the PopupCanvas


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to load the LouisCWorkScene
    public void LoadLouisCWorkScene()
    {
        SceneManager.LoadScene("LouisCWorkScene");
    }

    // Function to show the popup canvas
    public void ShowPopupCanvas()
    {
        popupCanvas.SetActive(true);
        canvas.SetActive(false);
    }

    public void ClosePopupCanvas()
    {
        popupCanvas.SetActive(false);
        canvas.SetActive(true);
    }

    public void ShowSettingsCanvas()
    {
        settingsCanvas.SetActive(true);
        canvas.SetActive(false);
    }

    public void CloseSettingsCanvas()
    {
        settingsCanvas.SetActive(false);
        canvas.SetActive(true);
    }

    public void ShowPopupCanvasFromSettings()
    {
        popupCanvasFromSettings.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void ClosePopupCanvasFromSettings()
    {
        popupCanvasFromSettings.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    // Function to close the game
    public void ExitGame()
    {
        #if UNITY_EDITOR
        // If running in the Unity Editor, log a message
        Debug.Log("ExitGame called - Application.Quit() does not work in the Editor.");
        #else
        // If running in a standalone build, quit the application
        Application.Quit();
        #endif
    }
}