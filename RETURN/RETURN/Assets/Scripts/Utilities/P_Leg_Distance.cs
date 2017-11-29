using UnityEngine;
using System.Collections;

public class P_Leg_Distance : MonoBehaviour {

    public  Transform RupperLeg, RlowerLeg, RFoot;
    public  Transform LupperLeg, LlowerLeg, LFoot;
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
        float r1 = Utilities_Class.distance3D(RupperLeg.position, RlowerLeg.position);
        float r2 = Utilities_Class.distance3D(RlowerLeg.position, RFoot.position);
        float rfinal = r1 + r2;
        r = rfinal;
        float l1 = Utilities_Class.distance3D(LupperLeg.position, LlowerLeg.position);
        float l2 = Utilities_Class.distance3D(LlowerLeg.position, LFoot.position);
        float lfinal = l1 + l2;
        l = lfinal;
    }
    // c===============3
    // c===============3
    // c===============3
    void CalculateDistance()
    {
        UpperCentre = Utilities_Class.Centre3D(LupperLeg.position, RupperLeg.position);

        if (IKControl.leftFootIK && IKControl.rightFootIK)
        {
            //HCentre = Utilities_Class.Centre3D(Lhand.position, RHand.position);
            HCentre = Utilities_Class.Centre3D(IKControl.leftFootPos, IKControl.rightFootPos);
            c = Utilities_Class.distance3D(HCentre, UpperCentre);
        }
        else if(IKControl.leftFootIK && !IKControl.rightFootIK)
        {
            // HCentre = Lhand.position;
            HCentre = IKControl.leftFootPos;
            c = Utilities_Class.distance3D(HCentre, LupperLeg.position);
        }
        else if (!IKControl.leftFootIK && IKControl.rightFootIK)
        {
            //HCentre = RHand.position;
            HCentre = IKControl.rightFootPos;
            c = Utilities_Class.distance3D(HCentre, LupperLeg.position);
        }
        else
        {
            HCentre = Utilities_Class.Centre3D(LFoot.position, RFoot.position);
            c = Utilities_Class.distance3D(HCentre, UpperCentre);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(HCentre, 0.05f);
        Gizmos.DrawSphere(UpperCentre, 0.05f);
        Gizmos.DrawLine(HCentre, UpperCentre);

        Gizmos.color = Color.white;
        //RightArm
        Gizmos.DrawSphere(RupperLeg.position, .05f);
        Gizmos.DrawLine(RupperLeg.position, RlowerLeg.position);
        Gizmos.DrawSphere(RlowerLeg.position, .05f);
        Gizmos.DrawLine(RlowerLeg.position, RFoot.position);
        Gizmos.DrawSphere(RFoot.position, .05f);

        //RightArm
        Gizmos.DrawSphere(LupperLeg.position, .05f);
        Gizmos.DrawLine(LupperLeg.position, LlowerLeg.position);
        Gizmos.DrawSphere(LlowerLeg.position, .05f);
        Gizmos.DrawLine(LlowerLeg.position, LFoot.position);
        Gizmos.DrawSphere(LFoot.position, .05f);
    }
}
