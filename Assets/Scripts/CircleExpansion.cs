using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleExpansion : MonoBehaviour {

	public Transform head;
	public Transform vrRig;
    public GameObject[] featuresOn = new GameObject[4];
    public GameObject[] featuresOff = new GameObject[4];
    public SteamVR_TrackedObject hand;
    public Material skyWithClouds;
    public float virtualRadius;

    Vector3 previousHeadPosition;
	Quaternion previousHeadRotation;
    private float totalRealRadians = 0;
    private int mode = 1;
    private bool[] featuresEnabled = new bool[4];
    private bool enableClouds = false;
    private Material skyNoClouds;

    // Use this for initialization
    void Start () {
        previousHeadPosition = head.position;
		previousHeadRotation = head.localRotation;
        skyNoClouds = RenderSettings.skybox;
        for (int i=0; i < featuresEnabled.Length; ++i) featuresEnabled[i] = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Get inputs and update mode
        if (hand.index != SteamVR_TrackedObject.EIndex.None)
        {
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)hand.index);
            if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Vector2 touchpad = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
                float deadzone = 0.6f;
                if (touchpad.y > deadzone)  featuresEnabled[0] = !featuresEnabled[0];
                if (touchpad.y < -deadzone) featuresEnabled[2] = !featuresEnabled[2];
                if (touchpad.x > deadzone)  featuresEnabled[1] = !featuresEnabled[1];
                if (touchpad.x < -deadzone) featuresEnabled[3] = !featuresEnabled[3];

                for (int i=0; i<featuresEnabled.Length; ++i)
                {
                    if (featuresOn[i] != null) featuresOn[i].SetActive(featuresEnabled[i]);
                    if (featuresOff[i] != null) featuresOff[i].SetActive(!featuresEnabled[i]);
                }
            }

            if (controller.GetHairTriggerDown()) enableClouds = !enableClouds;
            if (enableClouds) RenderSettings.skybox = skyWithClouds;
            else RenderSettings.skybox = skyNoClouds;
        }

        normal();
    }

    private void normal()
    {
        Vector3 headMovementDelta = head.position - previousHeadPosition;
        Vector2 headMovementDelta2D = new Vector2(headMovementDelta.x, headMovementDelta.z);

        Vector3 rigCenterToHead = head.position - vrRig.position;
        Vector2 rigCenterToHead2D = new Vector2(rigCenterToHead.x, rigCenterToHead.z).normalized;
        Vector2 circleTangent = new Vector2(rigCenterToHead2D.y, -rigCenterToHead2D.x);
        float forwardMovement = headMovementDelta2D.magnitude * Vector2.Dot(headMovementDelta2D.normalized, circleTangent);

        float realRadius = rigCenterToHead2D.magnitude;
        float realRadiansTurned = forwardMovement / realRadius;
        float virtualRadiansTurned = (realRadius * realRadiansTurned) / (virtualRadius);

        vrRig.RotateAround(head.position, Vector3.up, Mathf.Rad2Deg * (virtualRadiansTurned - realRadiansTurned));
        
        previousHeadPosition = head.position;
        previousHeadRotation = head.localRotation;
    }

    private void testSet0()
    {
        Vector3 headMovementDelta = head.position - previousHeadPosition;
        Vector2 headMovementDelta2D = new Vector2(headMovementDelta.x, headMovementDelta.z);

        Vector3 rigCenterToHead = head.position - vrRig.position;
        Vector2 rigCenterToHead2D = new Vector2(rigCenterToHead.x, rigCenterToHead.z).normalized;
        Vector2 circleTangent = new Vector2(rigCenterToHead2D.y, -rigCenterToHead2D.x);
        float forwardMovement = headMovementDelta2D.magnitude * Vector2.Dot(headMovementDelta2D.normalized, circleTangent);

        float realRadius = rigCenterToHead2D.magnitude;
        float realRadiansTurned = forwardMovement / realRadius;
        float virtualRadiansTurned = (realRadius * realRadiansTurned) / (virtualRadius);

        vrRig.RotateAround(head.position, Vector3.up, Mathf.Rad2Deg * (virtualRadiansTurned - realRadiansTurned));

        // Rotate with the rig (move forward)
        if (mode == 0)
        {
            totalRealRadians += virtualRadiansTurned - realRadiansTurned;
            RenderSettings.skybox.SetFloat("_Rotation", -Mathf.Rad2Deg * totalRealRadians);
        }
        // No rotations to sky (best)
        if (mode == 1)
        {
            
        }
        // Rotate with view (backward)
        if (mode == 2)
        {
            totalRealRadians += virtualRadiansTurned;
            RenderSettings.skybox.SetFloat("_Rotation", -Mathf.Rad2Deg * totalRealRadians);
        }
        // opposite rotation (backward)
        if (mode == 3)
        {
            totalRealRadians += virtualRadiansTurned - realRadiansTurned;
            RenderSettings.skybox.SetFloat("_Rotation", Mathf.Rad2Deg * totalRealRadians);
        }
        previousHeadPosition = head.position;
        previousHeadRotation = head.localRotation;
    }

    private void testSet2()
    {
        Vector3 headMovementDelta = head.position - previousHeadPosition;
        Vector2 headMovementDelta2D = new Vector2(headMovementDelta.x, headMovementDelta.z);

        Vector3 rigCenterToHead = head.position - vrRig.position;
        Vector2 rigCenterToHead2D = new Vector2(rigCenterToHead.x, rigCenterToHead.z).normalized;
        Vector2 circleTangent = new Vector2(rigCenterToHead2D.y, -rigCenterToHead2D.x);
        float forwardMovement = headMovementDelta2D.magnitude * Vector2.Dot(headMovementDelta2D.normalized, circleTangent);

        float realRadius = rigCenterToHead2D.magnitude;
        float realRadiansTurned = forwardMovement / realRadius;
        float virtualRadiansTurned = (realRadius * realRadiansTurned) / (virtualRadius);

        vrRig.RotateAround(head.position, Vector3.up, Mathf.Rad2Deg * (virtualRadiansTurned - realRadiansTurned));

        totalRealRadians += virtualRadiansTurned - realRadiansTurned;
        if (mode == 0)
        {
            RenderSettings.skybox.SetFloat("_Rotation", -0.1f * Mathf.Rad2Deg * totalRealRadians);
        }
        if (mode == 1)
        {
            RenderSettings.skybox.SetFloat("_Rotation", 0.0f * Mathf.Rad2Deg * totalRealRadians);
        }
        if (mode == 2)
        {
            RenderSettings.skybox.SetFloat("_Rotation", 0.1f * Mathf.Rad2Deg * totalRealRadians);
        }
        if (mode == 3)
        {
            RenderSettings.skybox.SetFloat("_Rotation", 0.2f * Mathf.Rad2Deg * totalRealRadians);
        }
        previousHeadPosition = head.position;
        previousHeadRotation = head.localRotation;
    }
}
