using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class P_RigidBody_Physics : MonoBehaviour
{
    Vector3 gravity = new Vector3(0, 9.8f, 0);
    public List<Rigidbody> rigidBodys = new List<Rigidbody>();
    SkinnedMeshRenderer skin;
	// Use this for initialization
	void Awake ()
    {
        rigidBodys = GetComponentsInChildren<Rigidbody>().ToList();
        //EnableRagDoll();
        DisableRagDoll();
    }

    void EnableRagDoll()
    {
        for (int i = 0; i < rigidBodys.Count; i++)
        {
            rigidBodys[i].isKinematic = false;
            rigidBodys[i].detectCollisions = true;
        }
    }

    void DisableRagDoll()
    {
        for (int i = 0; i < rigidBodys.Count; i++)
        {
            if (rigidBodys[i].GetComponent<CharacterJoint>())
            {
                //Destroy(rigidBodys[i].GetComponent<CharacterJoint>());
                //Destroy(rigidBodys[i].GetComponent<Collider>());
                rigidBodys[i].GetComponent<CharacterJoint>().enableCollision = true;
                rigidBodys[i].GetComponent<CharacterJoint>().enablePreprocessing = true;
            }
            
            rigidBodys[i].isKinematic = false;
            rigidBodys[i].position = new Vector3(0, 0, 0);
            rigidBodys[i].detectCollisions = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        for (int i = 0; i < rigidBodys.Count; i++)
        {
            rigidBodys[i].isKinematic = false;
            rigidBodys[i].MovePosition(new Vector3(0, 0, 0));
        }
    }

    void OnCollisionStay(Collision col)
    {

    }

}
