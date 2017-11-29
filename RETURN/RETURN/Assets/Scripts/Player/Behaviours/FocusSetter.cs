using UnityEngine;
using System.Collections;

public class FocusSetter : MonoBehaviour {
	Transform focusObject;
	RaycastHit focuseHit;
	[Range(0.01f, 1f)] public float focusSpeed;

	void Awake(){
		focusObject = transform.GetChild (0).transform;
	}
		
	void LateUpdate () {
		if (Physics.Raycast (transform.position, transform.forward, out focuseHit, Mathf.Infinity)) {
			LerpFocalPoint (focuseHit.point);
		} else {
			LerpFocalPoint (Vector3.zero);
		}
	}

	///Lerps focal point to hitpoint of raycast
	void LerpFocalPoint(Vector3 toPos){
		focusObject.position = Vector3.Lerp (focusObject.position, toPos, focusSpeed);
		Debug.DrawLine (transform.position, focusObject.position, Color.yellow);
	}

}
