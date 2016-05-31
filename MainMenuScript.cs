using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	PlayerScript PScript;

	void Start () {
		PScript = GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ();
	}

	public void GameMenu () {
		PScript.ShowGameMenu ();
	}

	public void DeckEditor () {
		PScript.DeckEditor ();
	}

	public void HowToPlay () {
		PScript.ShowHowToPlay ();
	}

	public void Exit () {
		Application.Quit ();
	}

}
