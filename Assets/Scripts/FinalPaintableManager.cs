using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPaintableManager : PaintableManager
{
    public override void Start()
    {
        managedPaintables = new List<Paintable>(FindObjectsOfType<Paintable>());

        base.Start();
    }
}
