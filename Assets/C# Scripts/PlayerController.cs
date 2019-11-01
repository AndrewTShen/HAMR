using UnityEngine;
using System.Collections; //partofJumpCode

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Rigidbody))] //jump

public class PlayerController : MonoBehaviour {

	[SerializeField] private float speed = 8f;
	[SerializeField] private float sprint = 2f;
	[SerializeField] private float lookSensitivity = 3f;
	//	[SerializeField] private float jumpForce = 1000f; //thruster
	[SerializeField] private float jumpSpeed = 100.0f; // jump
	[SerializeField] private float maxSpeed = 10f;
	private bool onGround = false; //jump
	Rigidbody rb; //jump

	//	[Header("Spring settings")] //thruster
	//	[SerializeField] private float jointSpring = 20f; //thruster
	//	[SerializeField] private float jointMaxForce = 40f; //thruster

	//CursorLock
	private float nextTimeButton = 1f;
	public float clickRate = 1f;

	private PlayerMotor motor;
	//	private ConfigurableJoint joint; //Thrusters

	void Start ()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		motor = GetComponent<PlayerMotor> ();
		//		joint = GetComponent<ConfigurableJoint> (); //thrusters
		rb = GetComponent<Rigidbody> ();

		//		SetJointSettings (jointSpring); //thruster

	}

	void Update ()
	{
		#region Cursor Lock
		//Figure out lock state
		if (Input.GetKey (KeyCode.Alpha0) && Time.time >= nextTimeButton) {
			nextTimeButton = Time.time + 1f / clickRate;
			if (Cursor.visible == false) {
				Debug.Log ("Cursor is now visible.");
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}  else {
				Debug.Log ("Cursor is now invisible.");
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}		
		}
		#endregion

		#region Calc Move (Vertical & Horizontal)
		//Calculate moment velocity as a 3D vector
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _yMov = Input.GetAxisRaw ("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _moveVertical = transform.forward * _yMov;
		#endregion

		#region Sprint
		speed = 8f;
		if (Input.GetKey(KeyCode.LeftShift)) { 
			speed = speed * sprint; 
		}  else if (Input.GetKey(KeyCode.LeftControl)) {
			speed = speed / (sprint * 2); 
		}
		#endregion

		#region Jump
		if (Input.GetKeyDown("space") && (onGround)) {
			rb.AddForce(Vector3.up * jumpSpeed);
		}
		onGround = false;
		#endregion

		#region Move (Vertical & Horizontal)

		Vector3 _velocity = (_movHorizontal + _moveVertical).normalized * speed;


		//Apply movement


		motor.Move(_velocity);
		#endregion

		#region Look Around
		//Calculate rotation as a 3D vector: This applies for turning around
		float _yRot = Input.GetAxisRaw ("Mouse X");

		Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * lookSensitivity;

		motor.Rotate(_rotation); 

		//Calculate camera rotation as a 3D vector: This applies for looking up and down

		float _xRot = Input.GetAxisRaw ("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply rotation
		motor.RotateCamera(_cameraRotationX); 
		#endregion

		#region Thuster
		//Vector3 _jumpForce = Vector3.zero; //Thruster
		//Apply thruster force
		//		if (Input.GetButton ("Jump")) //Thruster
		//		{
		//			_jumpForce = Vector3.up * jumpForce; //Thruster

		//Re-apply this for thruster-like effects
		//			SetJointSettings (0f);
		//		}
		//			else 
		//		{
		//			SetJointSettings (jointSpring);
		//		}
		//		motor.ApplyJump (_jumpForce); //Thruster
		#endregion



	}

	void FixedUpdate() {
		//		Debug.Log (rb.velocity.ToString ());
		if(rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}		
	}


	void OnCollisionStay () {
		onGround = true;
		//		Debug.Log ("Touching Ground");
	}

	//	private void SetJointSettings (float _jointSpring) //Thruster
	//	{
	//		joint.yDrive = new JointDrive //Thruster
	//		{  positionSpring = _jointSpring, //Thruster
	//			maximumForce = jointMaxForce//Thruster
	//		} ;//Thruster
	//	} //Thruster
}
