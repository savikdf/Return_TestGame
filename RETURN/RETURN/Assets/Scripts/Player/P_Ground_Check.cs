using UnityEngine;
using System.Collections;

public class P_Ground_Check : MonoBehaviour {

    [Range(-1, 1)] public float YDist = 0f;

    [Range(1, 10)]public float vectorLength = 2f;

    public float SlopeAngleThreshold;

    [Range(0, 360)]
    public float angleX = 0;
    [Range(0, 360)]
    public float angleY = 0;
    [Range(0, 360)]
    public float angleZ = 0;

    [Range(0, 10)]
    public float distToGround = 6f;

    float fwdDst = 2f;

    float bckDst = 2f;

    public Transform pelvis; 
    public Vector3 offset = new Vector3(0,-.5f,0);
    Vector3 CastPos;
    public LayerMask ignoreMask;
    public static bool isGrounded;
    public static bool isBlw, isFwD, isBck, isSlp;
    public static Vector3 groundDir, groundPos;
    public static float groundSlope;

    void FixedUpdate()
    {
        CastPos = new Vector3 (transform.position.x, pelvis.position.y, transform.position.z);

        GroundedCast();
        //isGrounded = isBlw;
        DanceOftheBools();
       // Debug.Log("Grounded ? : " + isGrounded);
    }

    void DanceOftheBools()
    {
        if(isBlw)
        {
            isGrounded = true;
        }
        else if (isFwD)
        {
            isGrounded = true;
        }
        else if(isBck)
        {
            isGrounded = true;
        }
         else
        {
            isGrounded = false;
        }
    }
    Vector3 ParrellSurfaceVector(float angleX, float angleY, float angleZ , Vector3 inputVector)
    {

        //angleRads = angleRads * Mathf.Deg2Rad;
        float P_angle = (Mathf.Deg2Rad * transform.root.rotation.eulerAngles.y) + (150) * Mathf.Deg2Rad;

        ////Get origin coords
        float o_x = inputVector.x;
        float o_y = inputVector.y;
        float o_z = inputVector.z;

        angleX = (angleX * Mathf.Deg2Rad); //+ Mathf.Atan2(o_z, o_y);
        angleY = (angleY * Mathf.Deg2Rad); //+ Mathf.Atan2(o_z, o_x);
        angleZ = (angleZ * Mathf.Deg2Rad); //+ Mathf.Atan2(o_y, o_x);

        //Vector3 outputVector = new Vector3(x1, y1, o_z);
        float inputAngleZ = Mathf.Atan2(o_y, o_x);
        float inputAngleY = Mathf.Atan2(o_x, o_z);
        float inputAngleX = Mathf.Atan2(o_y, o_z);

        float CosZ = Mathf.Cos(angleZ + inputAngleZ);
        float SinZ = Mathf.Sin(angleZ + inputAngleZ);

        float CosX = Mathf.Cos(angleX + inputAngleX);
        float SinX = Mathf.Sin(angleX + inputAngleX);

        float CosY = Mathf.Cos(-P_angle);
        float SinY = Mathf.Sin(-P_angle);

        Vector3 R1 = new Vector3(CosZ*CosY, (CosZ*SinY*SinX)-(SinZ* CosX),(CosZ*SinY*CosX)+(SinZ*SinX));
        Vector3 R2 = new Vector3(SinZ*CosY, (SinZ*SinY*SinX)+(CosZ*CosX), (SinZ*SinY*CosX)-(CosZ*SinX));
        Vector3 R3 = new Vector3(-SinY, CosY*SinX, CosY * CosX);
        float x = R1.x + R1.y + R1.z;
        float y = R2.x + R2.y + R2.z;
        float z = R3.x + R3.y + R3.z;

        Vector3 outputVector = new Vector3(x, y, z);
        return outputVector;
    }

    float SlopAngle(Vector3 inputVector)
    {
        //Get origin coords
        float o_x = inputVector.x;
        float o_y = inputVector.y;

        float angleZ = Mathf.Atan2(o_y, o_x);
 
        return angleZ * Mathf.Rad2Deg;
    }

    float angleConvert(float f)
    {
        if (Mathf.Sign(f) < 1)
        {
            f = (f % 360);
            while (f < 0)
            {
                f += 360;
            }
            return f;

        }
        else
        {
            return f;
        }

    }

    //public void IsGroundedTest()
    //{
    //    RaycastHit groundRay;
    //    RaycastHit FwdgroundRay;
    //    RaycastHit BckgroundRay;


    //    if (Physics.Raycast(CastPos, -transform.up, out groundRay, vectorLength, ignoreMask))
    //    {
    //        Debug.DrawRay(P_Leg_Distance.HCentre - offset, -transform.up * vectorLength, Color.red);

    //        Debug.DrawRay(groundRay.point, groundRay.normal * vectorLength, Color.black);

    //        Debug.DrawRay(groundRay.point, (ParrellSurfaceVector(angleX, angleY, angleZ, groundRay.normal)), Color.cyan);

    //        float s_angle = angleConvert(Mathf.RoundToInt(SlopAngle((ParrellSurfaceVector(angleX, angleY, angleZ, groundRay.normal)))));

    //        Debug.Log(s_angle);

    //    }
    //    else
    //    {
    //        //return false;
    //    }

    //    if (Physics.Raycast(CastPos, -transform.forward, out BckgroundRay, bckDst, ignoreMask))
    //    {
    //        Debug.DrawRay(P_Leg_Distance.HCentre - offset, -transform.forward * bckDst, Color.blue);
    //    }
    //    else
    //    {
    //       // return false;
    //    }

    //    if (Physics.Raycast(CastPos, transform.forward, out FwdgroundRay, fwdDst, ignoreMask))
    //    {
    //        Debug.DrawRay(P_Leg_Distance.HCentre - offset, transform.forward * fwdDst, Color.yellow);
    //    }
    //    else
    //    {
    //        //return false;
    //    }

    //}

    bool CheckSlope(float slope)
    {
        float SlopeAngleThreshold2 = SlopeAngleThreshold + 90;

        if (slope < SlopeAngleThreshold2 || slope > SlopeAngleThreshold)
        {
            return true;
        }
        return false;
    }
    
    public void GroundedCast()
    {
        RaycastHit groundRay;
        RaycastHit FwdgroundRay;
        RaycastHit BckgroundRay;

        Debug.DrawRay(CastPos, -transform.up * distToGround, Color.green);

        if (Physics.Raycast(CastPos, -transform.up, out groundRay, 100, ignoreMask))
        {
            groundSlope = angleConvert(Mathf.RoundToInt(SlopAngle((ParrellSurfaceVector(angleX, angleY, angleZ, groundRay.normal)))));
            Debug.DrawRay(groundRay.point, (ParrellSurfaceVector(angleX, angleY, angleZ, groundRay.normal)), Color.cyan);
            float dist = Utilities_Class.distance3D(transform.position, groundRay.point);

            if (dist < .1f)
            {
                groundPos = groundRay.point;
                isBlw = CheckSlope(groundSlope);
            }
            else
            {
                isBlw = false;
                if (Physics.Raycast(CastPos - offset, -transform.forward, out BckgroundRay, bckDst, ignoreMask))
                {
                    groundSlope = angleConvert(Mathf.RoundToInt(SlopAngle((ParrellSurfaceVector(angleX, angleY, angleZ, BckgroundRay.normal)))));
                    groundPos = groundRay.point;

                    Debug.DrawRay(BckgroundRay.point, (ParrellSurfaceVector(angleX, angleY, angleZ, BckgroundRay.normal)), Color.magenta);
                    Debug.DrawRay(CastPos - offset, -transform.forward * bckDst, Color.red);

                    isBck = CheckSlope(groundSlope);
      
                }
                else
                {
                    Debug.DrawRay(CastPos - offset, -transform.forward * bckDst, Color.blue);
                    isBck = false;
                }

                if (Physics.Raycast(CastPos - offset, transform.forward, out FwdgroundRay, fwdDst, ignoreMask))
                {
                    groundSlope = angleConvert(Mathf.RoundToInt(SlopAngle((ParrellSurfaceVector(angleX, angleY, angleZ, FwdgroundRay.normal)))));
                    groundPos = groundRay.point;

                    Debug.DrawRay(FwdgroundRay.point, (ParrellSurfaceVector(angleX, angleY, angleZ, FwdgroundRay.normal)), Color.magenta);
                    Debug.DrawRay(CastPos - offset, transform.forward * fwdDst, Color.yellow);

                    isFwD = CheckSlope(groundSlope);
                }
                else
                {
                    Debug.DrawRay(CastPos - offset, transform.forward * fwdDst, Color.magenta);
                    isFwD = false;
                }
            }

            groundPos = transform.position;
        }
    }

}
