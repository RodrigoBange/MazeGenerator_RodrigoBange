using UnityEngine;

public class TitleObjectsController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform mazeCamera;

    [SerializeField]
    private ParticleSystem pSystemFly;

    [SerializeField]
    private GameObject landCollider;

    private bool flyUp, moveCamera;

    private Vector3 startSlimePos, startCameraPos;

    private void Awake()
    {
        // Save values.
        startSlimePos = rb.gameObject.transform.position;
        startCameraPos = mazeCamera.position;
    }

    private void OnEnable()
    {
        // Set default values.
        flyUp = false;
        moveCamera = false;
        landCollider.SetActive(true);
        rb.gameObject.transform.position = startSlimePos;
        mazeCamera.position = startCameraPos;
    }

    void Update()
    {
        // Move the slime and platform off the screen.
        if (flyUp)
        {            
            rb.velocity = new Vector3(transform.position.x, 3f, transform.position.z);            
        }        
        if (moveCamera)
        {
            mazeCamera.Translate(0.8f * Time.deltaTime * Vector3.up);
        }
    }

    /// <summary>
    /// Activate the animation for the title screen.
    /// </summary>
    public void ActivateTitleAnimation()
    {
        flyUp = true;
        pSystemFly.Play();
        moveCamera = true;
        Invoke(nameof(DisableObjects), 2f);
    }

    /// <summary>
    /// Swap from the title screen to the game screen UI.
    /// </summary>
    private void DisableObjects()
    {
        gameObject.SetActive(false);
    }
}
