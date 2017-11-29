using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Physics_Animation_Blend : MonoBehaviour
{
    public AnimationCurve blendCurve;

    public Transform AnimationBody;
    public Transform PhysicalBody;

    public static List<Transform> animBones;
    public static List<Transform> bodyBones;

    public bool EnableRagdoll;

	// Use this for initialization
	void Awake ()
    {
        HandleRagDoll();
    }

    void HandleRagDoll()
    {
        for (int i = 0; i < bodyBones.Count; i++)
        {
            if (bodyBones[i].GetComponent<Rigidbody>() && bodyBones[i].GetComponent<CharacterJoint>())
            {
                if (EnableRagdoll)
                {
                    EnableRagDollPhysics(i);
                }
                else
                {
                    DisableRagDollPhysics(i);
                }

                EvaluateCharacterJoint(i);
            }
        }
    }

    void EnableRagDollPhysics(int i)
    {
            bodyBones[i].GetComponent<Rigidbody>().isKinematic = false;
            bodyBones[i].GetComponent<Rigidbody>().detectCollisions = true;
    }

    void DisableRagDollPhysics(int i)
    {    
            bodyBones[i].GetComponent<Rigidbody>().isKinematic = true;
            bodyBones[i].GetComponent<Rigidbody>().detectCollisions = true;
    }

    void EvaluateCharacterJoint(int i)
    {
        bodyBones[i].GetComponent<CharacterJoint>().enablePreprocessing = true;
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    public void MapTransforms()
    {

    }

    //Needs Work
    public void FixedUpdate()
    {
        for(int i = 0; i < animBones.Count; i++)
        {
            bodyBones[i].transform.position = animBones[i].transform.position;
            bodyBones[i].transform.rotation = animBones[i].transform.rotation;
        }
    }

    void OnValidate()
    {
        animBones = AnimationBody.GetComponentsInChildren<Transform>().ToList();
        bodyBones = PhysicalBody.GetComponentsInChildren<Transform>().ToList();
    }
}
