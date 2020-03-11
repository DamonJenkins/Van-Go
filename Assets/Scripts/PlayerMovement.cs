using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    [SerializeField]
    private float speed = 12f,
    maxSpeed = 12,
    baseSpeed = 8,
    gravity = -25f,
    jumpHeight = 2f,
    acceleration = 0.5f,
    decelleration = 1.0f,
    throwForce = 10f;

    public Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        

        Vector3 move = ((transform.right * x) + transform.forward * z);

        if(speed < maxSpeed && z > 0)
        {
            speed += acceleration / 50;
        }

        if (z == 0 && speed > baseSpeed)
        {
            speed -= decelleration / 50;
        }


        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        
    }

    public float GetThrowForce()
    {
        return throwForce;
    }
}
