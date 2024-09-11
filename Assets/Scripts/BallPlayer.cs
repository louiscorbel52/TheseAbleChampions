using UnityEngine;

public class BallPlayer : MonoBehaviour
{
    // Sensibilit� de la souris
    public float sensitivity = 10.0f;

    // R�f�rence � la cam�ra principale
    private Camera mainCamera;

    // R�f�rence au Rigidbody de la boule
    private Rigidbody rb;

    public float vitesse;

    public string ballMovementSound = "SD_MVT";

    void Start()
    {
        // R�cup�re la cam�ra principale
        mainCamera = Camera.main;

        // R�cup�re le composant Rigidbody attach� � la boule
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // R�cup�re la position de la souris en coordonn�es d'�cran
        Vector3 mousePosition = Input.mousePosition;

        // Convertit la position de la souris en coordonn�es du monde
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane + 1.0f));

        // Calcule la direction du d�placement
        Vector3 direction = (worldPosition - transform.position);

        // Applique la v�locit� � la boule en fonction de la direction et de la sensibilit�
        rb.velocity = direction * sensitivity;
        vitesse = rb.velocity.y;

        if (vitesse < 0.1f)
        {
            SoundManager.Instance.PlayTrailSound(ballMovementSound);
        }
    }

    
}
