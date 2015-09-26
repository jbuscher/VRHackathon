using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class ServerCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NetworkView nView = GetComponent<NetworkView>();
		if (!nView.isMine) {
			Destroy(GetComponentInChildren<Camera>());
			Destroy(GetComponent<FirstPersonController>());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
