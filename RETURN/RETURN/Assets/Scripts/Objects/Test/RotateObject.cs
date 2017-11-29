using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

    float timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        //magnetBlades.transform.Rotate (0.6f, 0, 0);
        transform.position += new Vector3(Mathf.Sin(timer) / 300f, Mathf.Sin(timer) / 300f, 0);
    }
}
