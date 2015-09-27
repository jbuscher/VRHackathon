using UnityEngine;
using System.Collections;

public class ButtonPlatformEvent : MonoBehaviour {

	public string platformTag;


	void hidePlatforms() {
		GameObject[] platforms = GameObject.FindGameObjectsWithTag (platformTag);
		foreach (GameObject platform in platforms) {
			platform.transform.Translate(Vector3.up * 100);
		}
	}

	void showPlatforms() {
		GameObject[] platforms =GameObject.FindGameObjectsWithTag (platformTag);
		foreach (GameObject platform in platforms) {
			platform.transform.Translate(Vector3.down * 100);
		}
	}
}
