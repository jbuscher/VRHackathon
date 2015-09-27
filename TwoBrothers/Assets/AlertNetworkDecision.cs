﻿using UnityEngine;
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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
