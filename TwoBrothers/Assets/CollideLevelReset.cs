using UnityEngine;
using System.Collections;

public class CollideLevelReset : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				GetComponent<NetworkView>().RPC("ResetLevel", RPCMode.AllBuffered);
			}
		}
	}

	[RPC]
	void ResetLevel() {
		Application.LoadLevel(Application.loadedLevel);
	}
}
