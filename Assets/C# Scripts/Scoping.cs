using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoping : MonoBehaviour {

	public Animator animator;
	private bool isScoped = false;
	public GameObject scopeOverlay;

	void Update () {
		if (Input.GetButtonDown ("Fire2")) {
			//			nextTimeToScope = Time.time + 1f / scopeTime;
			isScoped = !isScoped;
			animator.SetBool ("SniperScoping", isScoped);

			scopeOverlay.SetActive (isScoped);
		}
	}
}
