using UnityEngine;
using System.Collections;

public class SelfDestroyScript : MonoBehaviour {

	public void SelfDestroy () {
		if (name == "ShowMainMenu") {
			GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ().ShowMainMenu ();
		}
		Destroy (gameObject.transform.parent.gameObject);
	}
}
