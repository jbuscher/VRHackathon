using UnityEngine;
using System.Collections;

public class SpawnCharacter : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		StartCoroutine("DelayedStart");
	}
	
	// Update is called once per frame
	IEnumerator DelayedStart() {
		yield return new WaitForSeconds(5);
		Network.Instantiate(player, this.transform.position, this.transform.rotation, 0);
	}
}
