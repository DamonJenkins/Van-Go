using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    Portal linkedPortal;
    [SerializeField]
    public MeshRenderer screen;
    Camera playerCam, portalCam;
    RenderTexture viewTexture;
    MeshFilter screenMeshFilter;

    List<PortalableObject> trackedTravellers;

    void Awake()
    {
        playerCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = false;

        screen.material.SetInt("displayMask", 1);

        trackedTravellers = new List<PortalableObject>();
        ProtectScreenFromClipping();
    }

    void ProtectScreenFromClipping()
    {
        float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;

        Transform screenT = screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - playerCam.transform.position) > 0;
        screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, dstToNearClipPlaneCorner);
        screenT.localPosition = Vector3.forward * dstToNearClipPlaneCorner * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < trackedTravellers.Count; i++) {
            PortalableObject traveller = trackedTravellers[i];
            Transform travellerT = traveller.transform;

            Vector3 offsetFromPortal = travellerT.position - transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
            int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.previousOffsetFromPortal, transform.forward));

            if (portalSide != portalSideOld) {
                Matrix4x4 m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                traveller.Teleport(transform, linkedPortal.transform, m.GetColumn(3), m.rotation);

                linkedPortal.OnTravellerEnterPortal(traveller);
                trackedTravellers.RemoveAt(i);
                i--;
            }

            traveller.previousOffsetFromPortal = offsetFromPortal;
        }
    }

    void CreateViewTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }
        }

        viewTexture = new RenderTexture(Screen.width, Screen.height, 0);

        portalCam.targetTexture = viewTexture;

        linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);

    }


    public void Render()
    {
        if (!CameraUtility.VisibleFromCamera(linkedPortal.screen, playerCam))
        {
            return;
        }

        screen.enabled = false;
        CreateViewTexture();

        Matrix4x4 m = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        portalCam.Render();

        screen.enabled = true;
    }

    void OnTravellerEnterPortal(PortalableObject traveller)
    {
        if (!trackedTravellers.Contains(traveller)) {
            traveller.EnterPortalThreshold();
            traveller.previousOffsetFromPortal = traveller.transform.position - transform.position;
            trackedTravellers.Add(traveller);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PortalableObject traveller = other.GetComponent<PortalableObject>();

        if (traveller)
        {
            OnTravellerEnterPortal(traveller);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PortalableObject traveller = other.GetComponent<PortalableObject>();

        if(traveller && trackedTravellers.Contains(traveller))
        {
            traveller.ExitPortalThreshold();
            trackedTravellers.Remove(traveller);
        }
    }
}
