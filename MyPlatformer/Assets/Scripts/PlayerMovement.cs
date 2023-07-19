using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 6;
    public float rotationSpeed = 750;
    public float jumpSpeed = 12;
    public float jumpButtonGracePeriod = .2f;

    private Animator animator;
    private CharacterController characterController;//handles most of the difficult math problems for me...
    //private float originalstepOffset;//used for a bug that the character controller might have that causes weird collision when jumping into walls (bug might have been fixed)
    private float ySpeed;
    private float gravityMultiplier = 3;
    private float? lastGroundedTime;//question mark means that this float can either be null or have an actual value
    private float? jumpButtonPressedTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();//this will grab the unity character controller component that is attached to the player
        //originalstepOffset = characterController.stepOffset;//for a weird unity controller bug (might be fixed)
    }

    // Update is called once per frame
    void Update()
    {
        ////player walking movement

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = movementDirection.magnitude;// allows player to move slower if not fully pushing the controller stick in any direction
        magnitude = Mathf.Clamp01(magnitude) * speed;//makes sure magnitude is never over 1
        movementDirection.Normalize();//helps prevent the player from moving faster(having a magnitude over 1) than intended when walking diagonally

        ////player jump
        ySpeed += Physics.gravity.y * Time.deltaTime * gravityMultiplier;//this add the players jump speed to gravity and also controls how stong gravity is

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
            if(animator.GetBool("Grounded") != true)
            {
                animator.SetBool("Grounded", true);
            }
        }
        else
        {
            if (animator.GetBool("Grounded") == true)
            {
                animator.SetBool("Grounded", false);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            ySpeed = -0.5f;//its set to -.5 b/c for some reason setting it to 0 makes the player stick to the ground and sometime wont jump when you tell it to

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                //characterController.stepOffset = originalstepOffset;//for a unity controller bug
                animator.SetTrigger("Jump");
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        //else
        //{
        //    characterController.stepOffset = 0;//for a unity controller bug
        //}

        //player velocity (adding player movement with jumping into Vector3 velocity)
        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);//NOTE: dont need to multiply by deltaTime b/c its already accounted for in the SimpleMove method

        ////player movement rotation
        if(movementDirection != Vector3.zero)//checks if player is moving
        {
            animator.SetBool("Running", true);

            ////rotation with movement direction

            //Space.World seems neccessary for player rotation to work properly with movement direction
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed  * Time.deltaTime);

        }
        else
        {
            animator.SetBool("Running", false);
        }
    }
}
