using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameObject.Find("NetworkManager").GetComponent<NetworkView>().RPC("LoadLevel", RPCMode.AllBuffered, "Level2", 2);
	}
}
