using UnityEngine;
using System.Collections;
using UnityEditor;

public class CollideLevelReset : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<NetworkView>() != null) {
			if (other.GetComponent<NetworkView>().isMine) {
				string[] temp = EditorApplication.currentScene.Split('.')[0].Split('/');
				GameObject.Find("NetworkManager").GetComponent<NetworkView>().RPC("LoadLevel", RPCMode.AllBuffered, temp[temp.Length - 1], Application.loadedLevel);
			}
		}
	}
}
