using UnityEngine;
using System.Collections;

public class P_Hand_Controller : MonoBehaviour {



	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
       // Vector3 fwd = transform.InverseTransformDirection(Vector3.right);

        Debug.DrawRay(transform.position, transform.right, Color.red);

	}
}
