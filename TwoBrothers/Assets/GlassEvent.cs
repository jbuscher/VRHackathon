using UnityEngine;
using System.Collections;

public class GlassEvent : MonoBehaviour {
	
	public void Fire() {
		Destroy(gameObject);
	}
}