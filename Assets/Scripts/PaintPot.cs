using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPot : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.contacts[0].normal.y > 0.98f){
            GetComponent<Rigidbody>().velocity = new Vector3();
        }
    }
}
