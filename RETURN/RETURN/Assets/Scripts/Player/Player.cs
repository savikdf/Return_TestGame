using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static Player instance;
	[HideInInspector] public bool isAlive = true;

	void Awake () {
		if (!instance)
			instance = this;
	}

	void Start(){
		OnPlayerSpawned ();
	}

	void OnPlayerSpawned(){
		isAlive = true;
		StartCoroutine (P_Controls.instance.CheckPlayerInputs());	//starts checking inputs
		StartCoroutine (P_Controls.instance.CheckPlayerInputs());	//starts checking inputs

    }

	void OnPlayerDeath(){
		isAlive = false;	//this wil end all coroutines related to player checks
	}


}
