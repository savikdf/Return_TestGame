using UnityEngine;
using System.Collections;

public class P_Arm_Distance : MonoBehaviour {

    public  Transform RupperArm, RlowerArm, RHand;
    public  Transform LupperArm, LlowerArm, Lhand;
    public static float l, r, c;
    public static Vector3 UpperCentre, HCentre;

    // c===============3
    void Start ()
    {
        CalculateArmLengths();
        CalculateDistance();
    }

    // c===============3
    void Update ()
    {
        CalculateDistance();
    }
    // c===============3
    // c===============3
    // c===============3
    void CalculateArmLengths()
    {
        float r1 = Utilities_Class.distance3D(RupperArm.position, RlowerArm.position);
        float r2 = Utilities_Class.distance3D(RlowerArm.position, RHand.position);
        float rfinal = r1 + r2;
        r = rfinal;
        float l1 = Utilities_Class.distance3D(LupperArm.position, LlowerArm.position);
        float l2 = Utilities_Class.distance3D(LlowerArm.position, Lhand.position);
        float lfinal = l1 + l2;
        l = lfinal;
    }
    // c===============3
    // c===============3
    // c===============3
    void CalculateDistance()
    {
        UpperCentre = Utilities_Class.Centre3D(LupperArm.position, RupperArm.position);

        if (IKControl.leftHandIK && IKControl.rightHandIK)
        {
            //HCentre = Utilities_Class.Centre3D(Lhand.position, RHand.position);
            HCentre = Utilities_Class.Centre3D(IKControl.leftHandPos, IKControl.rightHandPos);
            c = Utilities_Class.distance3D(HCentre, UpperCentre);
        }
        else if(IKControl.leftHandIK && !IKControl.rightHandIK)
        {
            // HCentre = Lhand.position;
            HCentre = IKControl.leftHandPos;
            c = Utilities_Class.distance3D(HCentre, LupperArm.position);
        }
        else if (!IKControl.leftHandIK && IKControl.rightHandIK)
        {
            //HCentre = RHand.position;
            HCentre = IKControl.rightHandPos;
            c = Utilities_Class.distance3D(HCentre, LupperArm.position);
        }
        else
        {
            HCentre = Utilities_Class.Centre3D(Lhand.position, RHand.position);
            c = Utilities_Class.distance3D(HCentre, UpperCentre);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(HCentre, 0.05f);
        Gizmos.DrawSphere(UpperCentre, 0.05f);
        Gizmos.DrawLine(HCentre, UpperCentre);

        Gizmos.color = Color.white;
        //RightArm
        Gizmos.DrawSphere(RupperArm.position, .05f);
        Gizmos.DrawLine(RupperArm.position, RlowerArm.position);
        Gizmos.DrawSphere(RlowerArm.position, .05f);
        Gizmos.DrawLine(RlowerArm.position, RHand.position);
        Gizmos.DrawSphere(RHand.position, .05f);

        //RightArm
        Gizmos.DrawSphere(LupperArm.position, .05f);
        Gizmos.DrawLine(LupperArm.position, LlowerArm.position);
        Gizmos.DrawSphere(LlowerArm.position, .05f);
        Gizmos.DrawLine(LlowerArm.position, Lhand.position);
        Gizmos.DrawSphere(Lhand.position, .05f);
    }
}
