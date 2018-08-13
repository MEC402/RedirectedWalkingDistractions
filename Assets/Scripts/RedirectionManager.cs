using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Tavi {
	public class RedirectionManager : MonoBehaviour {
		public Transform walkingSpace;

		public Transform gameSpace;

		public Transform player;

		void Update() {
			Vector3 headPosition = InputTracking.GetLocalPosition(XRNode.CenterEye);
			Quaternion headRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);

			Matrix4x4 cameraReset = Matrix4x4.TRS(-headPosition, Quaternion.Inverse(headRotation), Vector3.one);

			Matrix4x4 worldToWalking = walkingSpace.worldToLocalMatrix * cameraReset;
			Matrix4x4 walkingToGame = walkingSpace.localToWorldMatrix * gameSpace.worldToLocalMatrix;

			Vector3 walkingHeadPosition = worldToWalking * headPosition;
			Quaternion walkingHeadRotation = Quaternion.Euler(worldToWalking * worldToWalking.MultiplyVector(headRotation.eulerAngles));
			Vector3 gameHeadPosition = walkingToGame * walkingHeadPosition;
			Quaternion gameHeadRotation = Quaternion.Euler(walkingToGame * walkingToGame.MultiplyVector(walkingHeadRotation.eulerAngles));

			player.localPosition = gameHeadPosition;
			player.localRotation = gameHeadRotation;
		}
	}
}
