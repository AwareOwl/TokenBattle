using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SScript : MonoBehaviour {

	/*
		Skrypt służący do przechowywania danych na serwerze i ich inicjalizacji.
	*/
	int LastGameId = 1;
	int LastPlayerId = 1;


	// Games

	List<BasicGame> Games = new List<BasicGame>();

	public class BasicGame {

		public bool Finished;
		public int Id;
		public int TurnOfPlayer;
		public int NumberOfPlayers;
		public BasicToken [,] Token;
		public BasicPlayer [] Player;

		public class BasicToken {

			public bool Exist;
			public bool AlredyChecked; 
			public int Type;
			public int Value;
			public int Player;

			public BasicToken () {
				Exist = false;
				AlredyChecked = false;
				Type = new int ();
				Value = 0;
				Player = 0;
			}
		}

		public class BasicPlayer {
			public bool Connected;
			public bool Conceded;
			public int ID;
			public int Number;
			public int Score;
			public int DeltaScore;
			public int UsedTokens;
			public int [] UsedTokensByAbilityType;
			public string Nickname;
			public BasicCardQueue [] CardQueue;

			public BasicPlayer () {
				Connected = false;
				ID = 0;
				Number = 0;
				Score = 0;
				DeltaScore = 0;
				UsedTokens = 0;
				UsedTokensByAbilityType = new int [4];
				Nickname = "";
				CardQueue = new BasicCardQueue [4];
				for (int x = 0; x < 4; x++) {
					CardQueue [x] = new BasicCardQueue ();
				}
			}
		}

		public BasicPlayer NextPlayer (BasicPlayer player) {
			if (player.Number + 1 < NumberOfPlayers) {
				return Player [player.Number + 1];
			} else {
				return Player [0];
			}
		}

		public class BasicCardQueue {
			public int QueueSize;
			public int QueueTop;
			public int [] CardNumbers;

			public BasicCardQueue () {
			}
		}

		public BasicGame () {
			NewGame ();
		}

		public BasicGame (int id) {
			Id = id;
			NewGame ();
		}

		public bool FinishedByConcede () {
			int InPlay = 0;
			foreach (BasicPlayer player in Player) {
				if (!player.Conceded) {
					InPlay++;
				}
			}
			if (InPlay == 1) {
				return true;
			} else {
				return false;
			}
		}

		public BasicPlayer FindWinner () {
			int BestScore = -100;
			BasicPlayer winner = new BasicPlayer ();
			foreach (BasicPlayer player in Player) {
				if (!player.Conceded && player.Score > BestScore) {
					BestScore = player.Score;
					winner = player;
				}
			}
			return winner;
		}

		void NewGame () {
			Token = new BasicToken [GameData.MapSizeX, GameData.MapSizeY];
			for (int x = 0; x < GameData.MapSizeX; x++) {
				for (int y = 0; y < GameData.MapSizeY; y++) {
					Token [x, y] = new BasicToken ();
				}
			}
			TurnOfPlayer = 0;
		}

		void Balance () {
			if (NumberOfPlayers == 2) {
				int x = 2 + Random.Range (0, 2);
				int y = 2 + Random.Range (0, 2);
				Token [x, y].Exist = true;
				Token [x, y].Value = 2;
				Token [x, y].Type = 1;
				Token [x, y].Player = 1;
				Player [1].DeltaScore = 2;
			}
		}

		public void SetNumberOfPlayers (int numberOfPlayers) {
			NumberOfPlayers = numberOfPlayers;
			Player = new BasicPlayer [NumberOfPlayers];
			for (int x = 0; x < NumberOfPlayers; x++) {
				Player [x] = new BasicPlayer ();
				Player [x].Number = x;
			}
			Balance ();
		}

		public float CheckUsedAbilityTypeByOpponents (BasicPlayer player, int abilityType) {
			float amount = 0;
			int AllTokens = 0;
			foreach (BasicPlayer player2 in Player) {
				if (player2 != player) {
					amount += player2.UsedTokensByAbilityType [abilityType];
					AllTokens += player2.UsedTokens;
				}
			}
			amount /= AllTokens;
			return amount;
		}
	}

	public BasicGame CreateGame (int numberOfPlayers) {
		BasicGame newGame = new BasicGame (LastGameId++);
		newGame.SetNumberOfPlayers (numberOfPlayers);
		Games.Add (newGame);
		return newGame;
	}

	public BasicGame GetEmptyGame (int numberOfPlayers) {
		foreach (BasicGame game in Games) {
			if (!game.Finished && game.NumberOfPlayers == numberOfPlayers) {
				bool empty = false;
				foreach (BasicGame.BasicPlayer player in game.Player) {
					if (!player.Connected) {
						empty = true;
					}
				}
				if (empty) {
					return game;
				}
			}
		}
		return CreateGame (numberOfPlayers);
	}

	public BasicGame.BasicPlayer JoinNewGame (string nickname, int numberOfPlayers) {
		BasicGame newGame = CreateGame (numberOfPlayers);
		newGame.Player [0].Connected = true;
		newGame.Player [0].Nickname = nickname;
		return newGame.Player [0];
	}

	public BasicGame.BasicPlayer JoinGame (string nickname, int numberOfPlayers, int id) {
		BasicGame game = GetGame (id);
		foreach (BasicGame.BasicPlayer player in game.Player) {
			if (!player.Connected) {
				player.Connected = true;
				player.Nickname = nickname;
				return player;
			}
		}
		return game.Player [0];
	}

	public BasicGame.BasicPlayer JoinEmptyGame (string nickname, int numberOfPlayers) {
		BasicGame game = GetEmptyGame (numberOfPlayers);
		foreach (BasicGame.BasicPlayer player in game.Player) {
			if (!player.Connected || player.Nickname == nickname) {
				player.Connected = true;
				player.Nickname = nickname;
				return player;
			}
		}
		return game.Player [0];
	}

	public BasicGame GetGame (int id) {
		foreach (BasicGame game in Games) {
			if (game.Id == id && !game.Finished) {
				return game;
			}
		}
		return new BasicGame ();
	}

	public BasicGame GetGame (string nickname) {
		foreach (BasicGame game in Games) {
			if (!game.Finished) {
				foreach (BasicGame.BasicPlayer player in game.Player) {
					if (player.Nickname == nickname && !game.Finished) {
						return game;
					}
				}
			}
		}
		return new BasicGame ();
	}


	// Players

	List<BasicPlayer> Players = new List<BasicPlayer> ();

	public struct BasicPlayer {
		public bool Connected;
		public bool InGame;
		public string Ip;
		public string Nickname;
		public string Password;
		public int Id;
	}

	public void AddPlayer (string nickname) {
		BasicPlayer player = new BasicPlayer ();
		player.Nickname = nickname;
		player.Id = LastPlayerId++;
		Players.Add (player);
	}
	public BasicPlayer GetPlayer (int id) {
		foreach (BasicPlayer player in Players) {
			if (player.Id == id) {
				return player;
			}
		}
		return new BasicPlayer ();
	}

	public bool IsPlayerConnected (string nickname) {
		foreach (BasicPlayer player in Players) {
			if (player.Nickname == nickname) {
				return true;
			}
		}
		return false;
	}

	public BasicPlayer GetPlayer (string nickname) {
		foreach (BasicPlayer player in Players) {
			if (player.Nickname == nickname) {
				return player;
			}
		}
		return new BasicPlayer ();
	}

	public int GetPlayerId (string nickname) {
		foreach (BasicPlayer player in Players) {
			if (player.Nickname == nickname) {
				return player.Id;
			}
		}
		return 0;
	}

	public bool GetPlayerInGame (string nickname) {
		foreach (BasicPlayer player in Players) {
			if (player.Nickname == nickname) {
				return player.InGame;
			}
		}
		return false;
	}
	public void SetPlayerInGame (string nickname, bool inGame) {
		foreach (BasicPlayer player in Players) {
			if (player.Nickname == nickname) {
				//player.InGame = inGame;
			}
		}
	}
}
