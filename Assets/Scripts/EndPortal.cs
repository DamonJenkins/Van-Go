using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPortal : Portal
{
	public override void TeleportTraveller(PortalableObject _traveller)
	{
		if (_traveller.name == "Player") {
			SceneManager.LoadScene("EndMenu");
		}

		FindObjectOfType<GameManager>().StopTimer();

	}
}
