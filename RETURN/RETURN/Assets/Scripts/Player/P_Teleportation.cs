using UnityEngine;
using System.Collections;

public class P_Teleportation : MonoBehaviour {
	Vector3 teleHitPos;
	RaycastHit teleHit;

	void Start () {
		StartCoroutine (DetectTelePortation ());
	}


	IEnumerator DetectTelePortation(){
		while (Player.instance.isAlive) {
			if (Physics.Raycast (transform.position, Camera.main.transform.forward, out teleHit) && Input.GetButtonDown("Pow")) {
				teleHitPos = teleHit.point;
				TeleportPlayer ();
			}
			yield return null;
		}
	}

	void TeleportPlayer(){
		Player.instance.gameObject.transform.position = teleHitPos + new Vector3 (0, 3, 0);
	}

}
