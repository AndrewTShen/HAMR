using UnityEngine;
using UnityEngine.Networking;


[RequireComponent (typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";



	[SerializeField] private Camera cam;

	[SerializeField] private LayerMask mask;

	private PlayerWeapon currentWeapon;
	private WeaponManager weaponManager;

	// Use this for initialization
	void Start () {
		if (cam == null) {
			Debug.LogError ("PlayerShoot: No camera referenced.");
			this.enabled = false;
		}

		weaponManager = GetComponent<WeaponManager> ();
	} 	

	void Update () {
		currentWeapon = weaponManager.GetCurrentWeapon ();


		if (currentWeapon.fireRate <= 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				InvokeRepeating ("Shoot", 0f, 1f/currentWeapon.fireRate);
			} else if (Input.GetButtonUp ("Fire1")) {
				CancelInvoke ("Shoot");
			}	
		}
	}

	[Client]
	void Shoot () {
		if (!isLocalPlayer) {
			return;
		}

		CmdOnShoot ();

		Debug.Log ("Shoot.");
		RaycastHit _hit;
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask)) {
			if (_hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot (_hit.collider.name, currentWeapon.damage);
			}

			//We hit something, call the OnHit method on the server
			CmdOnHit (_hit.point, _hit.normal);
		}
	}

	//Called on server when player shoots
	[Command]
	void CmdOnShoot () {
		RpcDoShootEffect ();
	}

	//Called on all clients when needed a shoot effect
	[ClientRpc]
	void RpcDoShootEffect () {
		weaponManager.GetCurrentGraphics().muzzleFlash.Play ();
	}

	//Is called on the server when we hit somehting, takes in the hit point and the normal of the surface.
	[Command]
	void CmdOnHit (Vector3 _pos, Vector3 _normal) {
		RpcDoHitEffect (_pos, _normal);
	}

	//Is called on all clients, we can spawn in effects.
	[ClientRpc]
	void RpcDoHitEffect (Vector3 _pos, Vector3 _normal) {
		Debug.Log ("RPC HIT EFFECT");
		GameObject _hitEffect = Instantiate (weaponManager.GetCurrentGraphics ().hitEffectPrefab, _pos, Quaternion.LookRotation (_normal));
		Destroy (_hitEffect, 1f);
	}

	[Command]
	void CmdPlayerShot (string _playerID, int _damage) {
		Debug.Log (_playerID + " has been shot.");
		Player _player = Game_Manager.GetPlayer (_playerID);
		_player.TakeDamage (_damage);
	}
}


		