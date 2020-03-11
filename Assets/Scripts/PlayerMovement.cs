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

	[SerializeField]
	private GameObject pot;
	private bool isHoldingPot;

	[SerializeField]
	private GameObject paintGlob;
	[SerializeField]
	private GameObject paintBrush;
	[SerializeField]
	private int maxStrokes = 2;
	private int strokeLeft;

	public Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
		Debug.Assert(pot != null, "No pot attached to player");

		throwForce = GetComponent<PlayerMovement>().GetThrowForce();
		paintGlob.SetActive(false);
		strokeLeft = maxStrokes;
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

		if (Input.GetButtonDown("ThrowPot"))
		{
			if (isHoldingPot)
			{
				//Throw pot

				pot.transform.parent = null;

				pot.GetComponent<Rigidbody>().AddForce(GetComponentInChildren<Camera>().transform.forward * throwForce);
			}
			else
			{
				//Teleport to pot
				transform.position = pot.transform.position;
				ReAttachPot();
			}
		}
		else if (Input.GetButtonDown("RegrabPot") && !isHoldingPot)
		{
			ReAttachPot();
		}

		if (isHoldingPot) strokeLeft = maxStrokes;

		if (Input.GetButtonDown("Fire1") && !paintGlob.activeSelf && maxStrokes > 0)
		{
			paintGlob.SetActive(true);
			paintGlob.transform.position = paintBrush.transform.position;
			paintGlob.GetComponent<Rigidbody>().velocity = GetComponentInChildren<Camera>().transform.forward * throwForce;
			strokeLeft--;
		}
	}

	private void ReAttachPot()
	{
		//pot.transform.position = theBone.transform.position;
		//pot.transform.parent = theBone.transform;
	}

	public float GetThrowForce()
    {
        return throwForce;
    }
}
