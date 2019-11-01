using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {
	
	[SerializeField] private string weaponLayerName = "Weapon";

	[SerializeField] private Transform weaponHolder;

	[SerializeField] public PlayerWeapon primaryWeapon;

	[SerializeField] public PlayerWeapon secondaryWeapon;


	//GunSwitch
	[SerializeField] private int numGuns = 2;
	private int gunSwitch = 0;

	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;

	void Start() {
		EquipWeapon (primaryWeapon);
	}
		
	void Update (){
		#region Gun Switch

		if (Input.GetKey (KeyCode.E)) {
			if (gunSwitch < numGuns - 1) {
				gunSwitch = gunSwitch + 1;
			} else {
				gunSwitch = 0;
			}
		}
		if (gunSwitch == 0) {
			EquipWeapon (primaryWeapon);
		} else if (gunSwitch == 1) {
			EquipWeapon (secondaryWeapon);
		}
		Debug.Log (gunSwitch.ToString());

		#endregion
	}

	public PlayerWeapon GetCurrentWeapon () {
		return currentWeapon;
	}

	public WeaponGraphics GetCurrentGraphics () {
		return currentGraphics;
	}

	public void EquipWeapon (PlayerWeapon _weapon) {
		currentWeapon = _weapon;

		GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
		_weaponIns.transform.SetParent (weaponHolder);

		currentGraphics = _weaponIns.GetComponent<WeaponGraphics> ();
		if (currentGraphics == null) {
			Debug.Log ("No WeaponGraphics component on the weapon object: " + _weaponIns.name);
		}

		if (isLocalPlayer) {
			Util.SetLayerRecursively (_weaponIns, LayerMask.NameToLayer (weaponLayerName));
		}
	}

}
