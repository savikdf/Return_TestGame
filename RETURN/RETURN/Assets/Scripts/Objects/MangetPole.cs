using UnityEngine;
using System.Collections;

public class MangetPole : MonoBehaviour {

	public GameObject magnetBlades; //magBall;
	float timer;

	void Update(){
		timer += Time.deltaTime;
		//magnetBlades.transform.Rotate (0.6f, 0, 0);
		magnetBlades.transform.position += new Vector3 (0, Mathf.Sin(timer) / 300f, 0);
		//magBall.transform.position += new Vector3 (Mathf.Sin(timer) / 300f, 0, Mathf.Cos(timer) / 300f);

	}




}
