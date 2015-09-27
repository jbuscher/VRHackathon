using UnityEngine;
using System.Collections;

public class EventKickoffLevel2 : MonoBehaviour {

	public bool Clicked;

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer, true);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer, false);
			}
		}
	}

	[RPC]
	void EventHappened(bool serverSent, bool value) {
		Clicked = value;

		if (Network.isServer) {
			if (Clicked) {
				GetComponent<NetworkView>().RPC("TriggerDoorOpen", RPCMode.AllBuffered);
			}
		}
	}

	[RPC]
	void TriggerDoorOpen() {
		GameObject.Find("DoorRed").GetComponent<DoorEvent>().Fire();
		GameObject.Find("DoorBlue").GetComponent<DoorEvent>().Fire();
	}
}
