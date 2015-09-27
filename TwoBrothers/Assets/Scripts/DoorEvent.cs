using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

	public void Fire() {
		Collider.enabled = false;
	}
}
