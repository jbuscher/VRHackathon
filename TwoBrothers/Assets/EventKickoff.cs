using UnityEngine;
using System.Collections;

public class EventKickoff : MonoBehaviour {

	bool serverButton = false;
	bool clientButton = false;

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer);
			}
		}
	}

	[RPC]
	void EventHappened(bool serverSent) {
		if (!serverSent) {
			clientButton = true;
			print("client!!!");
		} else {
			serverButton = true;
			print("SERVERRRR!!!");
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
