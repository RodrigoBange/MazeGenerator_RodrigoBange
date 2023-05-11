using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform mazeCamera;

    [SerializeField]
    private ParticleSystem pSystem;

    [SerializeField]
    private UIController uiController;

    public bool flyUp;
    private bool moveCamera;

    void Update()
    {
        // Move the slime and platform off the screen
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
    /// Activate the animation for the title screen
    /// </summary>
    public void ActivateTitleAnimation()
    {
        flyUp = true;
        pSystem.Play();
        moveCamera = true;
        Invoke(nameof(ActivateGenerationMenu), 2.8f);
    }

    /// <summary>
    /// Swap from the title screen to the game screen UI 
    /// </summary>
    private void ActivateGenerationMenu()
    {        
        gameObject.SetActive(false);
    }
}
