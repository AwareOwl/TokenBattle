using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

	/*
		Skrypt służący do komunikacji clienta z serwerem, oraz wykonywaniem działań w obu tych miejscach.
		
		Jeśli nazwa funkcji zaczyna się na S, to znaczy, że powinna być ona wywoływana na serwerze.
		Jeśli nazwa funkcji zaczyna się na C, to znaczy, że powinna być ona wywoływana na kliencie w trakcie gry.
		Jeśli nazwa funkcji zaczyna się na E, to znaczy, że powinna być ona wywoływana na kliencie w edytorze.
		Jeśli funkcja nie zaczyna się na żaden z powyższych znaków, to znaczy, że albo go tam nie dałem, 
		albo może być wywołana na dowolnym sprzęcie.
	*/

	public GameObject Menu;
	public GameObject Selection;

	GameObject SObject;
	SScript SScript;
	SScript.BasicGame Game;
	SScript.BasicGame.BasicPlayer Player;
	public int GameId;

	GameObject EObject;
	DeckEditorScript EScript;

	public string Nickname;

	public string GameMode = "";

	NetworkManagerHUD NetHUD () {
		return GameObject.Find ("NetManager").GetComponent<NetworkManagerHUD> ();
	}

	public GameObject CObject () {
		return GameObject.Find ("CObject");
	}

	public CScript CScript () {
		return CObject ().GetComponent<CScript> ();
	}

	void Start () {
		if (isLocalPlayer) {
			NetHUD ().enabled = false;
			GameObject Clone = Instantiate (Resources.Load ("PreLogin")) as GameObject;
			Clone.name = "PreLogin";
			Clone.transform.parent = GameObject.Find ("Canvas").transform;
			Clone.transform.localPosition = new Vector3 (0, 0, 0);
			name = "MyPlayer";
			CmdFindSScript ();
		}
		if (isServer) {
			SConvertVersion ();
		}
	}

	[Command]
	public void CmdFindSScript (){
		SObject = GameObject.Find ("SObject");
		SScript = SObject.GetComponent<SScript> ();
	}

	[Command]
	public void CmdRegister (string Nickname, string Email, string Password, string ConfirmPassword) {
		if (Nickname != "NormalAI" && Nickname != "EasyAI" && Nickname != "") {
			if (Password == ConfirmPassword) {
				if (!File.Exists (@"C:/TokenBattle/Users/" + Nickname + ".txt")) {
					string HandSets = "\r\nHandset\r\n0\r\n\r\nset 0\r\nSet 1\r\n0 1\r\n2 3\r\n4 5\r\n6 7";
					File.WriteAllText (@"C:/TokenBattle/Users/" + Nickname + ".txt", Nickname + "\r\n" + Email + "\r\n" + Password + HandSets);
					RpcShowMessage ("Account created.");
				} else {
					RpcShowMessage ("Nickname taken.");
				}
			} else {
				RpcShowMessage ("Password doesn't match.");
			}
		} else {
			RpcShowMessage ("Inwalid nickname.");
		}
	}


	public void SConvertVersion () {
		if (!Directory.Exists (@"C:/TokenBattle")) {
			Directory.CreateDirectory (@"C:/TokenBattle");
		}
		if (!Directory.Exists (@"C:/TokenBattle/Version")) {
			Directory.CreateDirectory (@"C:/TokenBattle/Version");
			File.WriteAllText (@"C:/TokenBattle/Version/version.txt", "v0.01");
			Directory.CreateDirectory (@"C:/TokenBattle/Users");
			string [] FileNames = Directory.GetFiles (@"C:/TokenBattle");
			foreach (string fileName in FileNames) {
				string fName = fileName.Remove (0, fileName.IndexOf ("\\") + 1);
				File.Move (@"C:/TokenBattle/" + fName, @"C:/TokenBattle/Users/" + fName);
				List <string> Lines = new List <string> (File.ReadAllLines (@"C:/TokenBattle/Users/" + fName));
				int HandsetNumber = 0;
				while (Lines.IndexOf ("set " + HandsetNumber.ToString ()) != -1) {
					Lines.Insert (Lines.IndexOf ("set " + HandsetNumber.ToString ()) + 1, "Set " + (HandsetNumber + 1).ToString ());
					HandsetNumber++;
				}
				File.WriteAllLines (@"C:/TokenBattle/Users/" + fName, Lines.ToArray());
			}
		}
		if (File.ReadAllText (@"C:/TokenBattle/Version/version.txt") == "v0.01") {
			string HandSets = "\r\nHandset\r\n0\r\n\r\nset 0\r\nSet 1\r\n0 1\r\n2 3\r\n4 5\r\n6 7";
			File.WriteAllText (@"C:/TokenBattle/Users/NormalAI.txt", "NormalAI\r\n\r\n "+ HandSets);
			File.WriteAllText (@"C:/TokenBattle/Version/version.txt", "v0.02");
		}

			/*
			string HandSets = "\r\nHandset\r\n0\r\n\r\nset 0\r\nSet 1\r\n0 1\r\n2 3\r\n4 5\r\n6 7";
		File.WriteAllText (@"C:/TokenBattle/Users/" + Nickname + ".txt", Nickname + "\r\n" + Email + "\r\n" + Password + HandSets);
		*/
	}
	

	[Command]
	public void CmdLogin (string Nickname, string Password) {
		if (File.Exists (@"C:/TokenBattle/Users/" + Nickname + ".txt")) {
			string [] Lines = File.ReadAllLines (@"C:/TokenBattle/Users/" + Nickname + ".txt");
			if (Lines [2] == Password) {
				ConnectPlayer (Nickname);
			} else {
				RpcShowMessage ("Password is incorrect.");
			}
		} else {
			RpcShowMessage ("User not found.");
		}
	}

	[ClientRpc]
	public void RpcShowMessage (string s) {
		if (isLocalPlayer) {
			GameData.ShowMessage (s);
		}
	}

	[ClientRpc]
	public void RpcShowMessageWithOption (string s, string option) {
		if (isLocalPlayer) {
			GameData.ShowMessage (s, option);
		}
	}

	[ClientRpc]
	public void RpcSpawnCScript () {
		if (isLocalPlayer) {
			GameObject Clone = Instantiate (Resources.Load ("PreCObject")) as GameObject;
			Clone.name = "CObject";
		}
	}

	void OnDestroy () {
		if (isServer) {
			//DisconnectPlayer (Network.player);
		}
	}

	/*
	public void DisconnectPlayer (NetworkPlayer player) {
		for (int p = 0; p < SScript.NumberOfPlayers; p++) {
			if (SScript.Player [p].Connected == true && SScript.Player [p].Ip == player.ipAddress) {
				SScript.Player [p].Connected = false;
				break;
			}
		}
	}
	*/
	
	[Command]
	public void CmdConcedeGame () {
		Player.Conceded = true;
		if (Game.FinishedByConcede ()) {
			EndGame ();
		}
	}

	public void EndGame () {
		Game.Finished = true;
		RpcShowMessageToPlayers (Game.Id, "Player " + Game.FindWinner ().Nickname + " has won the match.", "ShowMainMenu");
	}

	[ClientRpc]
	public void RpcShowMessageToPlayers (int id, string message, string option) {
		if (CheckId (id) && CObject () != null) {
			GameData.ShowMessage (message, option);
		}
	}
	public void ConnectPlayer (string nickname) {
		if (!SScript.IsPlayerConnected (nickname)) {
			SScript.AddPlayer (nickname);
		}
		int id = SScript.GetPlayerId (nickname);
		FinishConnecting (id, nickname);
		/*
		bool done = false;
		for (int p = 0; p < SScript.NumberOfPlayers; p++) {
			if (SScript.Player [p].Nickname == Nickname && !done) {
				FinishConnecting (p, Nickname);
				done = true;
				break;
			}
		}
		for (int p = 0; p < SScript.NumberOfPlayers; p++) {
			if (SScript.Player [p].Connected == false && !done) {
				FinishConnecting (p, Nickname);
				done = true;
				break;
			}
		}
		if (!done) {
			RpcDebug ("Unable to connect");
		} 
		*/
	}

	public void FinishConnecting (int p, string nickname) {
		RpcSetPlayer (nickname);
		RpcShowMenu ();
	}

	[ClientRpc]
	public void RpcShowMenu () {
		if (isLocalPlayer) {
			ShowMainMenu ();
		}
	}
	
	public void ShowMainMenu () {
		if (CObject () != null) {
			Destroy (CObject ());
		}
		if (EObject != null) {
			Destroy (EObject);
		}
		if (Selection != null) {
			Destroy (Selection);
		}
		if (Menu != null) {
			Destroy (Menu);
		}
		if (GameObject.Find ("PreLogin") != null){
			Destroy (GameObject.Find ("PreLogin"));
		}
		SetNormalCamera ();
		GameObject Clone = Instantiate (Resources.Load ("PreMainMenu")) as GameObject;
		Clone.name = "MainMenu";
		Clone.transform.parent = GameObject.Find ("Canvas").transform;
		Clone.transform.localPosition = new Vector3 (0, 0, 0);
		Menu = Clone;
	}

	public void ShowGameMenu () {
		if (Menu != null) {
			Destroy (Menu);
		}
		GameObject Clone = Instantiate (Resources.Load ("PreGameMenu")) as GameObject;
		Clone.name = "MainMenu";
		Clone.transform.parent = GameObject.Find ("Canvas").transform;
		Clone.transform.localPosition = new Vector3 (0, 0, 0);
		Menu = Clone;
	}

	public void ShowHowToPlay () {
		if (Menu != null) {
			Destroy (Menu);
		}
		GameObject Clone = Instantiate (Resources.Load ("PreHowToPlay")) as GameObject;
		Clone.name = "HowToPlay";
		Clone.transform.localPosition = new Vector3 (0, 0, 0);
		Menu = Clone;
	}
	public void ShowInGameMenu () {
		GameObject Clone = Instantiate (Resources.Load ("PreInGameMenu")) as GameObject;
		Clone.name = "InGameMenu";
		Clone.transform.parent = GameObject.Find ("Canvas").transform;
		Clone.transform.localPosition = new Vector3 (0, 0, 0);
		Menu = Clone;
	}

	public void DeckEditor () {
		EObject = Instantiate (Resources.Load ("PreDeckEditor")) as GameObject;
		EObject.name = "EObject";
		EObject.transform.localPosition = new Vector3 (0, 0, 0);
		EScript = EObject.GetComponent<DeckEditorScript> ();
		SetNormalCamera ();
		Destroy (GameObject.Find ("MainMenu"));
		CmdDownloadSetToEditor ();
	}

	[Command]
	public void CmdDownloadSetToEditor () {
		RpcDownloadSetToEditor ();
	}


	[ClientRpc]
	public void RpcDownloadSetToEditor () {
		if (isLocalPlayer) {
			string [] Lines = File.ReadAllLines (@"C:/TokenBattle/Users/" + Nickname + ".txt");
			int HandsetNumber = int.Parse (Lines [Array.FindIndex (Lines, s => s.Equals ("Handset")) + 1]);
			int LineNumber = Array.FindIndex (Lines, s => s.Equals ("set " + HandsetNumber.ToString ()));
			LineNumber++;
			for (int x = 0; x < 4; x++) {
				string [] Stack = Lines [++LineNumber].Split (' ');
				int y = 0;
				foreach (string number in Stack) {
					PickCardToSet (x, y, int.Parse (number));
					y++;
				}
			}
		}
	}

	public void SetGameCamera () {
		GameObject Camera = GameObject.Find ("MainCamera");
		Camera.GetComponent<Camera> ().orthographic = false;
		Camera.transform.localPosition = new Vector3 (0, -2.85f, -7.85f);
		Camera.transform.localEulerAngles = new Vector3 (345, 0, 0);
	}

	public void SetNormalCamera () {
		GameObject Camera = GameObject.Find ("MainCamera");
		Camera.GetComponent<Camera> ().orthographic = true;
		Camera.transform.localPosition = new Vector3 (0, 0, -10f);
		Camera.transform.localEulerAngles = new Vector3 (0, 0, 0);

	}

	public void StartGame (string gameMode) {
		CmdStartGame (gameMode, Nickname);
	}

	[Command]
	public void CmdStartGame (string gameMode, string nickname) {
		GameMode = gameMode;
		JoinGame (nickname);
		RpcDestroyMainMenu ();
	}

	[ClientRpc]
	public void RpcDestroyMainMenu () {
		if (isLocalPlayer) {
			Destroy (GameObject.Find ("MainMenu"));
		}
	}

	public bool CheckId (int gameId) {
		if (GameObject.Find ("MyPlayer").GetComponent <PlayerScript> ().GameId == gameId) {
			return true;
		} else {
			return false;
		}
	}

	public void JoinGame (string nickname) {
		SetGameCamera ();
		SScript.SetPlayerInGame (nickname, true);
		switch (GameMode) {
			case "LANGame":
				Player = SScript.JoinEmptyGame (nickname, 2);
				break;
			case "EasyAI":
				Player = SScript.JoinNewGame (nickname, 2);
				break;
			case "NormalAI":
				Player = SScript.JoinNewGame (nickname, 2);
				break;
			case "Sandbox":
				Player = SScript.JoinNewGame (nickname, 1);
				break;
			case "Hotseat":
				Player = SScript.JoinNewGame (nickname, 2);
				break;
		}
		Game = SScript.GetGame (nickname);
		GameId = Game.Id;
		RpcJoinGame (GameId);
		SetHandset (nickname, Player);
		
		RpcSpawnCScript ();
		RpcSetPNumber (Player.Number);
		DownloadData ();
		for (int x = 0; x < 4; x++) {
			RpcSetTopCardInQueue (GameId, x, STopCardInQueue (x));
		}
		// Specjal modes
		if (GameMode == "NormalAI") {
			SScript.JoinGame ("NormalAI", 2, GameId);
			SetHandset ("NormalAI", Game.NextPlayer (Player));
		} else if (GameMode == "EasyAI") {
			SScript.JoinGame ("EasyAI", 2, GameId);
			SetHandset ("NormalAI", Game.NextPlayer (Player));
		} else if (GameMode == "Hotseat") {
			SScript.JoinGame (nickname, 2, GameId);
			SetHandset (nickname, Game.NextPlayer (Player));
		}
	}

	[ClientRpc]
	public void RpcJoinGame (int id) {
		if (isLocalPlayer) {
			GameId = id;
		}
	}

	[ClientRpc]
	public void RpcSetPNumber (int pNumber) {
		if (isLocalPlayer) {
			CScript ().PNumber = pNumber;
		}
	}

	public void SetHandset (string nickname, SScript.BasicGame.BasicPlayer player) {
		string [] Lines = File.ReadAllLines (@"C:/TokenBattle/Users/" + nickname + ".txt");
		int HandsetNumber = int.Parse (Lines [Array.FindIndex (Lines, s => s.Equals ("Handset")) + 1]);
		int LineNumber = Array.FindIndex (Lines, s => s.Equals ("set " + HandsetNumber.ToString ()));
		LineNumber++;
		for (int x = 0; x < 4; x++) {
			string [] Stack = Lines [++LineNumber].Split (' ');
			player.CardQueue [x].QueueSize = Stack.Length;
			player.CardQueue [x].CardNumbers = new int [Stack.Length];
			int y = 0;
			foreach (string number in Stack) {
				player.CardQueue [x].CardNumbers [y] = int.Parse (number);
				y++;
			}
		}
	}

	[ClientRpc]
	public void RpcSetPlayer (string nick) {
		if (isLocalPlayer) {
			Nickname = nick;
		}
	}
	

	[Command]
	public void CmdDebug (string s) {
		RpcDebug (s);
	}

	[ClientRpc]
	public void RpcSetTopCardInQueue (int id, int x, int number) {
		if (CheckId (id) && CObject () != null && isLocalPlayer) {
			CScript ().CardQueue [x].CardNumber = number;
			CSetCard (x);
		}
	}

	public int CGetCardNumber (int x) {
		return CScript ().CardQueue [x].CardNumber;
	}

	public void CSetCard (int x) {
		GameData.SetCard (CScript ().CardQueue [x].Background, CGetCardNumber (x));
	}

	void Update () {
		if (isLocalPlayer) {
			if (Input.GetKeyDown ("f10")) {
				if (CObject () != null) {
					ShowInGameMenu ();
				}
			}
		}
	}

	public GameObject CToken (int x, int y) {
		return CScript ().Token [x, y];
	}

	public GameObject CTokenRange () {
		return CScript ().TokenRange;
	}

	public GameObject CSetToken (int x, int y) {
		return CScript ().Token [x, y] = CCreateToken (x, y, true);
	}

	public GameObject CSetToken (int x, int y, GameObject token) {
		return CScript ().Token [x, y] = token;
	}

	public GameObject CSetTokenRange (int x, int y) {
		return CScript ().TokenRange = CCreateToken (x, y, false);
	}
	
	public SScript.BasicGame.BasicToken SToken (int x, int y) {
		return Game.Token [x, y];
	}

	public void SSwapToken (int x, int y, int x2, int y2) {
		SScript.BasicGame.BasicToken temp = Game.Token [x, y];
		Game.Token [x, y] = Game.Token [x2, y2];
		Game.Token [x2, y2] = temp;
	}

	public GameObject CField (int x, int y) {
		return CScript ().Field [x, y];
	}

	public int SGetTokenType (int x, int y) {
		return Game.Token [x, y].Type;
	}

	public void SSetTokenType (int x, int y, int type) {
		Game.Token [x, y].Type = type;
	}

	public int SGetTokenValue (int x, int y) {
		return Game.Token [x, y].Value;
	}

	public void SSetTokenValue (int x, int y, int value) {
		Game.Token [x, y].Value = value;
	}

	public int SGetTokenPlayer (int x, int y) {
		return Game.Token [x, y].Player;
	}

	public void SSetTokenPlayer (int x, int y, int player) {
		Game.Token [x, y].Player = player;
	}

	public bool SGetTokenExist (int x, int y) {
		return Game.Token [x, y].Exist;
	}

	public void SSetTokenExist (int x, int y, bool exist) {
		Game.Token [x, y].Exist = exist;
	}

	public bool SGetTokenChecked (int x, int y) {
		return Game.Token [x, y].AlredyChecked;
	}

	public void SSetTokenChecked (int x, int y, bool alredyChecked) {
		Game.Token [x, y].AlredyChecked = alredyChecked;
	}

	public int SQueueTop (int QueueNumber) {
		return Player.CardQueue [QueueNumber].QueueTop;
	}

	public int SQueueSize (int QueueNumber) {
		return Player.CardQueue [QueueNumber].QueueSize;
	}

	public void SMoveQueue (int QueueNumber) {
		if (SQueueTop (QueueNumber) < SQueueSize (QueueNumber) - 1) {
			Player.CardQueue [QueueNumber].QueueTop++;
		} else {
			Player.CardQueue [QueueNumber].QueueTop = 0;
		}
	}

	public int STopCardInQueue (int QueueNumber) {
		return Player.CardQueue [QueueNumber].CardNumbers [SQueueTop (QueueNumber)];
	}

	public int STokenType (int QueueNumber) {
		return GameData.Card [STopCardInQueue (QueueNumber)].TokenType;
	}

	public int STokenValue (int QueueNumber) {
		return GameData.Card [STopCardInQueue (QueueNumber)].TokenValue;
	}

	public int SAbilityArea (int QueueNumber) {
		return GameData.Card [STopCardInQueue (QueueNumber)].AbilityArea;
	}

	public int SAbilitySize (int QueueNumber) {
		return GameData.Ability [SAbilityArea (QueueNumber)].AbilitySize;
	}

	public int SAbilityX (int QueueNumber, int Number) {
		return GameData.Ability [SAbilityArea (QueueNumber)].Pos [Number].x;
	}

	public int SAbilityY (int QueueNumber, int Number) {
		return GameData.Ability [SAbilityArea (QueueNumber)].Pos [Number].y;
	}

	public int SAbilityType (int QueueNumber) {
		return GameData.Card [STopCardInQueue (QueueNumber)].AbilityType;
	}

	public int CActiveQueue () {
		return CScript ().ActiveQueue;
	}

	public int CTopCardInQueue (int QueueNumber) {
		return CScript ().CardQueue [QueueNumber].CardNumber;
	}

	public int CActiveCard () {
		return CTopCardInQueue (CActiveQueue());
	}

	public int CTokenType () {
		return GameData.Card [CActiveCard ()].TokenType;
	}

	public int CTokenValue () {
		return GameData.Card [CActiveCard ()].TokenValue;
	}

	public int CAbilityArea () {
		return GameData.Card [CActiveCard ()].AbilityArea;
	}

	public int AbilityArea (int number) {
		return GameData.Card [number].AbilityArea;
	}

	public int CAbilitySize () {
		return GameData.Ability [CAbilityArea ()].AbilitySize;
	}

	public int CAbilityX (int Number) {
		return GameData.Ability [CAbilityArea ()].Pos [Number].x;
	}

	public int CAbilityY (int Number) {
		return GameData.Ability [CAbilityArea ()].Pos [Number].y;
	}

	public int CAbilityType () {
		return GameData.Card [CActiveCard ()].AbilityType;
	}

	public int CGetPNumber () {
		return CScript ().PNumber;
	}

	public IEnumerator UseToken (int x, int y) {
		CmdUseToken (x, y, CActiveQueue ());
		yield return new WaitForSeconds (0.25f);
	}

	public int NicknameToNumber (string Nickname) {
		int number = -1;
		for (int x = 0; x < Game.NumberOfPlayers; x++) {
			if (Nickname == SGetPlayerNickname (x)) {
				number = x;
			}
		}
		return number;
	}

	public SScript.BasicGame.BasicPlayer SGetPlayer (int number) {
		return Game.Player [number];
	}

	public string SGetPlayerNickname (int number) {
		return SGetPlayer (number).Nickname;
	}

	public void CSetTokenPlayer (int x, int y, int player) {
		if (player == 0) {
			CToken (x, y).GetComponent<Renderer> ().material.color = new Color (0.65f, 0.9f, 0.55f);
		} else {
			CToken (x, y).GetComponent<Renderer> ().material.color = new Color (0.85f, 0.6f, 0.6f);
		}
	}

	public void CSetTokenValue (int x, int y, int value) {
		CToken (x, y).transform.Find ("Text").GetComponent<TextMesh> ().text = value.ToString ();
	}

	public void CSetTokenRangePlayer () {
		if (CScript ().PNumber == 0) {
			CTokenRange ().GetComponent<Renderer> ().material.color = new Color (0.65f, 0.9f, 0.55f);
		} else {
			CTokenRange ().GetComponent<Renderer> ().material.color = new Color (0.85f, 0.6f, 0.6f);
		}
	}

	public void CSetTokenRangeValue (int value) {
		CTokenRange ().transform.Find ("Text").GetComponent<TextMesh> ().text = value.ToString ();
	}

	[ClientRpc]
	public void RpcSetTokenValue (int id, int x, int y, int value) {
		if (CheckId (id) && CObject () != null) {
			CSetTokenValue (x, y, value);
		}
	}
	
	public IEnumerator SCheckToken (int x, int y) {
		if (!CheckWithMap (x, y)) {
			RpcDebug ("Error Code: A01");
		}
		if (SGetTokenExist (x, y)) {
			yield return new WaitForSeconds (0.25f);
			RpcSetTokenValue (GameId, x, y, SGetTokenValue (x, y));
			if (SGetTokenValue (x, y) <= 0) {
				SSetTokenExist (x, y, false);
				RpcAddRisingScript (GameId, x, y);
			}
		}
	}

	[ClientRpc]
	public void RpcAddRisingScript (int id, int x, int y) {
		if (CheckId (id) && CObject () != null) {
			CToken (x, y).AddComponent<RisingScript> ();
		}
	}

	public void CSpawnToken (int x, int y, int player, int type, int value) {
		CSetToken (x, y);
		CSetTokenPlayer (x, y, player);
		CSetTokenValue (x, y, value);
	}
	public bool CheckWithMap (int x, int y) {
		return x >= 0 && x < GameData.MapSizeX && y >= 0 && y < GameData.MapSizeY;
	}
	
	public void DownloadData () {
		for (int x = 0; x < GameData.MapSizeX; x++) {
			for (int y = 0; y < GameData.MapSizeY; y++) {
				if (Game.Token [x, y].Exist) {
					SSpawnToken (x, y, false);
				}
			}
		}
		for (int x = 0; x < Game.NumberOfPlayers; x++) {
			CSetScore (x);
		}
	}

	public string SGetName (int player) {
		return Game.Player [player].Nickname;
	}

	public int SGetScore (int player) {
		return Game.Player [player].Score;
	}

	public int SGetDeltaScore (int player) {
		return Game.Player [player].DeltaScore;
	}

	public void CSetScore (int player) { // Ta funkcja wyjątkowo jest wywoływana na serwerze.
		RpcSetScore (GameId, player, SGetName (player), SGetScore (player), SGetDeltaScore (player));
	}

	[ClientRpc]
	public void RpcSetScore (int id, int player, string name, int value, int delta) {
		if (CheckId (id) && CObject () != null) {
			CScript ().PlayerScoreText [player].GetComponent<TextMesh>().text = name + ":\n" + 
				value.ToString () + " (+" + delta.ToString () + ")";
		}
	}

	[Command]
	public void CmdUseToken (int x, int y, int CardNumber) {
		UseToken (x, y, CardNumber);
	}

	public void PickCardToSet (int queue, int y) {
		PickCardToSet (queue, y, EScript.SelectedCardInCollection);
	}

	public void PickCardToSet (int queue, int y, int number) {
		if (!EScript.CardQueue [queue, y].Empty) {
			RemoveCardFromSet (queue, y);
		}
		if (number >= 0 && !EScript.CardInCollection [number].Empty) {
			GameData.SetCard (EScript.CardQueue [queue, y].Card, number);
			ESetCover (EScript.CardQueue [queue, y].Cover, false);
			EScript.CardQueue [queue, y].Number = number;
			EScript.CardQueue [queue, y].Empty = false;

			ESetCover (EScript.CardInCollection [number].Cover, true);
			EScript.CardInCollection [number].Empty = true;
		}
	}

	public void RemoveCardFromSet (int queue, int y) {
		if (!EScript.CardQueue [queue, y].Empty) {
			EScript.CardInCollection [EScript.CardQueue [queue, y].Number].Empty = false;
			ESetCover (EScript.CardInCollection [EScript.CardQueue [queue, y].Number].Cover, false);
		}
		ESetCover (EScript.CardQueue [queue, y].Cover, true);
		EScript.CardQueue [queue, y].Empty = true;

	}
	
	public bool IsHandsetCorrect () {
		bool Correct = true;
		for (int x = 0; x < 4; x++) {
			int CardsInQueue = 0;
			for (int y = 0; y < 5; y++) {
				if (!EScript.CardQueue [x, y].Empty) {
					CardsInQueue++;
				}
			}
			if (CardsInQueue < 2) {
				Correct = false;
			}
		}
		return Correct;
	}

	public string [] HandsetToString () {
		string [] s = new string [4];
		for (int x = 0; x < 4; x++) {
			bool space = false;
			for (int y = 0; y < 5; y++) {
				if (!EScript.CardQueue [x, y].Empty) {
					if (space) {
						s [x] += " ";
					}
					s [x] += EScript.CardQueue [x, y].Number.ToString ();
					space = true;
				}
			}
		}
		return s;
	}
	
	[Command]
	public void CmdSaveHandset (string [] NewSet) {
		string [] Lines = File.ReadAllLines (@"C:/TokenBattle/Users/" + Nickname + ".txt");
		if (IsHandsetCorrect ()) {
			for (int x = 0; x < 4; x++) {
				string [] Stack = NewSet [x].Split (' ');
			}
			int HandsetNumber = int.Parse (Lines [Array.FindIndex (Lines, s => s.Equals ("Handset")) + 1]);
			int LineNumber = Array.FindIndex (Lines, s => s.Equals ("set " + HandsetNumber.ToString ()));
			LineNumber++;
			for (int x = 0; x < 4; x++) {
				Lines [++LineNumber] = NewSet [x];
			}
			File.WriteAllLines (@"C:/TokenBattle/Users/" + Nickname + ".txt", Lines);
			RpcShowMenu ();
		} else {
			RpcShowMessage ("You need to have at least 2 cards in each queue.");
		}
	}

	public void ESetCover (GameObject Cover, bool Enabled) {
		if (Cover.GetComponent<CoverScript> () != null) {
			Cover.GetComponent<CoverScript> ().timer = 0;
			Cover.GetComponent<CoverScript> ().Enabled = Enabled;
		} else {
			Cover.AddComponent<CoverScript> ().Enabled = Enabled;
		}

	}

	public void UseToken (int x, int y, int CardNumber) {
		if (!SGetTokenExist (x, y) && (GameMode == "Sandbox" || Game.TurnOfPlayer == Player.Number) && !Game.Finished) {
			SSetTokenExist (x, y, true);
			SSetTokenType (x, y, STokenType (CardNumber));
			SSetTokenValue (x, y, STokenValue (CardNumber));
			SSetTokenPlayer (x, y, Player.Number);
			SSpawnToken (x, y, true);
			SetCheckedAll (false);
			for (int ta = 0; ta < SAbilitySize (CardNumber); ta++) {
				int cx = x + SAbilityX (CardNumber, ta);
				int cy = y + SAbilityY (CardNumber, ta);
				if (CheckWithMap (cx, cy)) {
					switch (SAbilityType (CardNumber)) {
						case 1:
							RpcCreateEffect (GameId, cx, cy);
							SSetTokenValue (cx, cy, SGetTokenValue (cx, cy) - 1);
							StartCoroutine (SCheckToken (cx, cy));
							break;
						case 2:
							RpcCreateArrowEffect (GameId, cx, cy, x, y);
							if (SGetTokenExist (cx, cy) && !SGetTokenChecked (cx, cy)) {
								int cx2 = x + 2 * SAbilityX (CardNumber, ta);
								int cy2 = y + 2 * SAbilityY (CardNumber, ta);
								if (CheckWithMap (cx2, cy2)) {
									if (!SGetTokenExist (cx2, cy2)) {
										RpcPushToken (GameId, cx, cy, cx2, cy2);
										SSwapToken (cx, cy, cx2, cy2);
										SSetTokenChecked (cx2, cy2, true);
									}
								} else {
									RpcPushToken (GameId, cx, cy, cx2, cy2);
									SSetTokenExist (cx, cy, false);
								}
							}
							break;
						case 3:
							RpcCreateArrowEffect (GameId, cx, cy, 2 * cx - x, 2 * cy - y);
							if (SGetTokenExist (cx, cy) && !SGetTokenChecked (cx, cy)) {
								int cx2 = x - SAbilityX (CardNumber, ta);
								int cy2 = y - SAbilityY (CardNumber, ta);
								if (CheckWithMap (cx2, cy2)) {
									if (!SGetTokenExist (cx2, cy2)) {
										RpcPushToken (GameId, cx, cy, cx2, cy2);
										SSwapToken (cx, cy, cx2, cy2);
										SSetTokenChecked (cx2, cy2, true);
									}
								} else {
									RpcPushToken (GameId, cx, cy, cx2, cy2);
									SSetTokenExist (cx, cy, false);
								}
							}
							break;
					}
				}
			}
			Player.UsedTokens++;
			Player.UsedTokensByAbilityType [SAbilityType (CardNumber)]++;
			SMoveQueue (CardNumber);
			EndTurn ();
			if (Player.Nickname != "NormalAI" && Player.Nickname != "EasyAI") {
				RpcSetTopCardInQueue (GameId, CardNumber, STopCardInQueue (CardNumber));
				if (GameMode == "EasyAI" || GameMode == "NormalAI" || GameMode == "Hotseat") {
					Player = Game.NextPlayer (Player);
				}
				if (GameMode == "EasyAI") {
					StartCoroutine (RunAI (NicknameToNumber ("EasyAI"), 0.25f));
				}
				if (GameMode == "NormalAI") {
					StartCoroutine (RunAI (NicknameToNumber ("NormalAI"), 0.25f));
				}
				if (GameMode == "Hotseat") {
					RpcSetPNumber (Player.Number);
					for (int queue = 0; queue < 4; queue++) {
						RpcSetTopCardInQueue (GameId, queue, STopCardInQueue (queue));
					}
				}
			} else {
				Player = Game.NextPlayer (Player);
			}
		}

	}

	public void SetCheckedAll (bool alredyChecked) {
		for (int x = 0; x < GameData.MapSizeX; x++) {
			for (int y = 0; y < GameData.MapSizeY; y++) {
				SSetTokenChecked (x, y, alredyChecked);

			}
		}
	}

	public void EndTurn () {
		int [] Score = new int [Game.NumberOfPlayers];
		bool FieldFilled = true;
		for (int x = 0; x < GameData.MapSizeX; x++) {
			for (int y = 0; y < GameData.MapSizeY; y++) {
				if (SGetTokenExist (x, y)) {
					Score [SGetTokenPlayer (x, y)] += SGetTokenValue (x, y);
				} else {
					FieldFilled = false;
				}
			}
		}
		int BestScore = 0;
		for (int p = 0; p < Game.NumberOfPlayers; p++) {
			Game.Player [p].Score += Score [p];
			if (Game.Player [p].Score > BestScore) {
				BestScore = Game.Player [p].Score;
			}
			Game.Player [p].DeltaScore = Score [p];
			CSetScore (p);
		}
		if (FieldFilled || BestScore >= 1000) {
			EndGame ();
		}
		Game.TurnOfPlayer = (Game.TurnOfPlayer + 1) % Game.NumberOfPlayers;
	}

	[ClientRpc]
	public void RpcCreateEffect (int id, int x, int y) {
		if (CheckId (id) && CObject () != null && CheckWithMap (x, y)) {
			StartCoroutine (CreateEffect (x, y));
		}
	}

	[ClientRpc]
	public void RpcCreateArrowEffect (int id, int x, int y, int fromX, int fromY) {
		if (CheckId (id) && CObject () != null && CheckWithMap (x, y)) {
			StartCoroutine (CreateArrowEffect (x, y, fromX, fromY));
		}
	}

	public IEnumerator CreateEffect (int x, int y) {
		yield return new WaitForSeconds (0.25f);
		PreAbility (x, y, true);
		PreFlatAbility (x, y, true);
	}

	public IEnumerator CreateArrowEffect (int x, int y, int fromX, int fromY) {
		yield return new WaitForSeconds (0.25f);
		PreArrow (x, y, fromX, fromY, true);
	}

	public GameObject PreAbility (int x, int y, bool autoDestroy) {
		GameObject Clone = Instantiate (Resources.Load ("PreAbility")) as GameObject;
		Clone.transform.parent = CField (x, y).transform;
		Clone.transform.localPosition = new Vector3 (0, 0, -0.01f);
		Clone.transform.localScale = new Vector3 (1, 1, 0.75f);
		Clone.AddComponent<EvadingScript> ();
		Clone.GetComponent<EvadingScript> ().AutoDestroy = autoDestroy;
		return Clone;
	}

	public GameObject PreFlatAbility (int x, int y, bool autoDestroy) {
		GameObject Clone = Instantiate (Resources.Load ("PreFlatAbility")) as GameObject;
		Clone.transform.parent = CField (x, y).transform;
		Clone.transform.localPosition = new Vector3 (0, 0, -0.02f);
		Clone.transform.localScale = new Vector3 (1, 1, 1);
		Clone.GetComponent<FlatEvading> ().AutoDestroy = autoDestroy;
		return Clone;
	}

	public GameObject PreArrow (int x, int y, int fromX, int fromY, bool autoDestroy) {
		GameObject Clone = Instantiate (Resources.Load ("PreArrow")) as GameObject;
		Clone.transform.parent = CField (x, y).transform;
		Clone.transform.localPosition = new Vector3 (0, 0, -0.03f);
		Clone.transform.localScale = new Vector3 (0.9f, 0.9f, 1);
		Clone.transform.Rotate (Vector3.forward, Atan3 (x - fromX, y - fromY));
		Clone.GetComponent<ArrowScript> ().AutoDestroy = autoDestroy;
		return Clone;
	}

	public float Atan3 (int x, int y) {
		return -180 * Mathf.Atan2 (x, y) / Mathf.PI + 90;
	}

	void SSpawnToken (int x, int y, bool OnAllClients) {
		RpcSpawnToken (GameId, x, y, SGetTokenPlayer (x, y), SGetTokenType (x, y), SGetTokenValue (x, y), OnAllClients);
	}

	[ClientRpc]
	public void RpcDebug (string s) {
		Debug.Log (s);
	}

	[ClientRpc]
	public void RpcSpawnToken (int id, int x, int y, int player, int type, int value, bool OnAllClients) {
		if (CheckId (id) && CObject () != null && (OnAllClients || isLocalPlayer)) {
			CSpawnToken (x, y, player, type, value);
		} 
	}

	public GameObject CCreateToken (int x, int y, bool falling) {
		GameObject Clone = Instantiate (Resources.Load ("PreToken")) as GameObject;
		Clone.transform.parent = CField (x, y).transform;
		if (falling) {
			Clone.transform.localPosition = new Vector3 (0, 0, -7.5f);
			Clone.AddComponent<FallingScript> ();
		} else {
			Clone.transform.localPosition = new Vector3 (0, 0, -0.01f);
		}
		Clone.transform.localScale = new Vector3 (0.9f, 0.9f, 1);
		GameObject Clone2 = Instantiate (Resources.Load ("PreText")) as GameObject;
		Clone2.name = "Text";
		Clone2.transform.parent = Clone.transform;
		Clone2.transform.localPosition = new Vector3 (0, 0, -0.01f);
		Clone2.transform.localScale = new Vector3 (0.9f, 0.9f, 1);
		Clone2.GetComponent<TextMesh> ().text = "0";
		return Clone;
	}

	[ClientRpc]
	public void RpcPushToken (int id, int firstX, int firstY, int secondX, int secondY) {
		if (CheckId (id) && CObject () != null && CToken (firstX, firstY) != null) {
			CToken (firstX, firstY).AddComponent<PushingScript> ();
			CToken (firstX, firstY).GetComponent<PushingScript> ().EndingPosition = CField (firstX, firstY).transform.position;
			CToken (firstX, firstY).GetComponent<PushingScript> ().EndingPosition.x += (secondX - firstX) * GameData.DiestanceBetweenFields;
			CToken (firstX, firstY).GetComponent<PushingScript> ().EndingPosition.y += (secondY - firstY) * GameData.DiestanceBetweenFields;
			if (CheckWithMap (secondX, secondY)) {
				CSetToken (secondX, secondY, CToken (firstX, firstY));
				CToken (secondX, secondY).transform.parent = CField (secondX, secondY).transform;
			} else {
				CToken (firstX, firstY).AddComponent<RisingScript> ();
			}
			CSetToken (firstX, firstY, null);
		}
	}

	public void ShowRange () { // Pokazuje graczowi na jakie pola na planszy wpływnie żeton
		int x = CScript ().MouseOverFieldX;
		int y = CScript ().MouseOverFieldY;
		if (CActiveQueue () >= 0 && CToken (x, y) == null /*  && TurnOfPlayer == PNumber */) {
			CSetTokenRange (x, y);
			CSetTokenRangePlayer ();
			CSetTokenRangeValue (CTokenValue ());
			for (int z = 0; z < CAbilitySize (); z++) {
				int cx = x + CAbilityX (z);
				int cy = y + CAbilityY (z);
				if (CheckWithMap (cx, cy)) {
					if (CAbilityType () == 1) {
						CScript ().AbilityRange [cx, cy] = PreFlatAbility (cx, cy, false);
					} else if (CAbilityType () == 2) {
						CScript ().AbilityRange [cx, cy] = PreArrow (cx, cy, x, y, false);
					} else if (CAbilityType () == 3) {
						CScript ().AbilityRange [cx, cy] = PreArrow (cx, cy, 2 * cx - x, 2 * cy - y, false);
					}
				}
			}
		}
	}

	public void HideRange () {
		for (int x = 0; x < GameData.MapSizeX; x++) {
			for (int y = 0; y < GameData.MapSizeY; y++) {
				if (CScript ().AbilityRange [x, y] != null) {
					Destroy (CScript ().AbilityRange [x, y]);
				}
			}
		}
		if (CScript ().TokenRange != null) {
			Destroy (CScript ().TokenRange);
		}
	}

	public IEnumerator RunAI (int PNumber, float delay) {
		yield return new WaitForSeconds (delay);
		RunAI (PNumber);
	}


	public void RunAI (int PNumber) {
		float [] AbilityTypesUsedByOpponents = new float [4];
		for (int x = 0; x < 4; x++) {
			AbilityTypesUsedByOpponents [x] = Game.CheckUsedAbilityTypeByOpponents (Player, x);
		}
		int MaxC = GameData.HandSize;
		int SizeX = GameData.MapSizeX;
		int SizeY = GameData.MapSizeY;
		int SizePow = SizeX * SizeY;
		int [] RNG = new int [SizePow];
		int [,] RNGOrder = new int [SizeX, SizeY];
		int [] RNGCardOrder = new int [MaxC];
		for (int x = 0; x < SizePow; x++) {
			RNG [x] = x;
		}
		Array.Sort (RNG);
		for (int x = 0; x < MaxC; x++) {
			RNGCardOrder [x] = x;
		}
		Array.Sort (RNGCardOrder);
		for (int x = 0; x < SizeX; x++) {
			for (int y = 0; y < SizeY; y++) {
				RNGOrder [x, y] = RNG [x * SizeY + y];
			}
		}
		int BestValue = -100;
		int BestC = 0;
		int BestX = 0;
		int BestY = 0;
		for (int c = 0; c < MaxC; c++) {
			int Card = RNGCardOrder [c];
			for (int x = 0; x < SizeX; x++) {
				for (int y = 0; y < SizeY; y++) {
					if (!SGetTokenExist (x, y)) {
						int TempValue = STokenValue (Card);
						if (GameMode == "EasyAI") {
							TempValue -= UnityEngine.Random.Range (0, 3);
						}
						if (GameMode == "NormalAI") {
							if (x == 0 || x == SizeX - 1 || y == 0 || y == SizeY - 1) {
								if (AbilityTypesUsedByOpponents [2] > 0.35f && STokenValue (Card) > 1) {
									TempValue -= 1;
								}
								if (AbilityTypesUsedByOpponents [2] > 0.15f && STokenValue (Card) > 2) {
									TempValue -= 1;
								}
							}
							if (x == 1 || x == SizeX - 2 || y == 1 || y == SizeY - 2) {
								if (AbilityTypesUsedByOpponents [3] > 0.35f && STokenValue (Card) > 1) {
									TempValue -= 1;
								}
								if (AbilityTypesUsedByOpponents [3] > 0.15f && STokenValue (Card) > 2) {
									TempValue -= 1;
								}
							}
						}
						// Uwzględnienie siły umiejętności
						for (int z = 0; z < SAbilitySize (Card); z++) {
							int cx = x + SAbilityX (Card, z);
							int cy = y + SAbilityY (Card, z);
							if (CheckWithMap (cx, cy)) {
								switch (SAbilityType (Card)) {
									case 1:
										if (SGetTokenExist (cx,cy)) {
											if (SGetTokenPlayer (cx, cy) == PNumber) {
												TempValue -= 1;
											} else {
												TempValue += 1;
											}
										}
										break;
									case 2:
										if (SGetTokenExist (cx, cy)) {
											int cx2 = x + 2 * SAbilityX (Card, z);
											int cy2 = y + 2 * SAbilityY (Card, z);
											if (!CheckWithMap (cx2, cy2)) {
												if (SGetTokenPlayer (cx, cy) == PNumber) {
													TempValue -= SGetTokenValue (cx, cy);
												} else {
													TempValue += SGetTokenValue (cx, cy);
												}
											}
										}
										break;
								}
							}
						}
						if (TempValue > BestValue || (TempValue == BestValue && RNGOrder [x, y] > RNGOrder [BestX, BestY])) {
							BestValue = TempValue;
							BestC = Card;
							BestX = x;
							BestY = y;
						}
					}
				}
			}
		}
		//ActiveCard [PNumber] = BestC;
		/*
		Debug.Log ("BestValue: " + BestValue.ToString ());
		for (int x = 1; x <= BestValue; x++) {
			Debug.Log (BestC);
		}
		*/
		UseToken (BestX, BestY, BestC);
	}
}
