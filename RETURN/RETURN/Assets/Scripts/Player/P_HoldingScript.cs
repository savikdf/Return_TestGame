using UnityEngine;
using System.Collections;

public class P_HoldingScript : MonoBehaviour {
	public static P_HoldingScript instance;
	public Transform holder;
	bool isGrabbing = false;
	[HideInInspector] public Grabbable grabObj;

	RaycastHit grabHit;

	void Awake(){
		if (!instance)
			instance = this;
		
	}

	void Update(){
		if (Input.GetButtonDown ("Grab")) {
			Grab ();
		}
		if (Input.GetButtonUp ("Grab")) {
			LetGo ();
		}
	}

	public void Grab(){
		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out grabHit) && !isGrabbing) {				//raycast out of cam
			if (grabHit.collider.gameObject.GetComponent<Grabbable> () != null) {											//checks if object has grabable class attatche
				//Debug.Log ("Grabbed.");
				grabObj = grabHit.collider.transform.GetComponent<Grabbable> ();
				if (grabObj.canGrab) {																					//checks if can be grabbed
					isGrabbing = true;
					grabObj.StartGrab ();	//starts grab
				}
			} else {
				//Debug.Log ("Cant Grab that.");
			}
		} else {
			//Debug.Log ("Nothing to grab.");
		}
	}

	void LetGo(){
		if (grabObj != null) {
			grabObj.EndGrab ();
			isGrabbing = false;
		}
	}

	public void ForceLetGo(){
		if (grabObj) {
			grabObj.ForceEndGrab ();	//forcefully drops the obj
			isGrabbing = false;
		}
	}

}
