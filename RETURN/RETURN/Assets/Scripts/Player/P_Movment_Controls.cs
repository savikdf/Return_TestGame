using UnityEngine;
using System.Collections;


public class P_Movment_Controls : MonoBehaviour
{
    public static P_Movment_Controls instance;
    public Rigidbody rb;
    float b;
    public int iterations = 6;
    //mov
    [Range(0, 10)]
    public float grabLengthDistance = 7f; //def 7

    [Range(0.0f, 25.0f)]
    public float movmentSpeed = 10f; //def 7

    [Range(1, 100)]
    public float jumpSpeed = 100f;      //def 21

    [Range(0, 1)]
    public float jumpForwardDamp = .5f;      //def 21

    [Range(0.00f, 1.00f)]
    public float PullbackDamp = 0.1f;    

    float VertVel, FwdVel, SideVel;
    [Range(0.00f, 1.00f)]
    public float gravAcc = 0.5f;

    Vector3 vertForce, sideForce, forwardForce;

    Vector3 verticleDir, forwardDir, sideDir, ExternalForce, outputVector;

    bool canDoubleJump = false;
    [Range(0f, 1f)]
    public float friction = 0.9998f;

    //rot
    [Range(1f, 30f)]
    public float mouseSensitivity = 5.0f; //def 5
    float verticalRotation = 0;
    public float upDownRange = 60.0f;

    public bool jump;

    public enum CharacterStates
    {
        Movement,
        Climb,
        Grab,
        Falling,
        Idle,
    };

    public static CharacterStates playerState;

    void Awake()
    {
        verticleDir = transform.up;
        forwardDir = transform.forward;
        sideDir = transform.right;

        rb = GetComponent<Rigidbody>();
        playerState = CharacterStates.Idle;
        if (!instance)
            instance = this;
    }

    void Start()
    {
        // c===============3
        // c===============3
        // c===============3
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        // c===============3
        // c===============3
        // c===============3
    }

    //-------------------------
    public void FixedUpdate()
    {
        UpdateVelocity();
        Look();   
        Move();       
    }


    void Update()
    {
        UserInput();
        Jump();
    }

    void UserInput()
    {
        FwdVel = (Input.GetAxis("Vertical") * movmentSpeed);
        SideVel = (Input.GetAxis("Horizontal") * movmentSpeed);

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        else if(Input.GetButtonUp("Jump"))
        {
            jump = false;
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            playerState = CharacterStates.Grab;
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            playerState = CharacterStates.Idle;
        }

    }

    void UpdateVelocity()
    {
        verticleDir = transform.up;
        forwardDir = transform.forward;
        sideDir = transform.right;

        outputVector = forwardForce + sideForce + vertForce + ExternalForce;
        rb.velocity = outputVector;
    }


    void ApplyExternalForce(Rigidbody rig)
    {
        ExternalForce = rig.velocity;
    }

    void Look()
    {
        float sideToSideRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, sideToSideRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Jump()
    {
        if (playerState != CharacterStates.Grab)
        {
            if (P_Ground_Check.isGrounded && jump)
            {
                vertForce = jumpSpeed * transform.up;
            }
            else if (!P_Ground_Check.isGrounded && playerState != CharacterStates.Grab)
            {
                vertForce -= gravAcc * transform.up;
            }
            else
            {
                vertForce = Vector3.zero;
            }
        }
        else if (playerState == CharacterStates.Grab)
        {

            if (P_Arm_Distance.c <= P_Arm_Distance.l + grabLengthDistance)
            {
                vertForce -= gravAcc * transform.up;
            }
            else
            {
                transform.position += new Vector3(0, UpdatePosition().y, 0);

                if (!P_Ground_Check.isGrounded)
                {
                    vertForce = -vertForce * PullbackDamp;
                }
                else
                {
                    vertForce = Vector3.zero;
                }
            }
        }

    }

    Vector3 UpdatePosition()
    {
        float dx = P_Arm_Distance.HCentre.x - P_Arm_Distance.UpperCentre.x;
        float dy = P_Arm_Distance.HCentre.y - P_Arm_Distance.UpperCentre.y;
        float dz = P_Arm_Distance.HCentre.z - P_Arm_Distance.UpperCentre.z;

        float dist = Utilities_Class.distance3D(P_Arm_Distance.UpperCentre, P_Arm_Distance.HCentre);
        float diff = (P_Arm_Distance.c + grabLengthDistance) - dist;

        float percent = (diff / dist) / 2;

        float offsetX = dx * percent;
        float offsetY = dy * percent;
        float offsetZ = dz * percent;

        return (new Vector3(offsetX, offsetY, offsetZ));
    }

    void Move()
    {
        if (P_Ground_Check.isGrounded && playerState != CharacterStates.Grab)
        {
            //float slopeMultiplier = (Utilities_Class.normal(((P_Ground_Check.groundSlope != 0 || P_Ground_Check.groundSlope != 180) ? P_Ground_Check.groundSlope : 1), 0, 60));
           // slopeMultiplier = (slopeMultiplier != 0) ? slopeMultiplier : 1; 
            //Debug.Log(slopeMultiplier);
            forwardForce = ((forwardDir) * FwdVel);
            sideForce = ((sideDir) * SideVel);
            transform.position = P_Ground_Check.groundPos;

        }
        else
        {
            if (playerState == CharacterStates.Grab)
            {
                if (P_Arm_Distance.c <= P_Arm_Distance.l + grabLengthDistance)
                {
                    forwardForce = (forwardDir) * FwdVel;
                    sideForce = (sideDir) * SideVel;
                }
                else
                {
                    transform.position += new Vector3 (UpdatePosition().x, 0, UpdatePosition().z) * 2;
                }

            }
            else
            {

                forwardForce = ((forwardDir) * FwdVel) * jumpForwardDamp;
                sideForce = ((sideDir) * SideVel) * jumpForwardDamp;
            }

        }
    }
 
}

