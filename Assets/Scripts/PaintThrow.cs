using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintThrow : MonoBehaviour
{

    [SerializeField]
    private GameObject paintGlob;
    [SerializeField]
    private GameObject paintBrush;
    private float throwForce;
    [SerializeField]
    private int maxStrokes = 2;
    private int strokeLeft;

    // Start is called before the first frame update
    void Start()
    {
        throwForce = GetComponent<PlayerMovement>().GetThrowForce();
        paintGlob.SetActive(false);
        strokeLeft = maxStrokes;
    }

    // Update is called once per frame
    void Update()
    {

        if (isHoldingPot) strokeLeft = maxStrokes;

        if (Input.GetButtonDown("") && !paintGlob.activeSelf && maxStrokes > 0)
        {
            paintGlob.SetActive(true);
            paintGlob.transform.position = paintBrush.transform.position;
            paintGlob.GetComponent<Rigidbody>().velocity = GetComponentInChildren<Camera>().transform.forward * throwForce;
            strokeLeft--;
        }
    }
}
