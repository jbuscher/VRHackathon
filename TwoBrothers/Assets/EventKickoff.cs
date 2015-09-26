using UnityEngine;
using System.Collections;

public class EventKickoff : MonoBehaviour {

	bool serverButton = false;
	bool clientButton = false;

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>().isMine) {
			GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer);
		}
	}

	void EventHappened(bool serverSent) {
		if (!serverSent) {
			clientButton = true;
		} else {
			serverButton = true;
		}

		if (serverButton && clientButton) {
			TriggerDoorOpen();
		}
	}

	void TriggerDoorOpen() {
		GameObject.Find("DoorRed").GetComponent<DoorEvent>().Fire();
		GameObject.Find("DoorBlue").GetComponent<DoorEvent>().Fire();
	}
}
