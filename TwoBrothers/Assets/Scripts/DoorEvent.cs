using UnityEngine;
using System.Collections;

public class DoorEvent : MonoBehaviour {

	public Animator anim;
	private AudioSource aud;

	void Start() {
		aud = GetComponent<AudioSource> ();
	}

	public void Fire() {
		if(aud != null)
			aud.Play();
		gameObject.GetComponent<BoxCollider>().enabled = false;
		anim.SetBool("Open", true);
	}

	public void UnFire() {
		gameObject.GetComponent<BoxCollider>().enabled = true;
		anim.SetBool("Open", false);
	}
}
