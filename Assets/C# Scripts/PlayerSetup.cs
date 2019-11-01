using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField] Behaviour[] componentsToDisable;

	[SerializeField] string remoteLayerName = "RemotePlayer";

	[SerializeField] string dontDrawLayerName = "DontDraw";

	[SerializeField] GameObject playerGraphics;
	[SerializeField] GameObject gunGraphics;

//	[SerializeField] GameObject playerUIPrefab;
//	private GameObject playerUIInstance;

	Camera pregameCamera;

	void Start () { 
		if (!isLocalPlayer) {
			DisableComponents ();
			AssignRemoteLayer ();
		} else {
			pregameCamera = Camera.main;
			if (pregameCamera != null) {
				pregameCamera.gameObject.SetActive (false);
			}

			// Disable player graphics for local player
			SetLayerRecusively (playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
			SetLayerRecusively (gunGraphics, LayerMask.NameToLayer(dontDrawLayerName));

			// Create Player UI
//			Debug.Log ("Instantiate Player UI");
//			playerUIInstance = Instantiate (playerUIPrefab);
//			playerUIInstance.name = playerUIPrefab.name;

		}

		GetComponent<Player> ().Setup ();

	}

	void SetLayerRecusively (GameObject obj, int newLayer) {
		obj.layer = newLayer;
		foreach (Transform child in obj.transform) {
			SetLayerRecusively (child.gameObject, newLayer);
		}
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		Player _player = GetComponent<Player> ();

		Game_Manager.RegisterPlayer (_netID, _player);
	}

	void AssignRemoteLayer () {
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents () {
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable [i].enabled = false;
		}
	}
		
	//When we die
	void OnDisable () {
//		Destroy (playerUIInstance);

		//Re-enable the scene camera
		if (pregameCamera != null) {
			pregameCamera.gameObject.SetActive(true);
		}

		Game_Manager.UnRegisterPlayer (transform.name);
	}
}
