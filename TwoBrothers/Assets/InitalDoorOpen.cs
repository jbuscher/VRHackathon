using UnityEngine;
using System.Collections;

public class InitalDoorOpen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("DoorDelay");
	}

	IEnumerator DoorDelay() {
		yield return new WaitForSeconds(5);
		GameObject.Find("DoorIntro").GetComponent<DoorEvent>().Fire();
		GameObject.Find("DoorIntro2").GetComponent<DoorEvent>().Fire();
	}
}
