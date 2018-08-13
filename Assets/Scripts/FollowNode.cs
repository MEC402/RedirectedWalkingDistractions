using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class FollowController : ControllerMotionTarget
public class FollowNode : MonoBehaviour, NodeMotionTarget
{
    // public override void ControllerUpdate(ControllerMotion controller)
    public void MotionUpdate(RigNodeMotion node)
    {
        transform.localPosition = node.position;
        transform.localRotation = node.rotation;
    }
}
