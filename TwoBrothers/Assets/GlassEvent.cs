using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {
	
	public void Fire() {
		Destroy(gameObject);
	}
}