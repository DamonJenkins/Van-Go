using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paintGlobScript : MonoBehaviour
{

    [SerializeField]
    GameObject paintBurst;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1000.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Paintable>())
        {
            collision.transform.GetComponent<Paintable>().Paint();
        }

        Destroy(Instantiate(paintBurst, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal, Vector3.up)), paintBurst.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }


}
