using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class LoginScript : MonoBehaviour {

	public GameObject LoginNickname;
	public GameObject LoginPassword;
	public GameObject LoginButton;
	public GameObject RegisterNickname;
	public GameObject RegisterEmail;
	public GameObject RegisterPassword;
	public GameObject RegisterConfirmPassword;
	public GameObject RegisterButton;
	public string LoginNicknameText;
	public string LoginPasswordText;
	public string RegisterNicknameText;
	public string RegisterEmailText;
	public string RegisterPasswordText;
	public string RegisterConfirmPasswordText;

	PlayerScript PScript;

	void Start () {
		PScript = GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ();
	}

	public void Login () {
		PScript.CmdLogin (LoginNicknameText, LoginPasswordText);
	}

	public void Register () {
		PScript.CmdRegister (RegisterNicknameText, RegisterEmailText, RegisterPasswordText, RegisterConfirmPasswordText);
	}

	public void ChangeLoginNickname () {
		LoginNicknameText = LoginNickname.GetComponent<InputField> ().text;
	}

	public void ChangeLoginPassword () {
		LoginPasswordText = LoginPassword.GetComponent<InputField> ().text;
	}

	public void ChangeRegisterNickname () {
		RegisterNicknameText = RegisterNickname.GetComponent<InputField> ().text;
	}

	public void ChangeRegisterEmail () {
		RegisterEmailText = RegisterEmail.GetComponent<InputField> ().text;
	}

	public void ChangeRegisterPassword () {
		RegisterPasswordText = RegisterPassword.GetComponent<InputField> ().text;
	}

	public void ChangeRegisterConfirmPassword () {
		RegisterConfirmPasswordText = RegisterConfirmPassword.GetComponent<InputField> ().text;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (LoginNickname.GetComponent<InputField>().isFocused) {
				LoginPassword.GetComponent<InputField> ().Select();
			} else if (LoginPassword.GetComponent<InputField> ().isFocused) {
				RegisterNickname.GetComponent<InputField> ().Select ();
			} else if (RegisterNickname.GetComponent<InputField> ().isFocused) {
				RegisterEmail.GetComponent<InputField> ().Select ();
			} else if (RegisterEmail.GetComponent<InputField> ().isFocused) {
				RegisterPassword.GetComponent<InputField> ().Select ();
			} else if (RegisterPassword.GetComponent<InputField> ().isFocused) {
				RegisterConfirmPassword.GetComponent<InputField> ().Select ();
			} else if (RegisterConfirmPassword.GetComponent<InputField> ().isFocused) {
				LoginNickname.GetComponent<InputField> ().Select ();
			}
		}
	}
	

	// Use this for initialization
	/*
	void OnGUI () {
		GUINickName = GUI.TextField (new Rect (220, 40, 200, 20), GUINickName, 25);
		GUIPassword = GUI.TextField (new Rect (220, 65, 200, 20), GUIPassword, 25);
		if (GUI.Button (new Rect (220, 90, 200, 20), "Apply")) {
			GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ().LogIn (GUINickName);
			//gameObject.GetComponent<PlayerScript> ().LogIn (GUINickName);
			Destroy (gameObject);
		}
	}
	*/
	
}
