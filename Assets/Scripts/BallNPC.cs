using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallNPC : MonoBehaviour
{
    public GameObject quadPrefab;
    public GameObject swimmerHoverTextureQuad; // Public variable for the hover quad
    public GameObject runnerHoverTextureQuad; // Public variable for the hover quad

    //public Material basePlayerMaterial;
    public float velocity;
    public float marginOfError = 0.5f;
    public GameObject dotPrefab;
    private Rigidbody rb;
    public bool needHint = false;
    private Camera mainCamera;
    [SerializeField] private int level;

    // Centre du NPC
    private Vector2 center;

    /// <summary>
    /// Variables pour indice 1 athlete 1
    /// </summary>
    // Rayon du cercle
    private float radius = 1.5f;
    // Vitesse angulaire (en radians par seconde)
    private float speed = 2.0f;
    // Variable pour suivre l'angle courant autour du cercle
    private float currentAngle = 0.0f;

    /// <summary>
    /// Variables pour indice 2 athlete 1
    /// </summary>
    // Distance que l'objet doit parcourir vers la gauche
    public float distance = 10.0f;

    /// <summary>
    /// Variables pour indice 3 athlete 1
    /// </summary>
    private float fastSpeed = 15f;
    private float fastDistance = 15f;
    private bool moveDown = true;
    private GameObject hoverQuadInstance;
    public GameObject assiaPopup;
    public GameObject oleksandrPopup;

    public string assiaHoverSoundName = "Sd_dot_hover_Assia_f";
    public string oleksandrHoverSoundName = "Sd_hover_Oleksandr";
    public string ballSelectionSound = "Sd_dot_selection_f";


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
        center = (Vector2)transform.position + new Vector2 ( -radius, 0 ) ;

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

        if (GateOpener.Instance.currentLevel == 0 && GateOpener.Instance.athleteID == -1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (distance <= marginOfError)
                {
                    //SoundManager.Instance.PlaySound(ballSelectionSound, 1.0f);

                    // Get the tag of the current GameObject
                    string objectTag = gameObject.tag;

                    List<string> validValues = new List<string> { "1", "2", "3" };

                    if (validValues.Contains(objectTag))
                    {
                        Debug.Log($"{gameObject.name} was clicked within the margin of error and will be destroyed!");
                        // Deactivate collider of clicked object
                        //gameObject.GetComponent<Collider>().enabled = false;

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
                        Debug.Log("Creating a controlled ball...");
                        Debug.Log("GateOpener.Instance.controlledBall = " + GateOpener.Instance.controlledBall);
                        GameObject controlledBall = Instantiate(dotPrefab, transform.position, Quaternion.identity);
                        GameObject quad = Instantiate(quadPrefab, transform.position, Quaternion.identity);

                        // Position the quad just above the NPC on the Z axis
                        Vector3 controlledBallPos = controlledBall.transform.position;
                        quad.transform.position = new Vector3(controlledBallPos.x, controlledBallPos.y, controlledBallPos.z + 1); // Adjust the offset as needed
                        //quad.GetComponent<Renderer>().material = basePlayerMaterial;
                        // Optionally, parent the quad to the NPC to maintain relative positioning
                        quad.transform.SetParent(controlledBall.transform);
                        // Set the controlled ball in the singleton
                        GateOpener.Instance.controlledBall = controlledBall;
                        // Set the original color of the ball in the singleton
                        GateOpener.Instance.ballOriginalColor = controlledBall.GetComponent<Renderer>().material.color;

                        Debug.Log("controlled ball created and assigned to the singleton.");
                        Debug.Log("GateOpener.Instance.controlledBall = " + GateOpener.Instance.controlledBall);
                        // Increment the current level in the singleton
                        GateOpener.Instance.GoToNextLevel();


                    }


                }
                else
                {
                    Debug.Log($"Click was outside the margin of error. Distance: {distance}");
                }
            }

        }
        if (needHint)
        {
            if (GateOpener.Instance.athleteID == 1)
            {
                if (GateOpener.Instance.currentLevel == 1) {
                    MoveInCircles();
                }
                if (GateOpener.Instance.currentLevel == 2)
                {
                    MoveOnSides();
                }
                if (GateOpener.Instance.currentLevel == 3)
                {
                    MoveFast();
                }
            }

        }

    }

    /// <summary>
    /// Fonction pour l'indice niveau 1 de oleksander
    /// </summary>
    private void MoveInCircles()
    {
        // Calculer l'angle courant en fonction de la vitesse et du temps écoulé
        currentAngle += speed * Time.deltaTime;

        // Calculer les nouvelles positions en utilisant sinus et cosinus
        float x = center.x + Mathf.Cos(currentAngle) * radius;
        float y = center.y + Mathf.Sin(currentAngle) * radius;

        // Appliquer la nouvelle position à l'objet
        transform.position = new Vector2(x, y);
    }
    /// <summary>
    /// Fonction pour l'indice 2 d'oleksander
    /// </summary>
    private void MoveOnSides()
    {
            // Déplacer l'objet vers la gauche en fonction de la vitesse et du temps
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            // Vérifier si l'objet a parcouru la distance D
            if (Vector3.Distance(center, transform.position) >= distance)
            {
            // Ramener l'objet instantanément à sa position de départ
            transform.position = center;
            }
        
    }

    /// <summary>
    /// Fonction pour l'indice 3 d'oleksander
    /// </summary>
    private void MoveFast()
    {
        if (moveDown)
        {
            // Déplacer l'objet vers le bas en fonction de la vitesse et du temps
            rb.velocity = Vector3.down * fastSpeed;

        }
        else {
            rb.velocity = Vector3.up * fastSpeed;
        }

        // Vérifier si l'objet a parcouru la distance D
        if (moveDown & Vector2.Distance(center, transform.position) >= fastDistance)
        {
            
            moveDown = false;
        }
        if (!moveDown & transform.position.y > 17f)
        {
            moveDown = true;
        }

    }

    private void OnMouseEnter()
    {
        if (tag == "1" && hoverQuadInstance == null)
        {
            //SoundManager.Instance.PlaySound(oleksandrHoverSoundName, 1.0f);

            // Disable the existing quad
            if (this.transform.GetChild(0) != null)
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
            }

            // Instantiate the hover quad
            hoverQuadInstance = Instantiate(swimmerHoverTextureQuad);

            // Position the hover quad just above the NPC on the Z axis
            Vector3 npcPosition = transform.position;
            hoverQuadInstance.transform.position = new Vector3(npcPosition.x, npcPosition.y, npcPosition.z + 1); // Adjust the offset as needed

            // Optionally, parent the hover quad to the NPC to maintain relative positioning
            hoverQuadInstance.transform.SetParent(transform);
        }
        else if (tag == "2" && hoverQuadInstance == null)
        {
            //SoundManager.Instance.PlaySound(assiaHoverSoundName, 1.0f);

            // Disable the existing quad
            if (this.transform.GetChild(0) != null)
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
            }

            // Instantiate the hover quad
            hoverQuadInstance = Instantiate(runnerHoverTextureQuad);

            // Position the hover quad just above the NPC on the Z axis
            Vector3 npcPosition = transform.position;
            hoverQuadInstance.transform.position = new Vector3(npcPosition.x, npcPosition.y, npcPosition.z + 1); // Adjust the offset as needed

            // Optionally, parent the hover quad to the NPC to maintain relative positioning
            hoverQuadInstance.transform.SetParent(transform);
        }
    }

    private void OnMouseExit()
    {
        if (tag == "1" && hoverQuadInstance != null)
        {
            // Destroy the hover quad
            Destroy(hoverQuadInstance);
            hoverQuadInstance = null;

            // Re-enable the existing quad
            if (this.transform.GetChild(0) != null)
            {
                this.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        if (tag == "2" && hoverQuadInstance != null)
        {
            // Destroy the hover quad
            Destroy(hoverQuadInstance);
            hoverQuadInstance = null;

            // Re-enable the existing quad
            if (this.transform.GetChild(0) != null)
            {
                this.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}