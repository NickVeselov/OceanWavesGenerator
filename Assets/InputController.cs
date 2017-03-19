using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputController : MonoBehaviour {
     public GameObject objectToLookAt;
     private Transform target;

     float zoomZoom;
     float disp;

     // Use this for initialization
     void Start () {
          target = objectToLookAt.transform;

          zoomZoom = Camera.main.fieldOfView;
          disp = 0;
     }
	
	// Update is called once per frame
	void LateUpdate () {
          if (Input.GetKeyDown(KeyCode.Q))
               disp = 1;
          if (Input.GetKeyDown(KeyCode.E))
               disp = -1;

          if (Input.GetKeyUp(KeyCode.Q))
               disp = 0;
          if (Input.GetKeyUp(KeyCode.E))
               disp = 0;

          transform.RotateAround(target.position, Vector3.up, disp);          

          zoomZoom -= Input.GetAxis("Mouse ScrollWheel") * 10f;
          Camera.main.fieldOfView = Mathf.Clamp(zoomZoom, 30f, 60f);
     }
}
