using UnityEngine;
using System.Collections;

public class InGameMenuScript : MonoBehaviour {

	PlayerScript PScript;

	void Start () {
		PScript = GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ();
	}
	
	public void Continue () {
		Destroy (gameObject);
	}

	public void Restart () {
		MainMenu ();
		PScript.StartGame (PScript.GameMode);
	}

	public void MainMenu () {
		PScript.CmdConcedeGame ();
		PScript.ShowMainMenu ();
		Destroy (gameObject);
	}
	
	public void QuitGame () {
		Application.Quit ();
	}
}
