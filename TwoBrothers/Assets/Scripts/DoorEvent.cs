using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

	private Animator anim;
	private AudioSource aud;

	void Start() {
		anim = GetComponent<Animator>();
		aud = GetComponent<AudioSource> ();
	}

	public void Fire() {
		anim.SetBool("Open", true);
		aud.Play();
		Destroy(gameObject.GetComponent<BoxCollider>());
	}
}
