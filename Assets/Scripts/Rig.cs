using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class Rig : MonoBehaviour
{
    public RigNodeMotion.Update centerEyeUpdate;
    public RigNodeMotion.Update headUpdate;
    public RigNodeMotion.Update leftHandUpdate;
    public RigNodeMotion.Update rightHandUpdate;

    [SerializeField]
    private RigNodeMotion centerEye = new RigNodeMotion();

    [SerializeField]
    private RigNodeMotion head = new RigNodeMotion();

    [SerializeField]
    private RigNodeMotion leftHand = new RigNodeMotion();
    [SerializeField]
    private RigNodeMotion rightHand = new RigNodeMotion();

    // private Dictionary<XRNode, XRNodeState> nodeStates = new Dictionary<XRNode, XRNodeState>();
    private List<XRNodeState> nodeStates = new List<XRNodeState>();

    void Start()
    {
        centerEye.position = InputTracking.GetLocalPosition(XRNode.CenterEye);
        centerEye.rotation = InputTracking.GetLocalRotation(XRNode.CenterEye);

        head.position = InputTracking.GetLocalPosition(XRNode.Head);
        head.rotation = InputTracking.GetLocalRotation(XRNode.Head);

        leftHand.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
        leftHand.rotation = InputTracking.GetLocalRotation(XRNode.LeftHand);

        rightHand.position = InputTracking.GetLocalPosition(XRNode.RightHand);
        rightHand.rotation = InputTracking.GetLocalRotation(XRNode.RightHand);
    }

    void Update()
    {
        InputTracking.GetNodeStates(nodeStates);
        foreach (XRNodeState state in nodeStates)
        {
            // Debug.Log(state.nodeType);
            RigNodeMotion nodeMotion;
            switch (state.nodeType)
            {
                case XRNode.CenterEye:
                    nodeMotion = centerEye;
                break;
                case XRNode.Head:
                    nodeMotion = head;
                    break;
                case XRNode.LeftHand:
                    nodeMotion = leftHand;
                    break;
                case XRNode.RightHand:
                    nodeMotion = rightHand;
                    break;
                default:
                    nodeMotion = head;
                    break;
            }
            Vector3 tempVec3;

            tempVec3 = nodeMotion.acceleration;
            if (state.TryGetAcceleration(out tempVec3))
            {
                nodeMotion.velocity = nodeMotion.acceleration;
            }

            // TODO remove temp variables and out directly to nodeMotion field
            tempVec3 = nodeMotion.velocity;
            if (state.TryGetVelocity(out tempVec3))
            {
                nodeMotion.velocity = tempVec3;
            }

            tempVec3 = nodeMotion.position;
            if (state.TryGetPosition(out tempVec3))
            {
                nodeMotion.position = tempVec3;
            }


            tempVec3 = nodeMotion.angularAcceleration;
            if (state.TryGetAngularAcceleration(out tempVec3))
            {
                nodeMotion.angularAcceleration = tempVec3;
            }

            tempVec3 = nodeMotion.angularVelocity;
            if (state.TryGetAngularVelocity(out tempVec3))
            {
                nodeMotion.angularVelocity = tempVec3;
            }

            Quaternion tempQuat;

            tempQuat = nodeMotion.rotation;
            if (state.TryGetRotation(out tempQuat))
            {
                nodeMotion.rotation = tempQuat;
            }
        }
        centerEyeUpdate.Invoke(centerEye);
        headUpdate.Invoke(head);
        leftHandUpdate.Invoke(leftHand);
        rightHandUpdate.Invoke(rightHand);
    }
}
