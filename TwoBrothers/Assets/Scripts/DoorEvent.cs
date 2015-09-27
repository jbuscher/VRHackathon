using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

	private Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
	}

	public void Fire() {
		anim.SetBool("Open", true);
		Destroy(gameObject.GetComponent<BoxCollider>());
	}
}
