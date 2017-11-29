using UnityEngine;
using System.Collections;

public class CircleMovement : MonoBehaviour {
	public float magnitude = 50;
	float timer = 0;
	public float damping = 3f;
	Vector3 startPos;

	void Start(){
		startPos = transform.position;
		if (damping <= 0) {
			damping = 1f;
		}
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime / damping;
		float xVal = Mathf.Sin (timer) * magnitude;
		float zVal = Mathf.Cos (timer) * magnitude;

		transform.localPosition = new Vector3 (xVal + startPos.x, startPos.y, zVal + startPos.z);

	}
}
