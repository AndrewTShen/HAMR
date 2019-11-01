using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

	public int selectedWeapon = 0;
	private float nextTimeButton = 0.1f;
	public float clickRate = 5f;

	public Animator animator;
	public Gun sniper;
	public Gun assault;

	void Start () {
		SelectWeapon ();
	}

	void Update () {

		int previousSelectedWeapon = selectedWeapon;

		if (Input.GetKey (KeyCode.E) && Time.time >= nextTimeButton) {

			#region Unscoping
			sniper.scopeOverlay.SetActive (false); 			sniper.gunCamera.SetActive (true); 			sniper.crosshairs.SetActive (true); 			Debug.Log ("Gun camera true."); 			sniper.mainCamera.fieldOfView = 60f;
			assault.scopeOverlay.SetActive (false); 			assault.gunCamera.SetActive (true); 			assault.crosshairs.SetActive (true); 			Debug.Log ("Gun camera true."); 			assault.mainCamera.fieldOfView = 60f;
			#endregion

			animator.SetBool ("SniperScoping", false);

			nextTimeButton = Time.time + 1f / clickRate;
			if (selectedWeapon >= transform.childCount - 1) {
				selectedWeapon = 0;
			}  else {
				selectedWeapon++;
			}
			Debug.Log ("Weapon Switched ++");

		}

		if (Input.GetKey (KeyCode.Q) && Time.time >= nextTimeButton) {

			#region Unscoping 			sniper.scopeOverlay.SetActive (false); 			sniper.gunCamera.SetActive (true); 			sniper.crosshairs.SetActive (true); 			Debug.Log ("Gun camera true."); 			sniper.mainCamera.fieldOfView = 60f; 			assault.scopeOverlay.SetActive (false); 			assault.gunCamera.SetActive (true); 			assault.crosshairs.SetActive (true); 			Debug.Log ("Gun camera true."); 			assault.mainCamera.fieldOfView = 60f; 			#endregion

			animator.SetBool ("SniperScoping", false);

			nextTimeButton = Time.time + 1f / clickRate;
			if (selectedWeapon <= 0) {
				selectedWeapon = transform.childCount - 1;
			}  else {
				selectedWeapon--;
			}
			Debug.Log ("Weapon Switched --");
		}

		if (previousSelectedWeapon != selectedWeapon) {
			SelectWeapon ();
		}
	}

	void SelectWeapon () {
		int i = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedWeapon)
				weapon.gameObject.SetActive (true);
			else
				weapon.gameObject.SetActive (false);

			i++;		
		}
	}
}
