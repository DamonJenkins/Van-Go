using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    static List<Portal> allPortals;

    public static void UpdatePortalFOV() {
		if (allPortals == null) return;
        for (int i = 0; i < allPortals.Count; i++) {
            allPortals[i].UpdateFOV();
        }
    }

    [SerializeField]
    Portal linkedPortal;
    [SerializeField]
    public MeshRenderer screen;
    Camera playerCam;
    public Camera portalCam;
    public RenderTexture viewTexture;
    public RenderTexture camTexture;
    MeshFilter screenMeshFilter;


	[SerializeField]
	Material activeMat;
	[SerializeField]
	Material inactiveMat;

    [SerializeField]
	bool activated = true;

    List<PortalableObject> trackedTravellers;

    void Start()
    {
        if (allPortals == null) {
            allPortals = new List<Portal>();
        }

        playerCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        portalCam = GetComponentInChildren<Camera>();
        portalCam.fieldOfView = PlayerPrefs.GetFloat("fieldOfView", playerCam.fieldOfView);
        portalCam.enabled = false;

        trackedTravellers = new List<PortalableObject>();
        ProtectScreenFromClipping();

        allPortals.Add(this);

        if (!activated) Deactivate();
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
        if (!activated) return;

        if (linkedPortal == null) {
            Deactivate();
            return;
        }

        for (int i = 0; i < trackedTravellers.Count; i++) {
            PortalableObject traveller = trackedTravellers[i];
            Transform travellerT = traveller.transform;

            Vector3 offsetFromPortal = travellerT.position - transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(transform.forward, offsetFromPortal));
            int portalSideOld = System.Math.Sign(Vector3.Dot(transform.forward, traveller.previousOffsetFromPortal));

            if (portalSide != portalSideOld && traveller.enabled)
            {
                Matrix4x4 m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                traveller.Teleport(transform, linkedPortal.transform, m.GetColumn(3), m.rotation);

                //linkedPortal.OnTravellerEnterPortal(traveller);
                trackedTravellers.RemoveAt(i);
                i--;
            }
            else {
                traveller.previousOffsetFromPortal = offsetFromPortal;
            }
        }
    }

    void CreateViewTexture()
    {
        if (linkedPortal.camTexture == null || linkedPortal.camTexture.width != Screen.width || linkedPortal.camTexture.height != Screen.height)
        {
            if (linkedPortal.camTexture != null)
            {
                linkedPortal.camTexture.Release();
            }

            linkedPortal.camTexture = new RenderTexture(Screen.width, Screen.height, 0);

            linkedPortal.portalCam.targetTexture = linkedPortal.camTexture;
        }

        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }

            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);

            if (activated) screen.material.SetTexture("_MainTex", viewTexture);
        }
    }


    public void Render()
    {
        if (linkedPortal == null) return;

        if (!CameraUtility.VisibleFromCamera(screen, playerCam))
        {
            return;
        }

        linkedPortal.screen.enabled = false;
        CreateViewTexture();

        Matrix4x4 m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        linkedPortal.portalCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        linkedPortal.portalCam.Render();

        Graphics.Blit(linkedPortal.portalCam.targetTexture, viewTexture);

        linkedPortal.screen.enabled = true;
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

    private void OnDestroy()
    {
        allPortals.Remove(this);
    }

    public bool ToggleActive(){
        if (activated) Deactivate();
        else Activate();

        return activated;
    }

    public void Activate() {
        activated = true;
		screen.gameObject.SetActive(true);


		RefreshTexture();

        screen.material = activeMat;
        //screen.GetComponent<BoxCollider>().isTrigger = true;
    }

    public void Deactivate() {
        activated = false;
        screen.gameObject.SetActive(false);

        //screen.material = inactiveMat;
        //GetComponentInChildren<BoxCollider>().isTrigger = false;
    }

    public void SwitchTarget(Portal _newPortal) {
        linkedPortal = _newPortal;

        Activate();
    }

    public void RefreshTexture() {
        Destroy(viewTexture);
    }

    private void UpdateFOV() {
        portalCam.fieldOfView = PlayerPrefs.GetFloat("fieldOfView", playerCam.fieldOfView);
    }
}
