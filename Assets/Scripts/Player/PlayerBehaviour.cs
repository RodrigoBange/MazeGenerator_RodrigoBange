using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{    
    private CustomInput input = null;
    private Vector3 movementVector = Vector3.zero;
    private Rigidbody rb = null;

    public float moveSpeed = 10f;
    public bool flyUp = true;
    public Vector3 goalPosition = Vector3.zero;
    private bool goalPositionReached = false;

    private Animator animator;

    private void Awake()
    {
        input = new CustomInput();       
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Enable input
        input.Enable();
        input.Player.Movement.performed += OnMovement;
        input.Player.Movement.canceled += OnMovementCancel;
    }

    private void OnDisable()
    {
        // Disable input
        input.Disable();
        input.Player.Movement.performed -= OnMovement;
        input.Player.Movement.canceled -= OnMovementCancel;
    }

    private void OnMovement(InputAction.CallbackContext value)
    {
        movementVector = value.ReadValue<Vector3>();
    }

    private void OnMovementCancel(InputAction.CallbackContext value)
    {
        movementVector = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Movement
        Move();

        // Flies the character up
        FlyUp();
    }

    /// <summary>
    /// Moves the player in a direction.
    /// </summary>
    private void Move()
    {
        rb.velocity = new Vector3(movementVector.x * moveSpeed, rb.velocity.y, movementVector.z * moveSpeed);

        if (movementVector != Vector3.zero)
        {
            Quaternion rotationTo = Quaternion.LookRotation(movementVector, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTo, 720f * Time.deltaTime);
            animator.SetBool("Walk", true); // Animate player
        }    
        else
        {
            animator.SetBool("Walk", false); // Idle Animate player
        }
    }

    /// <summary>
    /// Allows or denies the user the ability to move around.
    /// </summary>
    /// <param name="value">Boolean to enable or disable player movement.</param>
    public void AllowMovement(bool value)
    {
        if (!value)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        } 
        else
        {
            rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;           
        }
    }

    /// <summary>
    /// Makes the player move up vertically. Called after reaching the finish.
    /// </summary>
    private void FlyUp()
    {
        if (goalPosition != Vector3.zero && !goalPositionReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, 0.1f);

            if (Vector3.Distance(transform.position, goalPosition) < 0.01f)
            {
                goalPositionReached = true;
                flyUp = true;
            }
        } else if (flyUp)
        {
            rb.velocity = new Vector3(movementVector.x, 3f, movementVector.z);
        }      
    }
}
