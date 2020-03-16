using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    [SerializeField]
    private float speed = 12f,
    maxSpeed = 12f,
    baseSpeed = 8f,
    gravity = -25f,
    jumpHeight = 2f,
    acceleration = 0.5f,
    decelleration = 1.0f,
    throwForce = 10f;

	[SerializeField]
	private GameObject pot;
	private bool isHoldingPot = true;
	[SerializeField]
	private GameObject handBone;

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

	//Lerp Info
	private Vector3 startMarker;
	private Vector3 endMarker;
	private float t = 0.0f;

    private float timerLimit = 0.05f;

	private Vector3 originalPotPos;
	private Quaternion originalPotRot;


	// Start is called before the first frame update
	void Start()
    {
		Debug.Assert(pot != null, "No pot attached to player");

		originalPotPos = pot.transform.localPosition;
		originalPotRot = pot.transform.localRotation;
		ReAttachPot();

		throwForce = GetComponent<PlayerMovement>().GetThrowForce();
		//paintGlob.SetActive(false);
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

		bool shouldBeRunning = (Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f);

		GetComponent<Animator>().SetBool("Running", shouldBeRunning);

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

		GetComponent<Animator>().SetFloat("RunSpeed", speed/10.0f);

		if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

		if(t > timerLimit) {

			if (Input.GetButtonDown("ThrowPot"))
			{
				if (isHoldingPot)
				{
					//Throw pot
					isHoldingPot = false;
					pot.transform.parent = null;

					pot.GetComponent<BoxCollider>().isTrigger = false;
					pot.GetComponent<Rigidbody>().isKinematic = false;
					pot.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
					pot.GetComponent<PortalableObject>().enabled = true;

					pot.GetComponent<Rigidbody>().AddForce(GetComponentInChildren<Camera>().transform.forward * throwForce);
				}
				else
				{
                    //Teleport to pot
                    startMarker = transform.position;
                    endMarker = pot.transform.position;
					transform.rotation = (transform.rotation * pot.GetComponent<PortalableObject>().portalRot).normalized;
					t = 0.0f;

                    //controller.enabled = false;
                    //transform.position = pot.transform.position;
                    //controller.enabled = true;


                    ReAttachPot();
				}
			}
			else if (Input.GetButtonDown("RegrabPot") && !isHoldingPot)
			{
				ReAttachPot();
			}
		}

		if (isHoldingPot) strokeLeft = maxStrokes;

		if (Input.GetButtonDown("Fire1") && /*!paintGlob.activeSelf &&*/ maxStrokes > 0)
		{
			//paintGlob.SetActive(true);
			//paintGlob.transform.position = paintBrush.transform.position;
			//paintGlob.GetComponent<Rigidbody>().velocity = GetComponentInChildren<Camera>().transform.forward * throwForce;
			strokeLeft--;

			GetComponent<Animator>().SetTrigger("Fire");
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			FindObjectsOfType<Portal>()[2].ToggleActive();
		}
	}


	private void FixedUpdate()
	{
		if (t <= timerLimit) {
			transform.position = Vector3.Lerp(startMarker, endMarker, Mathf.Clamp(t * (1.2f / timerLimit), 0.0f, 1.0f));
			t += Time.deltaTime;
		}
	}

	private void ReAttachPot()
	{

		pot.GetComponent<PaintPot>().drawIcon = false;

		isHoldingPot = true;

		pot.transform.parent = handBone.transform;
		pot.transform.localPosition = originalPotPos;
		pot.transform.localRotation = originalPotRot;
		pot.GetComponent<BoxCollider>().isTrigger = true;
		pot.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		pot.GetComponent<Rigidbody>().isKinematic = true;
		pot.GetComponent<PortalableObject>().enabled = false;
		pot.GetComponent<PortalableObject>().portalRot = Quaternion.identity;
	}

	public float GetThrowForce()
    {
        return throwForce;
    }
}
