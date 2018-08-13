using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RigNodeMotion
{
    //TODO add tracked bool to check if the controller is being tracked
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Quaternion rotation;
    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;

    public RigNodeMotion()
    {

    }

    public RigNodeMotion(Vector3 position, Vector3 velocity, Vector3 acceleration, Quaternion rotation, Vector3 angularVelocity, Vector3 angularAcceleration)
    {
        this.position = position;
        this.velocity = velocity;
        this.acceleration = acceleration;
        this.rotation = rotation;
        this.angularVelocity = angularVelocity;
        this.angularAcceleration = angularAcceleration;
    }

    public RigNodeMotion(RigNodeMotion m) : this(m.position, m.velocity, m.acceleration, m.rotation, m.angularVelocity, m.angularAcceleration) { }

    [System.Serializable]
    public class Update : UnityEvent<RigNodeMotion> { }
}