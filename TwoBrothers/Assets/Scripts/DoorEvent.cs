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
		if(anim != null) 
			anim.SetBool("Open", true);
		if(aud != null)
			aud.Play();
		gameObject.GetComponent<BoxCollider>().enabled = false;
	}

	public void UnFire() {
		anim.SetBool("Open", false);
		gameObject.GetComponent<BoxCollider>().enabled = true;
	}
}
