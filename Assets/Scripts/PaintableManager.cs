using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PaintableManager : MonoBehaviour
{
    [SerializeField]
    protected List<Paintable> managedPaintables;
    [SerializeField]
    List<Portal> managedPortals;

    UnityEvent myEvent;

    public virtual void Start()
    {
        myEvent = new UnityEvent();

        myEvent.AddListener(CheckPaintables);

        for (int i = 0; i < managedPaintables.Count; i++) {
            managedPaintables[i].AddManagerEvent(myEvent);
        }
    }

    void CheckPaintables() {
        bool allPainted = true;
        for (int i = 0; i < managedPaintables.Count; i++){
            if (!managedPaintables[i].IsPainted()) {
                allPainted = false;
                break;
            }
        }

        if (allPainted) { 
            for (int i = 0; i < managedPortals.Count; i++){
                managedPortals[i].Activate();
            }
        }
    }
}
