using UnityEngine;
using System.Collections;

public class EventKickoffLevel2 : MonoBehaviour {

	public bool Clicked;
	public Animator anim;

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer, true);
			}
		}
	}

	[RPC]
	void EventHappened(bool serverSent, bool value) {
		Clicked = value;

		if (Clicked) {
			GetComponent<NetworkView>().RPC("TriggerDoorOpen", RPCMode.AllBuffered);
			anim.SetBool("Down", true)
		}
	}

	[RPC]
	void TriggerDoorOpen() {
		GameObject.Find("GlassToBreak").GetComponent<GlassEvent>().Fire();
		GameObject.Find("EndDoor").GetComponent<DoorEvent>().Fire();
	}
}
