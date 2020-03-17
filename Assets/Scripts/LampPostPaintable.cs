using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPostPaintable : Paintable
{

    Light pointLight;
    ParticleSystem pSystem;

    // Start is called before the first frame update
    void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        pSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Paint()
    {
        base.Paint();
        pointLight.enabled = true;
        pSystem.Play();
    }

}
