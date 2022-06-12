//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ParallaxCamera : MonoBehaviour {

//    public Camera mainCamera;

//	// Use this for initialization
//	void Start () {
		
//	}
	
//	// Update is called once per frame
//	void LateUpdate () {
//        UpdateCameras();
//	}

//    public void UpdateCameras()
//    {
//        if (mainCamera == null || farCamera == null || nearCamera == null) return;

//        // orthoSize
//        float a = mainCamera.orthographicSize;
//        // distanceFromOrigin
//        float b = Mathf.Abs(mainCamera.transform.position.z);

//        //change clipping planes based on main camera z-position
//        farCamera.nearClipPlane = b;
//        farCamera.farClipPlane = mainCamera.farClipPlane;
//        nearCamera.farClipPlane = b;
//        nearCamera.nearClipPlane = mainCamera.nearClipPlane;

//        //update field fo view for parallax cameras
//        float fieldOfView = Mathf.Atan(a / b) * Mathf.Rad2Deg * 2f;
//        nearCamera.fieldOfView = farCamera.fieldOfView = fieldOfView;
//    }
//}
