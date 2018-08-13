using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolatedController : MonoBehaviour, NodeMotionTarget
{

    public RigNodeMotion.Update interpolatedControllerUpdate;

    [SerializeField]
    private RigNodeMotion currentMotion = new RigNodeMotion();

    private RigNodeMotion lastMotion = new RigNodeMotion();

    private RigNodeMotion motionInput = new RigNodeMotion();

    // public override void ControllerUpdate(ControllerMotion controller)
    public void MotionUpdate(RigNodeMotion controller)
    {
        motionInput = controller;
        this.currentMotion = new RigNodeMotion(controller);
        if (motionInput.velocity == Vector3.zero)
        {
            currentMotion.velocity = (currentMotion.position - lastMotion.position) / Time.deltaTime;
        }

        if (motionInput.acceleration == Vector3.zero)
        {
            currentMotion.acceleration = (currentMotion.velocity - lastMotion.velocity) / Time.deltaTime;
        }

        if (motionInput.angularVelocity == Vector3.zero)
        {
            currentMotion.angularVelocity = Quaternion.LerpUnclamped(Quaternion.identity, currentMotion.rotation * Quaternion.Inverse(lastMotion.rotation), 1 / Time.deltaTime).eulerAngles;
            currentMotion.angularVelocity.x = currentMotion.angularVelocity.x > 180f ? currentMotion.angularVelocity.x - 360f : currentMotion.angularVelocity.x;
            currentMotion.angularVelocity.y = currentMotion.angularVelocity.y > 180f ? currentMotion.angularVelocity.y - 360f : currentMotion.angularVelocity.x;
            currentMotion.angularVelocity.z = currentMotion.angularVelocity.z > 180f ? currentMotion.angularVelocity.z - 360f : currentMotion.angularVelocity.x;
        }

        if (motionInput.angularAcceleration == Vector3.zero)
        {
            currentMotion.angularAcceleration = currentMotion.angularVelocity - lastMotion.angularVelocity / Time.deltaTime;
        }
        lastMotion = new RigNodeMotion(currentMotion);
        interpolatedControllerUpdate.Invoke(currentMotion);
    }
}
