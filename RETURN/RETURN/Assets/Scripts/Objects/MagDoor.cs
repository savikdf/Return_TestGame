using UnityEngine;
using System.Collections;

public class MagDoor : MonoBehaviour {
	public GameObject[] rings = new GameObject[3];
	public GameObject[] chunklets;
	public BoxCollider barrier;
	public GameObject weirdSphere;

	public bool doorIsLocked = true;
	bool isUnlocking = false;
	float rotateSpeed = .6f;
	float unlockSpeed = 4f;
	Grabbable unlockMag = null;

	void Start(){
		StartCoroutine (DoorIsActive ());
	}

	IEnumerator DoorIsActive(){
		while (doorIsLocked) {
			//rotates shit
			int temp = 1;
			for (int i = 0; i< rings.Length; i++){
				rings [i].transform.Rotate (0, rotateSpeed * temp, 0);
				temp *= -1;
			}
			yield return null;
		}
		//starts unlock animation
		isUnlocking = true;
		StartCoroutine(UnlockAnim());
		yield return new WaitForSeconds(1f);
		StopCoroutine (UnlockAnim ());
		UnlockDoor();
	}



	IEnumerator UnlockAnim(){
		while (isUnlocking) {
			//lerps all rotating pieces to their starting positions
			for (int i = 0; i< rings.Length; i++){
				Vector3 currentRot = rings[i].transform.eulerAngles;
				Vector3 newRot = new Vector3 (
					Mathf.LerpAngle(currentRot.x, 90, Time.deltaTime * unlockSpeed), 
					Mathf.LerpAngle(currentRot.y, 90, Time.deltaTime * unlockSpeed), 
					Mathf.LerpAngle(currentRot.z, 0, Time.deltaTime * unlockSpeed)
				);
				rings [i].transform.eulerAngles = newRot;
				if(weirdSphere)
					weirdSphere.transform.localScale -= new Vector3 (0.005f, 0.005f, 0.005f);
			}
			yield return null;
		}
	}

	void OnTriggerEnter(Collider col){
		if (doorIsLocked && col.gameObject.layer == LayerMask.NameToLayer("Mags")) {
			//tells player to let go
			unlockMag = P_HoldingScript.instance.grabObj;	//magnet to use
			P_HoldingScript.instance.ForceLetGo();
			//puts it in middle
			doorIsLocked = false;

		}
	}

	public void UnlockDoor(){
		barrier.enabled = false;
		Destroy (weirdSphere);
		if (doorIsLocked) {
		} else {
			for (int i = 0; i < chunklets.Length; i++) {			
				chunklets [i].transform.gameObject.AddComponent<Rigidbody> ();
				chunklets [i].transform.SetParent (null);
			}
			doorIsLocked = false;
		}

	}




}
