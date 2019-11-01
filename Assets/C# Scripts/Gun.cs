using System.Collections;
using UnityEngine;



public class Gun : MonoBehaviour {

	private const string PLAYER_TAG = "Player";

	[SerializeField] private Camera cam;
	public ParticleSystem muzzleFlash;
	public GameObject impactEffect;

	public int damage = 10;
	public float range = 100f;
	public float fireRate = 5f;
	//	public float impactForce = 30f;

	//Ammo and reload
	public int maxAmmo = 10; 
	private int currentAmmo;
	public float reloadTime = 2f;
	private bool isReloading = false;

	private float nextTimeToFire = 0f;

	public Animator animator;
//	private float nextTimeToScope = 1f;
//	public float scopeTime = 1f;

	private bool isScoped = false;
	public GameObject scopeOverlay;
	public GameObject gunCamera;
	public GameObject crosshairs;
	public Camera mainCamera;

	public float scopedFOV = 15f;
	private float normalFOV = 60f;

	void Start () {
		currentAmmo = maxAmmo;
	}

	void OnEnable () {
		isReloading = false;
		animator.SetBool ("Reloading", false);
		Debug.Log ("OnEnabled");
	}

	void Update () {

		if (isReloading) {
			return;
		}

//		if (animator.GetBool ("SniperScoping") == false) {
//			onUnscoped ();
//		}

		if (Input.GetButtonDown ("Fire2")) {
			//			nextTimeToScope = Time.time + 1f / scopeTime;
			isScoped = !isScoped;
			animator.SetBool ("SniperScoping", isScoped);

			if (isScoped) {
				StartCoroutine(OnScoped ()); 
			} else {
				onUnscoped ();
			}
		}


		if (currentAmmo <= 0) {
			StartCoroutine(Reload ());
			return;
		}

		if (Input.GetButton ("Fire1") && Time.time >= nextTimeToFire) {
			nextTimeToFire = Time.time + 1f / fireRate;
			Shoot ();
		}
	}
		
	IEnumerator Reload () {
		isReloading = true;
		Debug.Log ("Reloading.");

//		scopeOverlay.SetActive (false);
//		gunCamera.SetActive (true);
//		crosshairs.SetActive (true);
		onUnscoped ();

		animator.SetBool ("SniperScoping", false);
		
		animator.SetBool ("Reloading", true);


		yield return new WaitForSeconds (reloadTime - .25f);

		animator.SetBool ("Reloading", false);

		yield return new WaitForSeconds (0.25f);

		Debug.Log ("Reloaded.");

		currentAmmo = maxAmmo;

		isReloading = false;

	}

	void Shoot () {
		muzzleFlash.Play ();

		currentAmmo--;

		RaycastHit hit;
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, range)) {
			Debug.Log (hit.transform.name);
			if (hit.collider.tag == PLAYER_TAG) {
				PlayerShot (hit.collider.name, damage);
			}
			//Impact Force
			//			if (hit.rigidbody != null) {
			//				hit.rigidbody.AddForce (-hit.normal * impactForce);
			//			}

			//Hit ImpactEffect
			GameObject impactGO = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
			Destroy (impactGO, 0.75f);
		}
	}

	void PlayerShot (string _playerID, int _damage) {
		Debug.Log (_playerID + " has been shot.");
		Player _player = Game_Manager.GetPlayer (_playerID);
		_player.TakeDamage (_damage);
	}

	void onUnscoped () {
		scopeOverlay.SetActive (false);
		gunCamera.SetActive (true);
		crosshairs.SetActive (true);
		Debug.Log ("Gun camera true.");

		mainCamera.fieldOfView = normalFOV;

	}

	IEnumerator OnScoped () {
		yield return new WaitForSeconds (0.15f);
		scopeOverlay.SetActive (true);
		gunCamera.SetActive (false);
		crosshairs.SetActive (false);
		Debug.Log ("Gun camera false.");

		mainCamera.fieldOfView = scopedFOV;
	}



}

