using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalableObject : MonoBehaviour
{
    public Vector3 previousOffsetFromPortal;

    // Start is called before the first frame update
    public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    public virtual void EnterPortalThreshold()
    {

    }

    public virtual void ExitPortalThreshold()
    {

    }
}
