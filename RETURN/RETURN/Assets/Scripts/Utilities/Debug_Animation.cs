using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Debug_Animation : MonoBehaviour
{
    public bool visulizeTransforms;
    List<Transform> bones = new List<Transform>();
	// Use this for initialization

	void Awake ()
    {
        bones = GetComponentsInChildren<Transform>().ToList();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.F1))
        {
            visulizeTransforms = !visulizeTransforms;
        }
	}

    void OnDrawGizmos()
    {
        if (visulizeTransforms)
        {
            for (int i = 0; i < bones.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(bones[i].transform.position, .05f);
            }
        }
    }
}
