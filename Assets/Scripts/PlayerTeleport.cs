using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
	private bool isHoldingPot;
	private float throwForce;

	[SerializeField]
	private GameObject pot;

    // Start is called before the first frame update
    void Start()
    {
		throwForce = GetComponent<PlayerMovement>().GetThrowForce();

		Debug.Assert(pot != null, "No pot attached to player");
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetAxis("ThrowPot") > 0)
		{
			if (isHoldingPot)
			{
				//Throw pot

				pot.transform.parent = null;

				pot.GetComponent<Rigidbody>().AddForce(GetComponentInChildren<Camera>().transform.forward * throwForce);
			}
			else {
				//Teleport to pot
				transform.position = pot.transform.position;
				ReAttachPot();
			}
		}
		else if (Input.GetAxis("RegrabPot") > 0 && !isHoldingPot) {
			ReAttachPot();
		}
    }

	private void ReAttachPot() {
		//pot.transform.position = theBone.transform.position;
		//pot.transform.parent = theBone.transform;
	}
}
