using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

	public void Fire() {
		this.transform.Translate(Vector3.Slerp(this.transform.position, this.transform.position + Vector3.down * 10, 0.5f));
	}
}
