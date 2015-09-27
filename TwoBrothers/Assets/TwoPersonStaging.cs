using UnityEngine;
using System.Collections;

public class TwoPersonStaging : MonoBehaviour {

	public bool Clicked;
	
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				if (gameObject.tag.Equals("RedStaging") && Network.isServer) {
					GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer, true);
				} else if (gameObject.tag.Equals("BlueStaging") && Network.isClient) {
					GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer, true);
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				if (gameObject.tag.Equals("RedStaging") && Network.isServer) {
					GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer, false);
				} else if (gameObject.tag.Equals("BlueStaging") && Network.isClient) {
					GetComponent<NetworkView>().RPC("EventHappened", RPCMode.AllBuffered, Network.isServer, false);
				}
			}
		}
	}
	
	[RPC]
	void EventHappened(bool serverSent, bool value) {
		Clicked = value;
		
		if (Network.isServer) {
			var red = GameObject.FindGameObjectWithTag("RedStaging");
			var blue = GameObject.FindGameObjectWithTag("BlueStaging");
			if (red.GetComponent<TwoPersonStaging>().Clicked && blue.GetComponent<TwoPersonStaging>().Clicked) {
				GetComponent<NetworkView>().RPC("TriggerDoorOpen", RPCMode.AllBuffered);
			}
		}
	}
	
	[RPC]
	void TriggerDoorOpen() {
		GameObject.Find("DoorIntro").GetComponent<DoorEvent>().UnFire();
		GameObject.Find("DoorIntro2").GetComponent<DoorEvent>().UnFire();
		GameObject.Find("DoorIntro3").GetComponent<DoorEvent>().Fire();
		GameObject.Find("DoorIntro4").GetComponent<DoorEvent>().Fire();
	}
}
