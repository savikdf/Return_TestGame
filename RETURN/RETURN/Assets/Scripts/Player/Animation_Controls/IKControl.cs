using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{

    [Range(0, 5)] public float ArmRayDistance = 2f;
    [Range(-10, 10)] public float ArmRayLength = 1f;
    [Range(-10, 10)]public float ArmRayAngle = 1f;

    [Range(0, 5)]
    public float LegRayDistance = 2f;
    [Range(-10, 10)]
    public float LegRayLength = 1f;
    [Range(-10, 10)]
    public float LegRayAngle = 1f;

    private Animator anim;

    public static Vector3 leftHandPos, rightHandPos;
    public static Vector3 leftFootPos, rightFootPos;

    public bool useIK;

    public static bool leftHandIK, leftFootIK;
    public static bool rightHandIK, rightFootIK;

    Vector3 lefthandOriginalPos, righthandOriginalPos;
    Vector3 leftFootOriginalPos, rightFootOriginalPos;

    public static Transform hitObject;

    public Transform LeftArmLock, RightArmLock;
    Vector3 chestPos;

    Quaternion leftHandRot, rightHandRot;
    Quaternion leftFootRot, rightFootRot;

    public LayerMask ignoreMask;

    Vector3 ArmDirectionLeft, ArmDirectionRight;
    Vector3 LegDirectionLeft, LegDirectionRight;

    Vector3 RayCastArmVector, RayCastLegVector;

    void Awake()
    {
        anim = GetComponent<Animator>();
        lefthandOriginalPos = LeftArmLock.localPosition;
        righthandOriginalPos = RightArmLock.localPosition;
    }

    Vector3 CalculateArmDrawAngles()
    {
        float angle = Mathf.Deg2Rad * transform.root.rotation.eulerAngles.y;

        float x = transform.position.x;
        float z = transform.position.z;
 
        float x1 = x + Mathf.Cos(angle + Mathf.PI / 2) * -ArmRayDistance;
        float z1 = z - Mathf.Sin((angle + Mathf.PI / 2)) * -ArmRayDistance;

        return (new Vector3(x1, transform.root.position.y + 7, z1));
    }

    Vector3 CalculateLegDrawAngles()
    {
        float angle = Mathf.Deg2Rad * transform.root.rotation.eulerAngles.y;

        float x = (rightFootOriginalPos.x + leftFootOriginalPos.x)/2;
        float z = (rightFootOriginalPos.z + leftFootOriginalPos.z)/2;

        float x1 = x + Mathf.Cos(angle + Mathf.PI / 2) * -LegRayDistance;
        float z1 = z - Mathf.Sin((angle + Mathf.PI / 2)) * -LegRayDistance;

        return (new Vector3(x1, (rightFootOriginalPos.y + leftFootOriginalPos.y)/2, z1));
    }

    void FixedUpdate()
    {
        DebugRays();


        RayCastArmVector = CalculateArmDrawAngles();
        RayCastLegVector = CalculateLegDrawAngles();

        if (P_Movment_Controls.playerState != P_Movment_Controls.CharacterStates.Grab)
        {
            ArmDirectionLeft = (-transform.up * ArmRayLength) + (-transform.right * ArmRayAngle);
            ArmDirectionRight = (-transform.up * ArmRayLength) + (transform.right * ArmRayAngle);

            LegDirectionLeft = (-transform.up * LegRayLength) + (-transform.right * LegRayAngle);
            LegDirectionRight = (-transform.up * LegRayLength) + (transform.right * LegRayAngle);
        }
        RightHand();
        LeftHand();
        LeftFoot();
        RightFoot();
        Chest();
    }

    void Chest()
    {
        RaycastHit chestHit;
        //ChestRay
        if (Physics.Raycast(transform.position + new Vector3(0, 4, 0), transform.forward, out chestHit, 4f, ignoreMask))
        {
            chestPos = chestHit.point;
        }
    }

    void LeftHand()
    {
        RaycastHit LHit;
        //Casting and not Grabbing

        if (Physics.Raycast(RayCastArmVector, ArmDirectionLeft, out LHit, ArmRayLength, ignoreMask))
        {
            leftHandIK = true;
            leftHandPos = LHit.point;
             // leftHandPos.x = lefthandOriginalPos.x;
            leftHandRot = Quaternion.FromToRotation(Vector3.right, -transform.forward + transform.right + LHit.normal);
            hitObject = (hitObject == LHit.transform) ? hitObject : LHit.transform;
            LeftArmLock.position = leftHandPos;
            LeftArmLock.parent = LHit.transform;
        }
        else
        {
            if (P_Movment_Controls.playerState == P_Movment_Controls.CharacterStates.Grab)
            {
                leftHandPos = LeftArmLock.position;
                //leftHandPos = LHit.point;
                //Hands Need to be Spaced out;
            }
            else
            {
             
                LeftArmLock.parent = transform;
                LeftArmLock.localPosition = lefthandOriginalPos;
                leftHandIK = false;
            }
        }

    
    }

    void RightHand()
    {
        RaycastHit RHit;
 
        //No Grab Cast
        if (Physics.Raycast(RayCastArmVector, ArmDirectionRight, out RHit, ArmRayLength, ignoreMask))
        {
            rightHandIK = true;   
            rightHandPos = RHit.point;
            // rightHandPos.x = righthandOriginalPos.x;
            rightHandRot = Quaternion.FromToRotation(-Vector3.right, -transform.forward + -transform.right + RHit.normal);
            hitObject = (hitObject == RHit.transform) ? hitObject : RHit.transform;
            RightArmLock.position = rightHandPos;
            RightArmLock.parent = RHit.transform;
        }
        else
        {
            if (P_Movment_Controls.playerState == P_Movment_Controls.CharacterStates.Grab)
            {
                rightHandPos = RightArmLock.position;
                //Lock to hitpoint
                //rightHandPos = RHit.point;
                //Hands Need to be Spaced out;
            }
            else
            {
                RightArmLock.parent = transform;
                RightArmLock.localPosition = righthandOriginalPos;
                rightHandIK = false;
            }
           
        }

    }

    void LeftFoot()
    {
        RaycastHit LHit;
        //Casting and not Grabbing

        if (Physics.Raycast(RayCastLegVector, LegDirectionLeft, out LHit, LegRayLength, ignoreMask))
        {
            leftFootIK = true;
            leftFootPos = new Vector3(leftFootOriginalPos.x, leftFootOriginalPos.y + (LHit.point.y - leftFootOriginalPos.y), leftFootOriginalPos.z);
            // leftHandPos.x = lefthandOriginalPos.x;
            // leftFootRot = Quaternion.FromToRotation(transform.forward, LHit.normal);
            // hitObject = (hitObject == LHit.transform) ? hitObject : LHit.transform;
            // LeftLock.position = leftHandPos;
            // LeftLock.parent = LHit.transform;
        }
        else
        {
            if (P_Movment_Controls.playerState == P_Movment_Controls.CharacterStates.Grab)
            {
                //leftHandPos = LeftLock.position;
                //leftHandPos = LHit.point;
                //Hands Need to be Spaced out;
            }
            else
            {

                //LeftLock.parent = transform;
                //LeftLock.localPosition = lefthandOriginalPos;
                leftFootIK = false;
            }
        }


    }

    void RightFoot()
    {
        RaycastHit RHit;

        //No Grab Cast
        if (Physics.Raycast(RayCastLegVector, LegDirectionRight, out RHit, LegRayLength, ignoreMask))
        {
            rightFootIK = true;
            rightFootPos = new Vector3(rightFootOriginalPos.x, rightFootOriginalPos.y + (RHit.point.y - rightFootOriginalPos.y), rightFootOriginalPos.z);

            // rightHandPos.x = righthandOriginalPos.x;
            //rightFootRot = Quaternion.FromToRotation(transform.forward, RHit.normal);
           // hitObject = (hitObject == RHit.transform) ? hitObject : RHit.transform;
           // RightLock.position = rightHandPos;
           // RightLock.parent = RHit.transform;
        }
        else
        {
            if (P_Movment_Controls.playerState == P_Movment_Controls.CharacterStates.Grab)
            {
               // rightHandPos = RightLock.position;
                //Lock to hitpoint
                //rightHandPos = RHit.point;
                //Hands Need to be Spaced out;
            }
            else
            {
                //RightLock.parent = transform;
                //RightLock.localPosition = righthandOriginalPos;
                rightFootIK = false;
            }

        }

    }

    void DebugRays()
    {
        //CentreRay
        Debug.DrawRay(transform.position + new Vector3(0, 4, 0), transform.forward);
        //Right Ray
        Debug.DrawRay(RayCastArmVector, ArmDirectionRight, Color.green);
        //Left Ray 
        Debug.DrawRay(RayCastArmVector, ArmDirectionLeft, Color.green);

        //Right Ray
        Debug.DrawRay(RayCastLegVector, LegDirectionRight, Color.yellow);
        //Left Ray 
        Debug.DrawRay(RayCastLegVector, LegDirectionLeft, Color.yellow);
    }

    void OnAnimatorIK()
    {
        if(useIK)
        {
            // lefthandOriginalPos = anim.GetIKPosition(AvatarIKGoal.LeftHand);
            // righthandOriginalPos = anim.GetIKPosition(AvatarIKGoal.RightHand);

             leftFootOriginalPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
             rightFootOriginalPos = anim.GetIKPosition(AvatarIKGoal.RightFoot);

            if (leftHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);

                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            }

            if (rightHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);

                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            }

            if (leftFootIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);

                //anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
               // anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            }

            if (rightFootIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);

                //anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
                //anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            }
        }
    }

}
