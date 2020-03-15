using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPot : MonoBehaviour
{
    GameObject reticle;
    RaycastHit hitVar;

    private void Start()
    {
        reticle = GameObject.Find("Reticle");
    }

    private void Update()
    {

        if (GetComponent<Rigidbody>().velocity.magnitude > 2f)
        {
            reticle.SetActive(true);
            reticle.transform.rotation = Quaternion.identity;

            Physics.SphereCast(transform.position, 0.5f, new Vector3(0.0f, -1.0f, 0.0f), out hitVar, 2000.0f, LayerMask.GetMask("Ground"));
            reticle.transform.position = transform.position - new Vector3(0.0f, hitVar.distance, 0.0f);

        }
        else {
            reticle.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.contacts[0].normal.y > 0.98f){
            GetComponent<Rigidbody>().velocity = new Vector3();
        }
    }
}
