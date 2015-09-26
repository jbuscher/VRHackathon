using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class ServerCheck : MonoBehaviour {
	
	// Grab the network view and edit this player controller accordingly
	// Delete the camera and user inputs on client
	void Start () {
		NetworkView nView = GetComponent<NetworkView>();
		if (!nView.isMine) {
			Destroy(GetComponentInChildren<Camera>());
			Destroy(GetComponent<FirstPersonController>());
		} else if (Network.isClient) {
			// Hide blue
			Camera camera = GetComponentInChildren<Camera>();
			camera.cullingMask &=  ~(1 << LayerMask.NameToLayer("RedLayer"));
			camera.cullingMask |= 1 << LayerMask.NameToLayer("BlueLayer");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}