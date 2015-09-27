using UnityEngine;
using System.Collections;

public class AlertNetworkDecision : MonoBehaviour {

	public bool isServer;

	void OnTriggerEnter(Collider other) {
		if (isServer) {
			if (!Network.isServer) {
				GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartServerPlease();
				StartCoroutine("ServerLoop");
			}
		} else {
			StartCoroutine("LoopDaLoop");
		}
	}

	IEnumerator LoopDaLoop() {
		while (true) {
			GameObject.Find("NetworkManager").GetComponent<NetworkManager>().AutoJoinServer();
			yield return new WaitForSeconds(3);

			if (Network.isClient) {
				break;
			}
		}
	}

	IEnumerator ServerLoop() {
		while (true) {
			if (Network.connections.Length > 0) {
				GameObject.Find("NetworkManager").GetComponent<NetworkManager>().LoadLevel("Main", 1);
				break;
			}

			yield return new WaitForSeconds(3);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
