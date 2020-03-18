using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class Paintable : MonoBehaviour
{

    protected MeshRenderer obj_renderer;
    [SerializeField]
    protected Material mat_unpainted, mat_painted;

    List<UnityEvent> managerEvents;

    protected bool painted = false;

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

    public virtual void Paint()
    {
        painted = true;
        obj_renderer.material = mat_painted;

        for (int i = 0; i < managerEvents.Count; i++) {
            managerEvents[i].Invoke();
        }
    }

    public void AddManagerEvent(UnityEvent _event) {
        managerEvents.Add(_event);
    }
}
