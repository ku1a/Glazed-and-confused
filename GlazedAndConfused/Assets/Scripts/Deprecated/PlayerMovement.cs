using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Obtain basic information about GameObject
    //Rigidbody rb;
    CharacterController controller;

    // Adjustable parameters
    public float speed = 5.0f;
    public float rotspeed = 45.0f;

    public float gravity = -9.18f;
    public float jumpHeight = 3f;
    Vector3 velocity;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();    
    }

    void Update()
    {
        if (controller.isGrounded && velocity.y < 0) //if CheckSphere returns true, and our object is still falling by the numbers
        {
            velocity.y = -2f;
        }

        // Rotation
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotspeed * Time.deltaTime, 0);

        // Forwards / Backwards
        // Not factoring in horizontal movement, because that is used to rotate instead. Movement = direction * magnitude
        Vector3 move = transform.right * Input.GetAxis("Vertical");
        controller.Move(move * speed * Time.deltaTime);
        
        // Gravity
        // (delta y) = g * t^2
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            // y = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

}
