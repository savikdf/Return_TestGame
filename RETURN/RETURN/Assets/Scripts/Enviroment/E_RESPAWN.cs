using UnityEngine;
using System.Collections;

public class E_RESPAWN : MonoBehaviour
{
    public Transform resetPoint;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Coliding");
        col.transform.position = resetPoint.transform.position;
    }
}
