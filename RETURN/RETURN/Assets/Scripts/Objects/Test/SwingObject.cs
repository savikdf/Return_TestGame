using UnityEngine;
using System.Collections;

public class SwingObject : MonoBehaviour {

    float timer;
    Quaternion rotate;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime /1.5f;
        //magnetBlades.transform.Rotate (0.6f, 0, 0);
        rotate.eulerAngles = new Vector3(0, -90 ,0);
        rotate.eulerAngles += new Vector3 (0, 0, Mathf.Cos(timer) * 45);
        transform.rotation = rotate;
    }
}
