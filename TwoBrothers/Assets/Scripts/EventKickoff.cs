using UnityEngine;
using System.Collections;

public class EventKickoff : MonoBehaviour {

	public bool Clicked;
	private Animator anim;

	void Start() {
		anim = GetComponent<Animator> ();
	}

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
		if (Clicked) {
			anim.SetBool("Down", true);
		} else {
			anim.SetBool("Down", false);
		}

		if (Network.isServer) {
			var buttons = GameObject.FindGameObjectsWithTag("Room1Button");
			if (buttons[0].GetComponent<EventKickoff>().Clicked && buttons[1].GetComponent<EventKickoff>().Clicked) {
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
