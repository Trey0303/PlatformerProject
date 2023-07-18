using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 5;
    public float rotationSpeed = 750;

    private CharacterController characterController;//handles most of the difficult math problems for me...

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();//this will grab the unity character controller component that is attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        ////player walking movement

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = movementDirection.magnitude;// allows player to move slower if not fully pushing the controller stick in any direction
        magnitude = Mathf.Clamp01(magnitude);//makes sure magnitude is never over 1
        movementDirection.Normalize();//helps prevent the player from moving faster(having a magnitude over 1) than intended when walking diagonally

        characterController.SimpleMove(movementDirection * magnitude * speed);//NOTE: dont need to multiply by deltaTime b/c its already accounted for in the SimpleMove method

        if(movementDirection != Vector3.zero)//checks if player is moving
        {
            ////rotation with movement direction

            //Space.World seems neccessary for player rotation to work properly with movement direction
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed  * Time.deltaTime);

        }
    }
}
