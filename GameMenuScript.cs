using UnityEngine;
using System.Collections;

public class GameMenuScript : MonoBehaviour {

	PlayerScript PScript;

	void Start () {
		PScript = GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ();
	}

	public void GameVsEasyAI () {
		PScript.StartGame ("EasyAI");
	}

	public void GameVsNormalAI () {
		PScript.StartGame ("NormalAI");
	}

	public void LANGame () {
		PScript.StartGame ("LANGame");
	}

	public void Hotseat () {
		PScript.StartGame ("Hotseat");
	}

	public void Sandbox () {
		PScript.StartGame ("Sandbox");
	}

	public void MainMenu () {
		PScript.ShowMainMenu ();
	}
}
