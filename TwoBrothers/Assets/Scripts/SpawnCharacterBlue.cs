using UnityEngine;
using System.Collections;

public class SpawnCharacterBlue : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		if (Network.isClient) {
			StartCoroutine("DelayedStart");
		}
	}
	
	// Update is called once per frame
	IEnumerator DelayedStart() {
		yield return new WaitForSeconds(2);
		Network.Instantiate(player, this.transform.position, this.transform.rotation, 0);
	}
}
