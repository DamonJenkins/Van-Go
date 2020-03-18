using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintPot : MonoBehaviour
{
    GameObject reticle;
    GameObject potIcon;
    RaycastHit hitVar;
    Camera playerCam;
    public bool drawIcon = false;

	[SerializeField]
	ParticleSystem potParticle;

    private void Start()
    {
        reticle = GameObject.Find("Reticle");
        potIcon = GameObject.Find("PotImg");
        playerCam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {

        if (GetComponent<Rigidbody>().velocity.magnitude > 2f)
        {
            reticle.SetActive(true);
            reticle.transform.rotation = Quaternion.identity;

            Physics.SphereCast(transform.position, 0.5f, new Vector3(0.0f, -1.0f, 0.0f), out hitVar, 12000.0f, LayerMask.GetMask("Ground"));
            reticle.transform.position = transform.position - new Vector3(0.0f, hitVar.distance, 0.0f);

			ParticleSystem.EmissionModule emMod = potParticle.emission;

			emMod.enabled = true;

		}
		else {
            reticle.SetActive(false);

            Vector3 potPos = playerCam.WorldToScreenPoint(transform.position);
            RectTransform posRect = potIcon.GetComponent<RectTransform>();

            potIcon.SetActive(potPos.z > 0 && drawIcon);
            potPos.z = posRect.position.z;
            posRect.position = potPos;

			ParticleSystem.EmissionModule emMod = potParticle.emission;

			emMod.enabled = false;
		}
	}

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.contacts[0].normal.y > 0.98f){
            GetComponent<Rigidbody>().velocity = new Vector3();
            drawIcon = true;
        }
    }
}
