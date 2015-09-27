using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

	private Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
	}

	public void Fire() {
		anim.SetBool("Open", true);
		gameObject.GetComponent<BoxCollider>().enabled = false;
	}

	public void UnFire() {
		anim.SetBool("Open", false);
		gameObject.GetComponent<BoxCollider>().enabled = true;
	}
}
