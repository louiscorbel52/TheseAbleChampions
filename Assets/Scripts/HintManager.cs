using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    [SerializeField] private float hintDelay;
    // Start is called before the first frame update

    // La coroutine qui est appelée par la classe statique
    public IEnumerator HintCoroutine(int currentLevel, int currentAthlete)
    {
        foreach (GameObject npcGroup in GateOpener.Instance.levelNPC)
        {
            if (npcGroup.activeSelf == true)
            {
                foreach (Transform child in npcGroup.transform)
                {

                    child.GetComponent<BallNPC>().needHint = false;
                }
            }
        }
        // Attend un certain délai avant d'afficher le hint
        yield return new WaitForSeconds(hintDelay);

        if (GateOpener.Instance.currentLevel == currentLevel)
        {
            foreach (Transform child in GateOpener.Instance.levelNPC[currentLevel-1].transform)
            {
                child.GetComponent<BallNPC>().needHint = true;

            }
        }
    }
}
