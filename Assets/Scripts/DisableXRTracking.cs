using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DisableXRTracking : MonoBehaviour {

	[SerializeField]
	private new Camera camera;

	[SerializeField]
	private bool disableCamera = true;

	void Start () {
		if(camera == null) {
			camera = gameObject.GetComponentInChildren<Camera>();
		}
		XRDevice.DisableAutoXRCameraTracking(camera, true);
	}
}
