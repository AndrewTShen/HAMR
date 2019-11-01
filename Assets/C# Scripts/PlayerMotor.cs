﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	private Vector3 jumpForce = Vector3.zero;
	private Player player;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	//Movement
	public void Move(Vector3 _velocity)
	{
//		if (player._isDead == true) {
//			_velocity = Vector3.zero;
//		}
		velocity = _velocity;
	}

	//Rotate on the y axis
	public void Rotate (Vector3 _rotation)
	{
		rotation = _rotation;
	}	

	//Rotate on the x axis
	public void RotateCamera (float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

	//Get a force vector for jump
	public void ApplyJump (Vector3 _jumpForce)
	{
		jumpForce = _jumpForce;
	}

	//Run every physics iteration
	void FixedUpdate ()
	{
		PerformMovement(); 
		PerformRotation();
	}

	void PerformMovement ()
	{
		if (velocity != Vector3.zero) {
			rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
		}

		if (jumpForce != Vector3.zero)
		{
			rb.AddForce (jumpForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
	}
	void PerformRotation ()
	{
		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation));

		if (cam != null)
		{
			//Set rotation and clamp it
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp (currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			//Apply our rotation to the transform of the camera
			cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0f, 0f);

		}
	}
}