using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private int maximumSpeed = 6;
    [SerializeField]
    private float rotationSpeed = 750;
    [SerializeField]
    private float jumpHeight = 2;

    [SerializeField]
    private float gravityMultiplier = 3f;

    [SerializeField]
    private float jumpButtonGracePeriod = .2f;
    [SerializeField]
    private Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;//handles most of the difficult math problems for me...
    //private float originalstepOffset;//used for a bug that the character controller might have that causes weird collision when jumping into walls (bug might have been fixed)
    private float ySpeed;
    private float? lastGroundedTime;//question mark means that this float can either be null or have an actual value
    private float? jumpButtonPressedTime;
    private bool isSliding;
    private Vector3 slopeSlideVelocity;

    ////NEW INPUT SYSTEM

    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private InputActionReference walkButton;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        walkButton.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        walkButton.action.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(animator == null || animator.isActiveAndEnabled == false)
        {
            Debug.Log("either the animator component is NOT enabled OR there is no animator component attached to THIS gameObject");
        }
        characterController = GetComponent<CharacterController>();//this will grab the unity character controller component that is attached to the player
        //originalstepOffset = characterController.stepOffset;//for a weird unity controller bug (might be fixed)

        if(cameraTransform == null)
        {
            Debug.Log("Player Camera not attached to THIS script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////player walking/running movement

        Vector2 newMovementInputSystemControl = movementControl.action.ReadValue<Vector2>();//gets NEW input system controls
        Vector3 movementDirection = new Vector3(newMovementInputSystemControl.x, 0, newMovementInputSystemControl.y);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);// helps prevent player from moving faster diagonally

        if (walkButton.action.IsPressed())//sets an input so that is possible to walk using a keyboard
        {
            inputMagnitude /= 2;// make player move at half the speed
            Debug.Log("walk");
        }

        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);//0.05 is for damping (to make animation transitions smoother)
        
        float speed = inputMagnitude * maximumSpeed;//sets player movement speed
        if(cameraTransform != null)
        {
            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;//CAMERA stuff

        }
        movementDirection.Normalize();//helps prevent the player from moving faster(having a magnitude over 1) than intended when walking diagonally

        ////player jump
        float gravity = Physics.gravity.y * gravityMultiplier;//controls how stong gravity is

        if(ySpeed > 0 && jumpControl.action.IsPressed() == false)//short jump
        {
            gravity *= 2;
        }

        ySpeed += gravity * Time.deltaTime ;//this add the players jump speed to gravity 

        SetSlopeSlideVelocity();//check if player is standing on a steep slope
        if(slopeSlideVelocity == Vector3.zero)
        {
            isSliding = false;
        }

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
            if(animator != null && animator.GetBool("Grounded") != true)
            {
                animator.SetBool("Grounded", true);
            }
        }
        else
        {
            if (animator != null && animator.GetBool("Grounded") == true)
            {
                animator.SetBool("Grounded", false);
            }
        }

        if (jumpControl.action.IsPressed())//jump input
        {
            jumpButtonPressedTime = Time.time;
            
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)//checks if character has been grounded recently
        {
            if(slopeSlideVelocity != Vector3.zero)
            {
                isSliding = true;
            }

            if(isSliding == false)
            {
                ySpeed = -0.5f;//its set to -.5 b/c for some reason setting it to 0 makes the player stick to the ground and sometime wont jump when you tell it to

            }

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && isSliding == false)//checks if jump button has been pressed recently
            {
                //characterController.stepOffset = originalstepOffset;//for a unity controller bug
                if(animator != null)
                {
                    animator.SetTrigger("Jump");
                }
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);//this takes the height and gravity values and returns the required speed needed to reach the desired height
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        //else
        //{
        //    characterController.stepOffset = 0;//for a unity controller bug
        //}
        
        ////player movement rotation
        if(movementDirection != Vector3.zero)//checks if player is moving
        {
            if (animator != null)
            {
                animator.SetBool("Running", true);
            }

            ////rotation with movement direction

            //Space.World seems neccessary for player rotation to work properly with movement direction
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed  * Time.deltaTime);

        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("Running", false);
            }
        }

        //player velocity (adding player movement with jumping into Vector3 velocity)
        if (isSliding == false)
        {
            Vector3 velocity = movementDirection * speed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);//NOTE: dont need to multiply by deltaTime b/c its already accounted for in the SimpleMove method

        }

        if (isSliding)//sliding physics gets executed
        {
            Vector3 velocity = slopeSlideVelocity;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }

    }

    private void SetSlopeSlideVelocity()
    {
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 5)){
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if(angle >= characterController.slopeLimit)
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0,ySpeed,0), hitInfo.normal);
                return;
            }
        }

        if (isSliding)
        {
            slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * 6;

            if(slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }

        slopeSlideVelocity = Vector3.zero;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
