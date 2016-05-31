using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {

	int Page = 1;
	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if (Page < 2) {
				Page++;
				GetComponent <Renderer>().material.mainTexture = Resources.Load ("HowToPlay/HowToPlay" + Page.ToString ()) as Texture;
			} else {
				GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ().ShowMainMenu ();
				Destroy (gameObject);
			}
		}
	}
}
