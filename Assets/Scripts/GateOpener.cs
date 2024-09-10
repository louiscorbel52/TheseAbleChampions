using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GateOpener : MonoBehaviour
{
    public static GateOpener Instance;
    private HintManager hintManager;
    public int currentLevel;
    public int athleteID = -1;

    public List<GameObject> barriersToDisableAssia;
    public List<GameObject> barriersToActivateAssia;

    public string assiaLevel1SoundName = "bump";  // The name of the sound clip to play

    public float yMinimumValueForLevel2Assia = 5.0f;

    private bool changingColor = false;


    [SerializeField] private DisplayManager displayManager;

    //Gestion des barri�res
    [SerializeField] private GameObject[] levelBarriers;
    [SerializeField] private GameObject[] levelBarriersTextures;

    //~~~~Oleksander 
    //Niveau 2 v1
    [SerializeField] float moveSyncWithNPCMargin = 0.2f; // marge d'erreur sur la v�locit� quand le joueur se synchro avec les npc
    //niveau 2 v2
                                                         //Variable vérifiant si l'init de l'épreuve a été faite
        private bool wheelTrialInitialised = false;
        // Point central autour duquel l'objet pourrait tourner
        [SerializeField] Transform oleksanderSphereCenter;
        public Vector2 oleksanderCernterPoint;
        // La tolérance pour déterminer si le mouvement est circulaire
        [SerializeField] private float toleranceSphereRadius = 0.5f;
        // Distance initiale de l'objet par rapport au point central
        [SerializeField] private float oleksanderCircleRadius;
        // Angle total parcouru autour du point central
        private float totalAngleTravelled = 0.0f;
        // Position précédente pour calculer l'angle
        private Vector2 previousPosition;
        // Indicateur si le cercle complet est détecté
        private bool circleCompleted = false;
    //niveau 3 
        private float yThinkOutBox; // hauteur de la barriere pour l'�tage o� il faut contourner

    //~~~~Assia
    // Variable that defines the valid x position for the player to pass level 1
    private float xValidValueLevel1Assia = -7.0f;
    // Variable that defines the margin of error for the player to pass level 1
    public float marginOfErrorAssiaLevel1 = 0.5f;

    private Vector3 lastPosition;

    public Vector3 StartingPoint;

    //Gestion des NPC
    [SerializeField] float NPCSpeed;
    [SerializeField] float NPCAmplitude;
    private float NPCvelocity;
    [SerializeField] private GameObject level1NPC;
    [SerializeField] private GameObject level2NPC;
    [SerializeField] private GameObject level3NPC;
    public GameObject[] levelNPC;

    //Gestion du player
    [SerializeField] public GameObject controlledBall;
    private Rigidbody rb;

    public Color ballOriginalColor;
    private Coroutine changeColorCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        hintManager = GetComponent<HintManager>();

        currentLevel = 0;

        level1NPC.SetActive(true);
        level2NPC.SetActive(false);
        level3NPC.SetActive(false);
        levelNPC = new GameObject[] { level1NPC, level2NPC, level3NPC };

        lastPosition = transform.position;
        StartingPoint = transform.position;

        // Load the saved athlete ID and change color accordingly
        LoadState();

        //ballOriginalColor = controlledBall.GetComponent<Renderer>().material.color;

        //Oleksander

        // Initialise la valeur de hauteur à dépasser pour entrer au niveau 3
        yThinkOutBox = levelBarriers[1].transform.position.y;

    }

    private void LoadState()
    {
        if (PlayerPrefs.HasKey("CurrentAthleteID"))
        {
            int savedAthleteID = PlayerPrefs.GetInt("CurrentAthleteID");

            if (savedAthleteID == 1)
            {
                GameObject objWithTag1 = GameObject.FindWithTag("1");
                if (objWithTag1 != null)
                {
                    Renderer renderer = objWithTag1.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = Color.green;
                    }
                }
            }
            else if (savedAthleteID == 2)
            {
                GameObject objWithTag2 = GameObject.FindWithTag("2");
                if (objWithTag2 != null)
                {
                    Renderer renderer = objWithTag2.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = Color.green;
                    }
                }
            }
        }
    }

    void Awake()
    {
        // Ensure only one instance of the singleton exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (athleteID == 1){
            if (currentLevel == 1){

                /*
                //Code pour LVL 2 - v1
                
                //Gestion des NPC
                // Calcule la vitesse verticale en fonction du temps, de l'amplitude et de la vitesse
                NPCvelocity = Mathf.Cos(Time.time * NPCSpeed) * NPCAmplitude;
                foreach (GameObject npcGroup in levelNPC)
                {
                    if (npcGroup.activeSelf == true)
                    {
                        foreach (Transform child in npcGroup.transform)
                        {

                            // Applique la v�locit� au Rigidbody
                            child.GetComponent<Rigidbody>().velocity = new Vector3(rb.velocity.x, NPCvelocity, rb.velocity.z);
                        }
                    }
                }
                */
                //Code pour lvl 2 - v2
                if (!wheelTrialInitialised)
                {
                    rb = controlledBall.GetComponent<Rigidbody>();
                    // Initialise la distance initiale par rapport au centre
                    oleksanderCernterPoint = oleksanderSphereCenter.position;
                    // Initialise la position précédente
                    previousPosition = controlledBall.transform.position;
                    wheelTrialInitialised = true;
                }
                if (!circleCompleted && CheckForCircle())
                {
                    Debug.Log("Cercle complet détecté !");
                    circleCompleted = true;
                    OpenGateAttempt(2);
                }
            }
        }
        else if(athleteID == 2){

            
            foreach (GameObject obj in barriersToDisableAssia)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("A GameObject in the list is null.");
                }
            
            }

            foreach (GameObject obj in barriersToActivateAssia)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("A GameObject in the list is null.");
                }
            
            }

            if (currentLevel == 2)
            {
                // Get the current position of the object
                Vector3 currentPosition = controlledBall.transform.position;

                // Check if the current y position is lower than the last y position
                if (currentPosition.y < lastPosition.y)
                {
                    // Reset the StartingPoint variable
                    StartingPoint = currentPosition;
                    Debug.Log($"Starting point updated with {currentPosition.y}");
                }

                // Update the last position with the current position
                lastPosition = currentPosition;

            }

            // New condition for athleteID == 2 and currentLevel == 3
            else if (currentLevel == 3)
            {

                rb = controlledBall.GetComponent<Rigidbody>();
                float marginOfError = 0.1f;
                if (rb.velocity.magnitude < marginOfError && changingColor == false)
                {
                    changeColorCoroutine = StartCoroutine(ChangeBallColor());
                    changingColor = true;
                }

                else if (rb.velocity.magnitude >= marginOfError && changingColor == true)
                {
                    // Stop the coroutine if the ball moves
                    if (changeColorCoroutine != null && controlledBall.GetComponent<Renderer>().material.color != Color.green)
                    {
                        StopCoroutine(changeColorCoroutine);
                        changingColor = false;
                        // Optionally reset the barrier color immediately
                        controlledBall.GetComponent<Renderer>().material.color = ballOriginalColor;
                    }
                }
            }

        }     
    }

private IEnumerator ChangeBallColor()
{
    Color[] colors = { Color.red, new Color(1.0f, 0.65f, 0.0f), Color.green }; // Red, Orange, Green

    foreach (Color color in colors)
    {
        controlledBall.GetComponent<Renderer>().material.color = color;
        yield return new WaitForSeconds(1.0f);
    }

    yield return new WaitForSeconds(1.0f);

    controlledBall.GetComponent<Renderer>().material.color = ballOriginalColor;
    changingColor = false;
}


    public void OpenGateAttempt(int protectedLevel)
    {
        if(athleteID == 1){
            //Condition de passage au niveau 2
            //if (currentLevel == 1 & CheckForSynchronicity())
            if (currentLevel == 1 & circleCompleted)
            {
                GoToNextLevel();
                level2NPC.SetActive(true);

            }
            //Condition de passage au niveau 3
            if (currentLevel == 2 & CheckForOutsideBox())
            {
                GoToNextLevel();
                level3NPC.SetActive(true);
            }
            //Condition de passage au niveau 4
            if (currentLevel == 3 & CheckForSpeed())
            {
                MoveNPCBallsHigher();
                GoToNextLevel();
            }

            if (currentLevel == 4 )
            {
                currentLevel++;
                displayManager.DisplayProcess();
                Debug.Log("wahou");
            }
        }
        else if(athleteID == 2){
            //Condition de passage au niveau 2
            if (currentLevel == 1)
            {
                if(CheckForPrecisePosition()){
                    GoToNextLevel();
                    level2NPC.SetActive(true);
                }
                else{
                    Debug.Log("You are not at the right position");
                    float distanceToValidPos = Mathf.Abs(controlledBall.transform.position.x - xValidValueLevel1Assia);
                    Debug.Log($"You are {distanceToValidPos} away from the valid position");
                    
                    float scaledValue = ScaleValue(distanceToValidPos, 0f, 10.0f, 3.0f, 0f);
                    SoundManager.Instance.PlaySound(assiaLevel1SoundName, scaledValue);

                }


            }

            else if(currentLevel == 2){
                if(StartingPoint.y<yMinimumValueForLevel2Assia){
                    GoToNextLevel();
                }
                else{
                    Debug.Log($"You are not low enough Starting point is {StartingPoint.y} and minimum value is {yMinimumValueForLevel2Assia}");
                }
            }

            else if (currentLevel == 3)
            {
                if (controlledBall.GetComponent<Renderer>().material.color == Color.green)
                {
                    MoveNPCBallsHigher();
                    GoToNextLevel();
                    Debug.Log("Barrier is green, proceeding with level 3 actions.");
                }
            }

            else if (currentLevel == 4 )
            {
                currentLevel++;
                displayManager.DisplayProcess();
                Debug.Log("wahou");
            }
        }
        
    }

    // Method to move NPC balls higher by 8 units
    private void MoveNPCBallsHigher()
    {
        // Move level 1 NPCs
        MoveChildrenHigher(level1NPC);

        // Move level 2 NPCs
        MoveChildrenHigher(level2NPC);

        // Move level 3 NPCs
        MoveChildrenHigher(level3NPC);
    }

    // Helper method to move children of a parent GameObject higher by 8 units
    private void MoveChildrenHigher(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            // Check the gameTag of the child
            int gameTag;
            if (int.TryParse(child.gameObject.tag, out gameTag) && (gameTag == 1 || gameTag == 2))
            {
                // Skip moving this child
                continue;
            }

            Vector3 newPosition = child.position;
            newPosition.y += 8;
            child.position = newPosition;
        }
    }

    //Renvoie true si la boule est assez rapide, false sinon
    private bool CheckForSpeed()
    {
        if (rb.velocity.magnitude >= 80)        {            return true;        }
        else {            return false;        }

    }

    //Renvoie true si la boule est synchro zvec les NPC, false sinon
    private bool CheckForSynchronicity()
    {
        float bornUp = Math.Abs(NPCvelocity * (1 + moveSyncWithNPCMargin));
        float bornLow = Math.Abs(NPCvelocity * (1 - moveSyncWithNPCMargin));
        float vel = Math.Abs(rb.velocity.y);
        if (vel < bornUp & vel > bornLow) { StopAllNPC(); return true; }
        else { return false; }  
    }

    // Méthode pour détecter le mouvement circulaire et vérifier le cercle complet
    private bool CheckForCircle()
    {
        // Calculer la distance actuelle par rapport au point central
        float currentDistance = Vector2.Distance(controlledBall.transform.position, oleksanderCernterPoint);

        // Vérifier si l'objet reste à une distance constante du centre
        if (Mathf.Abs(currentDistance - oleksanderCircleRadius) > toleranceSphereRadius)
        {
            return false;
        }

        // Calculer les vecteurs de direction entre l'objet et le centre (précédent et actuel)
        Vector2 previousDirection = previousPosition - oleksanderCernterPoint;
        Vector2 currentDirection = (Vector2)controlledBall.transform.position - oleksanderCernterPoint;

        // Calculer l'angle entre les deux positions (en degrés)
        float angle = Vector2.SignedAngle(previousDirection, currentDirection);

        // Ajouter cet angle à l'angle total parcouru
        totalAngleTravelled += angle;

        // Mettre à jour la position précédente
        previousPosition = controlledBall.transform.position;
        /*
        Debug.Log("début d'un nouveau Log-------------------------------");
        Debug.Log("distance balle à centre = " + currentDistance);
        Debug.Log("angle = " + angle);
        Debug.Log("totalAngleTravelled = " + totalAngleTravelled);
        */


        // Vérifier si l'objet a parcouru au moins 360 degrés (cercle complet)
        if (Mathf.Abs(totalAngleTravelled) >= 360f)
        {
            return true;  // Cercle complet détecté
        }

        return false;
    }

    //Renvoie true si la balle est assez haute, false sinon. Sert � v�rifier que le joeuur a pens� hors de la boite et donc s'est �chapp� de l'�tage 2
    private bool CheckForOutsideBox()
    {
        if(controlledBall.transform.position.y > yThinkOutBox) {return true;}
        else { return false; }
    }

    //Renvoie true si la boule a pris assez d'�lan, false sinon
    private bool CheckForMomentum()
    {
        return true;
    }

    private bool CheckForPrecisePosition()
    {
        
        if(Mathf.Abs(controlledBall.transform.position.x - xValidValueLevel1Assia) <= marginOfErrorAssiaLevel1) {return true;}
        else { return false; }
    }

    //Fonction qui d�clenche la proc�dure compl�te de changement de niveau
    public void GoToNextLevel()
    {
        if (currentLevel == 0)
        {
            ChangeBarrierTextures();
        };
        if (currentLevel == 1 | currentLevel == 2 | currentLevel == 3)
        {
            OpenBarrier();
        }
        currentLevel++;
        if (currentLevel == 1 | currentLevel == 2 | currentLevel == 3)
        {
            StartCoroutine(hintManager.HintCoroutine(currentLevel, athleteID));
        }
        displayManager.DisplayProcess();
    }
    private void StopAllNPC()
    {
        foreach (GameObject npcGroup in levelNPC)
        {
            if (npcGroup.activeSelf == true)
            {
                foreach (Transform child in npcGroup.transform)
                {

                    // Applique la v�locit� au Rigidbody
                    child.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    private void OpenBarrier()
    {

        Renderer render = levelBarriersTextures[athleteID].transform.GetChild(currentLevel-1).GetComponent<Renderer>();
        Color c = render.material.color;
        c.a *= 0.2f;
        render.material.color = c;

        Destroy(levelBarriers[currentLevel - 1]);
    }

    private void ChangeBarrierTextures()
    {
        levelBarriersTextures[0].SetActive(false);
        levelBarriersTextures[athleteID].SetActive(true);
    }
    // Function to scale a value from one range to another
    private float ScaleValue(float value, float minOriginal, float maxOriginal, float minTarget, float maxTarget)
    {
        return ((value - minOriginal) / (maxOriginal - minOriginal)) * (maxTarget - minTarget) + minTarget;
    }
}
