using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class P_Object_Detection : MonoBehaviour
{
 
    public List<GameObject> activeObject = new List<GameObject>();
    [HideInInspector]
    public GameObject nearestObj;
    [HideInInspector]
    public Bounds nearestObjBounds;
    [HideInInspector]
    public float distance;
    [HideInInspector]
    public float angle;
    public float interActiveDistance;
    public float fieldOfView;
    SphereCollider detectionRaduis;
    // Use this for initialization

    void Awake ()
    {
        detectionRaduis = GetComponent<SphereCollider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (activeObject.Count > 0)
        {
            SortActiveObjects();
            ManageNearestObject();
        }
        else
        {
            nearestObj = null;
        }
    }



   void SortActiveObjects()
    {
        for (int i = 0; i < activeObject.Count; i++)
        {
            activeObject.Sort(delegate (GameObject a, GameObject b)
            {
                return (Utilities_Class.distance3D(this.transform.position, activeObject[i].transform.position)).
                CompareTo(Utilities_Class.distance3D(this.transform.position, activeObject[(i + 1) % activeObject.Count].transform.position));
            });
        }

    }

   void ManageNearestObject()
    {
        Bounds bounds = activeObject[activeObject.Count - 1].GetComponent<MeshRenderer>().bounds;
        distance = Utilities_Class.distance3D(transform.position, activeObject[activeObject.Count - 1].transform.position);

        if (interActiveDistance >= Utilities_Class.distance3D(transform.position, activeObject[activeObject.Count - 1].transform.position) &&
           transform.root.position.y <=  activeObject[activeObject.Count - 1].transform.position.y + bounds.extents.y)
        {
            nearestObj = activeObject[activeObject.Count - 1];
            nearestObjBounds = bounds;
        }
        else
        {
            nearestObj = null;
        }
    }
    
   void FieldOfViewCheck(GameObject col)
    {

        Vector3 direction = (col.transform.position - transform.position);

        angle = Vector3.Angle(direction, transform.forward);

        //Debug.DrawRay(transform.position + transform.up / 2, direction, Color.green);
        if (angle < fieldOfView)
        {
            //Debug.DrawLine(transform.position + transform.up / 2, direction, Color.red);
            if (!activeObject.Contains(col.gameObject))
            {
                activeObject.Add(col.gameObject);
            }
        }
        else
        {
            activeObject.Remove(col.gameObject);
        }

    }

   void OnTriggerStay(Collider col)
    {
        if (col.tag == "Mag" || col.tag == "InterActive")
        {
            FieldOfViewCheck(col.gameObject);
        }
    }

   void OnTriggerExit(Collider col)
    {
        activeObject.Remove(col.gameObject);
    }

  }
