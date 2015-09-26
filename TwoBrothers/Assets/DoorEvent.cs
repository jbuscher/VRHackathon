using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

	public void Fire() {
		this.transform.Translate(Vector3.down * 10);
	}
}
