using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this to use the Text component
using TMPro; // Add this to use the TMP_Text component


public class DisplayManager : MonoBehaviour
{
    [SerializeField] GameObject assiaPopup;
    [SerializeField] GameObject oleksandrPopup;
    [SerializeField] GameObject assiaEndGamePopup;
    [SerializeField] GameObject oleksandrEndGamePopup;

    public TMP_Text assiaPopupText; // Reference to the Text component in assiaPopup
    public TMP_Text oleksandrPopupText; // Reference to the Text component in oleksandrPopup

    
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
                if (GateOpener.Instance.currentLevel == 1){
                    assiaPopupText.text = "111111 ";
                }
                else if (GateOpener.Instance.currentLevel == 2){
                    assiaPopupText.text = @"Blind people experience perceptual delays that require the use of guides and/or cues to navigate their environment, which can lead to frustration and an increased sensitivity to mockery. Those who become blind during their lives face the additional challenge of having to learn from scratch how to live as adults. However, engaging in sports helps develop body and spatial awareness, thereby fostering autonomy and self-confidence, which are crucial for a normal life. In particular, outdoor activities enhance anticipation and adaptability.
That is why adapted team sports for visually impaired individuals are played on fixed playing fields, making it easier to integrate the necessary cues for safe play, and why individual sports are performed in tandem with a guide, thus gaining a collective dimension. Thanks to these adaptations, people with visual impairments can thrive in both adapted and mixed sports.";
                }
                else if (GateOpener.Instance.currentLevel == 3){
                    assiaPopupText.text = @"Performing at the highest level is already challenging in itself, but maintaining that level for many years is even harder. Few have succeeded. It proves to be incredibly demanding both mentally and physically. For some, the rigor they impose on themselves becomes a burden, to the point where they can't even accept second place. Others lose the will to win after having already achieved everything. In some extreme cases, even winning becomes irrelevant.

High-level sports require both physical and mental discipline. It demands excellence in every aspect of an athlete's life. Over the last decade, many athletes, both able-bodied and disabled, have opened up about the pressure from the media, the results, and the mental breakdowns they have experienced.";
                }
                else if (GateOpener.Instance.currentLevel == 4){
                    assiaPopupText.text = @"Para-athletes often face a lot of pressure and expectations, some of which, but not all is shared with abled-bodied athletes. Para-athletes represent their nation, and train rigorously for years if not decades, having high hopes for their performance. Para-athletes also represent the disability movement as a whole, more particularly people possessing the same type of disabilities.

Media scrutinity, and narratives that paint disabled athletes as extraordinary and capable of overcoming incredible obstacles can be empowering, but also suffocating and exhausting. This can cause additional pressure to perform and to meet people’s expectations regardless of their realism in particular of their fans and sponsors, even when unrealistic. This additional pressure can impact their well-being and personal life, and can encourage athletes to use harsher training regimens. ";
                }
                
                assiaPopup.SetActive(true);
            }
            else if (GateOpener.Instance.athleteID == 1)
            {
                if (GateOpener.Instance.currentLevel == 1){
                    oleksandrPopupText.text = "O11111r level ";
                }
                else if (GateOpener.Instance.currentLevel == 2){
                    oleksandrPopupText.text = @"In 1933 two mechanical engineers, Harry C. Jennings Sr. and Herbert Everest, who was disabled, invented the first lightweight, steel, folding, portable wheelchair. It gave a boost in the development of its numerous variations, including wheelchairs for sports like basketball, rugby, tennis, racing, and dancing.
While technological inventions are helping disabled people adapt to everyday life, the environment still can impose a lot of restrictions, and this is where lies the biggest problem of all: having the difficulty of moving freely, disabled people are rarely seen, and as a result, often forgotten.
Adapting the built environment to make it more accessible to wheelchair users is one of the key campaigns of disability rights movements and local equality legislatio";
                }
                else if (GateOpener.Instance.currentLevel == 3){
                    oleksandrPopupText.text = @"Ableism is discrimination and social prejudice against people with physical or mental disabilities.
In ableist societies, the lives of disabled people are considered less worth living, or less valuable, even sometimes expendable. The eugenics movement of the early 20th century is considered an expression of widespread ableism.
Even in developed societies with good education systems, there is still a place for misjudgments. Prejudice comes in many ways and forms, but the most unseen is a generalization in media representation. Especially when it comes to sports, disabled people are seen as those who are united by their struggles, and it's often overlooked that this is not what defines peopl";
                }
                else if (GateOpener.Instance.currentLevel == 4){
                    oleksandrPopupText.text = @"Para swimming is an adaptation of the sport of swimming for athletes with disabilities. Para swimmers compete at the Summer Paralympic Games and at other sports competitions throughout the world. The sport is governed by the International Paralympic Committee. Both men and women compete in para swimming, racing against competitors of their own gender.
Swimmers compete individually in backstroke, breaststroke, butterfly, freestyle, individual medley, and as teams in relay races. Significant differences between able-bodied and para swimming include the starting position and adaptations allowed for visually impaired swimmers. Competitors may start a race by standing on a platform and diving into the pool, as in non-disabled swimming, or by sitting on the platform and diving in, or they may start the race in the water";
                }
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
    /*public void RestartScene()
    {
        Debug.Log("Restarting scene...");
        SaveState();
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

    // Method to restart the scene
    public void RestartScene()
    {
        Debug.Log("Restarting scene...");
        SaveState();
        Time.timeScale = 1; // Resume the game
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the sceneLoaded event

        // Find the SoundManager singleton instance
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null)
        {
            var parent = GameObject.Find("ParentAudioSources").transform;
            // Set the audioSource and trailSource parameters
            soundManager.audioSource = parent.Find("AudioSource").GetComponent<AudioSource>();// Assign the appropriate AudioSource
            soundManager.trailSource = parent.Find("TrailSource").GetComponent<AudioSource>();// Assign the appropriate TrailSource
        }
    }


    public void SaveState()
    {
        PlayerPrefs.SetInt("CurrentAthleteID", GateOpener.Instance.athleteID);
        PlayerPrefs.Save();
    }

}
