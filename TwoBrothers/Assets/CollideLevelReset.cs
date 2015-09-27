using UnityEngine;
using System.Collections;

public class CollideLevelReset : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				GameObject.Find("NetworkManager").GetComponent<NetworkView>().RPC("LoadLevel", RPCMode.AllBuffered, Application.loadedLevelName, Application.loadedLevel);
			}
		}
	}
}
