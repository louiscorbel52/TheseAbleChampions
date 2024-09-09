using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GateOpener : MonoBehaviour
{
    public static GateOpener Instance;
    public int currentLevel;
    public int athleteID = -1;

    public List<GameObject> barriersToDisableAssia;
    public List<GameObject> barriersToActivateAssia;

    public string assiaLevel1SoundName = "bump";  // The name of the sound clip to play


    [SerializeField] private DisplayManager displayManager;

    //Gestion des barri�res
    [SerializeField] private GameObject[] levelBarriers;
    [SerializeField] float moveSyncWithNPCMargin = 0.2f; // marge d'erreur sur la v�locit� quand le joueur se synchro avec les npc
    private float yThinkOutBox; // hauteur de la barriere pour l'�tage o� il faut contourner

    // Variable that defines the valid x position for the player to pass level 1
    private float xValidValueLevel1Assia = -7.0f;
    // Variable that defines the margin of error for the player to pass level 1
    public float marginOfErrorAssiaLevel1 = 0.5f;

    //Gestion des NPC
    [SerializeField] float NPCSpeed;
    [SerializeField] float NPCAmplitude;
    private float NPCvelocity;
    [SerializeField] private GameObject level1NPC;
    [SerializeField] private GameObject level2NPC;
    [SerializeField] private GameObject level3NPC;
    private GameObject[] levelNPC;

    //Gestion du player
    [SerializeField] public GameObject controlledBall;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;

        yThinkOutBox = levelBarriers[1].transform.position.y;

        level1NPC.SetActive(true);
        level2NPC.SetActive(false);
        level3NPC.SetActive(false);
        levelNPC = new GameObject[] { level1NPC, level2NPC, level3NPC };
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
                rb = controlledBall.GetComponent<Rigidbody>();
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

        }     
    }


    public void OpenGateAttempt(int protectedLevel)
    {
        if(athleteID == 1){
            //Condition de passage au niveau 2
            if (currentLevel == 1 & CheckForSynchronicity())
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
                GoToNextLevel();
            }

            if (currentLevel == 4 )
            {
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
    private void GoToNextLevel()
    {
        currentLevel++;
        Destroy(levelBarriers[currentLevel-2]);
        displayManager.DisplayProcess(currentLevel);

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

    // Function to scale a value from one range to another
    private float ScaleValue(float value, float minOriginal, float maxOriginal, float minTarget, float maxTarget)
    {
        return ((value - minOriginal) / (maxOriginal - minOriginal)) * (maxTarget - minTarget) + minTarget;
    }
}
