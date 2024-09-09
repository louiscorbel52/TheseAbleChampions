using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallNPC : MonoBehaviour
{
    public float velocity;
    public float marginOfError = 0.5f;
    public GameObject dotPrefab;
    private Rigidbody rb;
    private Camera mainCamera;

    public GameObject assiaPopup;
    public GameObject oleksandrPopup;


    private void Start()
    {
        mainCamera = Camera.main;

        rb = GetComponent<Rigidbody>();

        if (GateOpener.Instance != null)
        {
            // Access the shared integer value from the singleton
            Debug.Log("Shared value: " + GateOpener.Instance.currentLevel);
        }
        else
        {
            Debug.LogError("SharedValueSingleton instance not found.");
        }
    }
    private void Update()
    {
        velocity = rb.velocity.y;

        Vector3 mousePosition = Input.mousePosition;

        // Convertit la position de la souris en coordonn�es du monde
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane + 1.0f));

        worldPosition.z = transform.position.z; // Ensure both positions are on the same plane

        // Calculate the distance between the mouse position and the object's position
        float distance = Vector3.Distance(worldPosition, transform.position);

        // Detect click on npc ball at level 0 -> update to level 1, destroy npc ball and create a controlled ball

        if(GateOpener.Instance.currentLevel == 0 && GateOpener.Instance.athleteID == -1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (distance <= marginOfError)
                {

                    // Get the tag of the current GameObject
                    string objectTag = gameObject.tag;

                    List<string> validValues = new List<string> { "1", "2", "3" };

                    if(validValues.Contains(objectTag))
                    {
                        Debug.Log($"{gameObject.name} was clicked within the margin of error and will be destroyed!");
                        // Deactivate collider of clicked object
                        gameObject.GetComponent<Collider>().enabled = false;

                        // Convert the tag to an integer
                        int tagAsInt;
                        
                        // Try to parse the tag as an integer
                        if (int.TryParse(objectTag, out tagAsInt))
                        {
                            Debug.Log($"The tag '{objectTag}' was successfully converted to the integer: {tagAsInt}");
                        }
                        else
                        {
                            Debug.LogError($"Failed to convert the tag '{objectTag}' to an integer.");
                        }

                        // Set the athleteID in the singleton to the converted integer
                        GateOpener.Instance.athleteID = tagAsInt;
                        // Destroy the GameObject
                        Destroy(gameObject);
                        // Create a new controlled ball at the position of the clicked object
                        GameObject controlledBall = Instantiate(dotPrefab, transform.position, Quaternion.identity);
                        // Increment the current level in the singleton
                        GateOpener.Instance.currentLevel++;
                        // Set the controlled ball in the singleton
                        GateOpener.Instance.controlledBall = controlledBall; 
                    }

                            
                }
                else
                {
                    Debug.Log($"Click was outside the margin of error. Distance: {distance}");
                }
            }
        }
        
    }

}
