using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class P_Controls : MonoBehaviour {
	public static P_Controls instance;

	//mov
	[Range(1,5)]public float movementSpeed = 3.5f; //def 4
	[Range(0,10)]public float jumpSpeed = 3.5f;		//def 4
	float verticalVelocity = 0;		
	bool canDoubleJump = false;

    [HideInInspector]
    public Vector3 speed;

	//rot
	[Range(1f,30f)]public float mouseSensitivity = 5.0f; //def 5
	float verticalRotation = 0;
	public float upDownRange = 60.0f;	

	//returns true if they can sprint
	bool SprintCheck{
		get{ return (characterController.isGrounded && Input.GetButton ("Sprint"));}
	}


    //comp
    [HideInInspector]
    public CharacterController characterController;

	//========================================================

	void Awake(){
		if (!instance)
			instance = this;
		characterController = GetComponent<CharacterController>();
	}

	void Start () {	
		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
	}

	//-------------------------

	public IEnumerator CheckPlayerInputs(){
		while (Player.instance.isAlive) {
			Look ();
			Move ();
			yield return null;
		}
	}

	void Look(){
		float sideToSideRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, sideToSideRotation, 0);

		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);	
	}

	void Move(){
		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

		//upwards velocity 
		if (!characterController.isGrounded) {
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
		} else if (characterController.isGrounded){	
			verticalVelocity = 0f;
			canDoubleJump = true;
		}
		//jump
		if( characterController.isGrounded && Input.GetButtonDown("Jump")) {
			verticalVelocity = jumpSpeed;
		}
		if (!characterController.isGrounded && canDoubleJump && Input.GetButtonDown("Jump")) {
			verticalVelocity = 0;
			canDoubleJump = false;
			verticalVelocity = (jumpSpeed / 10f) * 10f;
		}

		//sprint
		if (SprintCheck) {
			Debug.Log (SprintCheck);
			forwardSpeed = forwardSpeed * 2f;
		}

		speed = new Vector3( sideSpeed, verticalVelocity, forwardSpeed );
		speed = transform.rotation * speed;

		characterController.Move( speed * Time.deltaTime );

	}

}
