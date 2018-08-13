using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendAroundCircle : MonoBehaviour {

    private float distance;
    private float widthOffset;
    private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
        distance = transform.position.x;
        widthOffset = transform.position.z;
        initialRotation = transform.rotation;

        EndlessCircleRedirect redirector = FindObjectOfType<EndlessCircleRedirect>();
        float radians = distance / redirector.radius;
        float distanceFromCenter = redirector.radius + widthOffset;
        transform.position = new Vector3(-Mathf.Cos(radians), 0, Mathf.Sin(radians)) * distanceFromCenter;
        transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * radians - 90, 0) * initialRotation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateVisibility(float playerDistance, float visibleRange)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = Mathf.Abs(distance - playerDistance) < visibleRange;
        }
    }
}
