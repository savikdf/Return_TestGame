using UnityEngine;
using System.Collections;

public class MagBall : MonoBehaviour {
	Rigidbody rb;
	Grabbable grb;
	public bool isHeld = false;

	void Awake(){
		rb = GetComponent<Rigidbody> ();
		grb = GetComponent<Grabbable> ();
		//rb.isKinematic = true;

	}


	public void OnPickUpAndDrop(){
		if (grb.isBeingGrabed) {
			//being picked up
			//rb.isKinematic = false;
		} else if(!grb.isBeingGrabed) {
			//being dropped
			//rb.isKinematic = true;
		}

	}


}
