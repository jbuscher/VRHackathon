using UnityEngine;
using System.Collections;

public class AlertNetworkDecision : MonoBehaviour {

	public bool isServer;
	public bool isLooping= false;
	public AudioSource aud;

	void OnTriggerEnter(Collider other) {
		if (!isLooping) {
			if (isServer) {
				if (!Network.isServer) {
					GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartServerPlease();
					isLooping = true;
					aud.Play();
					StartCoroutine("ServerLoop");
				}
			} else {
				isLooping = true;
				aud.Play();
				StartCoroutine("LoopDaLoop");
			}
			if (isLooping) {
				GameObject.Find("Wall").transform.Translate(Vector3.forward * -8);
			}
		}
	}

	IEnumerator LoopDaLoop() {
		GameObject.Find("NetworkManager").GetComponent<NetworkManager>().AutoJoinServer();
		
		yield return new WaitForSeconds(3);

		if (!Network.isClient) {
			StartCoroutine("LoopDaLoop");;
		}
	}

	IEnumerator ServerLoop() {
		while (true) {
			if (Network.connections.Length > 0) {
				GameObject.Find("NetworkManager").GetComponent<NetworkView>().RPC("LoadLevel", RPCMode.AllBuffered, "Main", 1);
				break;
			}

			yield return new WaitForSeconds(3);
		}
	}

	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
