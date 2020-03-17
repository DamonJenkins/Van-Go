using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class Paintable : MonoBehaviour
{

    MeshRenderer obj_renderer;
    [SerializeField]
    Material mat_unpainted, mat_painted;

    public UnityEvent managerEvent;

    bool painted = false;

    // Start is called before the first frame update
    void Start()
    {
        obj_renderer = GetComponent<MeshRenderer>();
        obj_renderer.material= mat_unpainted;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsPainted(){
        return painted;
    }

    public void Paint()
    {
        painted = true;
        obj_renderer.material = mat_painted;
        managerEvent.Invoke();
    }
}
