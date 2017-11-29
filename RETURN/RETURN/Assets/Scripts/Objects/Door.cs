using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public GameObject doorHinge, doorHandle;

	void Awake(){

	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.CompareTag ("Player")) {
		
		}
	}



}
