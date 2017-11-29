using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]

public class Grabbable : MonoBehaviour {
	public bool canGrab = true;
	public bool isBeingGrabed = false;

	float decelRate = .2f;
	float decelLimit = .04f;
	Vector3 decelSpeed;
	bool isDecelerating{
		get{ return (GetComponent<Rigidbody> ().velocity.magnitude >= decelLimit);	}
	}

	void Awake(){
		if(GetComponent<Rigidbody>() == null)
			gameObject.AddComponent<Rigidbody> ();

		if (GetComponent<MeshCollider> () ) {
			GetComponent<MeshCollider> ().convex = true;
		}
	}

	public void StartGrab(){
		transform.parent = null;
		isBeingGrabed = true;
		GetComponent<Rigidbody> ().useGravity = false;
		//P_HoldingScript.instance.grabObj.gameObject.transform.SetParent (P_HoldingScript.instance.holder.transform);
		//P_HoldingScript.instance.grabObj.gameObject.transform.localPosition = P_HoldingScript.instance.holder.transform.position;
		StartCoroutine (isHeld ());
	}

	public void EndGrab(){
		//P_HoldingScript.instance.grabObj.gameObject.transform.SetParent (null);
		GetComponent<Rigidbody> ().AddForce (Camera.main.transform.forward * 6f, ForceMode.Impulse);
		isBeingGrabed = false;
	}
	public void ForceEndGrab(){
		isBeingGrabed = false;
	}


	IEnumerator isHeld(){
		while (isBeingGrabed) {
			if (isDecelerating) {
				GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity * decelRate;
			}
			P_HoldingScript.instance.grabObj.gameObject.transform.position = Vector3.Lerp (P_HoldingScript.instance.grabObj.gameObject.transform.position,
																							P_HoldingScript.instance.holder.transform.position, 
																								Time.deltaTime * 7.5f);
			yield return null;
		}
		GetComponent<Rigidbody> ().useGravity = true;
		P_HoldingScript.instance.grabObj = null;
	}

}
