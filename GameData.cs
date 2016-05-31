using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	static public int HandSize = 4;
	static public int MapSizeX = 6;
	static public int MapSizeY = 6;
	static public float CardSizeX = 0.75f;
	static public float CardSizeY = 1.0f;
	static public float DiestanceBetweenFields = 1.0f;

	static public float FallingDuration = 0.25f;
	static public float FallingDiestance = 7f;

	static public BasicCard [] Card = new BasicCard [100];
	static public BasicAbility [] Ability = new BasicAbility [20];

	static public Texture2D [] AbilityAreaTexture = new Texture2D [11];
	static public Texture2D [] AbilityTypeTexture = new Texture2D [11];

	public struct AbilityPosition {
		public int x;
		public int y;
	}

	public struct BasicAbility {
		public int AbilitySize;
		public AbilityPosition [] Pos;
	}

	public struct BasicCard {
		public int TokenType;
		public int TokenValue;
		public int AbilityType;
		public int AbilityArea;
	}

	static GameData () {
		LoadAbilityData ();
		LoadCardData ();
		LoadAbilityAreaData ();
		LoadAbilityTypeData ();
	}

	static public void LoadAbilityData () {
		BasicAbility TAbility = new BasicAbility ();
		Ability [0].AbilitySize = 0;
		TAbility.AbilitySize = 2;
		TAbility.Pos = new AbilityPosition [2];
		TAbility.Pos [0].x = -1;
		TAbility.Pos [0].y = 0;
		TAbility.Pos [1].x = 1;
		TAbility.Pos [1].y = 0;
		Ability [1] = TAbility;
		TAbility.AbilitySize = 2;
		TAbility.Pos = new AbilityPosition [2];
		TAbility.Pos [0].x = 0;
		TAbility.Pos [0].y = -1;
		TAbility.Pos [1].x = 0;
		TAbility.Pos [1].y = 1;
		Ability [2] = TAbility;
		TAbility.AbilitySize = 4;
		TAbility.Pos = new AbilityPosition [4];
		TAbility.Pos [0].x = -1;
		TAbility.Pos [0].y = 0;
		TAbility.Pos [1].x = 1;
		TAbility.Pos [1].y = 0;
		TAbility.Pos [2].x = 0;
		TAbility.Pos [2].y = -1;
		TAbility.Pos [3].x = 0;
		TAbility.Pos [3].y = 1;
		Ability [3] = TAbility;
		TAbility.AbilitySize = 2;
		TAbility.Pos = new AbilityPosition [2];
		TAbility.Pos [0].x = -1;
		TAbility.Pos [0].y = -1;
		TAbility.Pos [1].x = 1;
		TAbility.Pos [1].y = 1;
		Ability [4] = TAbility;
		TAbility.AbilitySize = 2;
		TAbility.Pos = new AbilityPosition [2];
		TAbility.Pos [0].x = -1;
		TAbility.Pos [0].y = 1;
		TAbility.Pos [1].x = 1;
		TAbility.Pos [1].y = -1;
		Ability [5] = TAbility;
		TAbility.AbilitySize = 4;
		TAbility.Pos = new AbilityPosition [4];
		TAbility.Pos [0].x = -1;
		TAbility.Pos [0].y = -1;
		TAbility.Pos [1].x = 1;
		TAbility.Pos [1].y = 1;
		TAbility.Pos [2].x = -1;
		TAbility.Pos [2].y = 1;
		TAbility.Pos [3].x = 1;
		TAbility.Pos [3].y = -1;
		Ability [6] = TAbility;
		TAbility.AbilitySize = 8;
		TAbility.Pos = new AbilityPosition [8];
		TAbility.Pos [0].x = -1;
		TAbility.Pos [0].y = 0;
		TAbility.Pos [1].x = 1;
		TAbility.Pos [1].y = 0;
		TAbility.Pos [2].x = 0;
		TAbility.Pos [2].y = -1;
		TAbility.Pos [3].x = 0;
		TAbility.Pos [3].y = 1;
		TAbility.Pos [4].x = -1;
		TAbility.Pos [4].y = -1;
		TAbility.Pos [5].x = 1;
		TAbility.Pos [5].y = 1;
		TAbility.Pos [6].x = -1;
		TAbility.Pos [6].y = 1;
		TAbility.Pos [7].x = 1;
		TAbility.Pos [7].y = -1;
		Ability [7] = TAbility;
		TAbility.AbilitySize = 8;
		TAbility.Pos = new AbilityPosition [8];
		TAbility.Pos [0].x = -2;
		TAbility.Pos [0].y = 0;
		TAbility.Pos [1].x = -1;
		TAbility.Pos [1].y = 0;
		TAbility.Pos [2].x = 1;
		TAbility.Pos [2].y = 0;
		TAbility.Pos [3].x = 2;
		TAbility.Pos [3].y = 0;
		TAbility.Pos [4].x = 0;
		TAbility.Pos [4].y = -2;
		TAbility.Pos [5].x = 0;
		TAbility.Pos [5].y = -1;
		TAbility.Pos [6].x = 0;
		TAbility.Pos [6].y = 1;
		TAbility.Pos [7].x = 0;
		TAbility.Pos [7].y = 2;
		Ability [8] = TAbility;
		TAbility.AbilitySize = 8;
		TAbility.Pos = new AbilityPosition [8];
		TAbility.Pos [0].x = -2;
		TAbility.Pos [0].y = -2;
		TAbility.Pos [1].x = -1;
		TAbility.Pos [1].y = -1;
		TAbility.Pos [2].x = 1;
		TAbility.Pos [2].y = 1;
		TAbility.Pos [3].x = 2;
		TAbility.Pos [3].y = 2;
		TAbility.Pos [4].x = 2;
		TAbility.Pos [4].y = -2;
		TAbility.Pos [5].x = 1;
		TAbility.Pos [5].y = -1;
		TAbility.Pos [6].x = -1;
		TAbility.Pos [6].y = 1;
		TAbility.Pos [7].x = -2;
		TAbility.Pos [7].y = 2;
		Ability [9] = TAbility;

	}

	static public void LoadCardData () {
		BasicCard TCard = new BasicCard ();
		// 1
		TCard.TokenValue = 3;
		TCard.AbilityArea = 1;
		TCard.AbilityType = 1;
		Card [0] = TCard;
		// 2
		TCard.TokenValue = 3;
		TCard.AbilityArea = 2;
		TCard.AbilityType = 1;
		Card [1] = TCard;
		// 3
		TCard.TokenValue = 2;
		TCard.AbilityArea = 3;
		TCard.AbilityType = 1;
		Card [2] = TCard;
		// 4
		TCard.TokenValue = 3;
		TCard.AbilityArea = 4;
		TCard.AbilityType = 1;
		Card [3] = TCard;
		// 5
		TCard.TokenValue = 3;
		TCard.AbilityArea = 5;
		TCard.AbilityType = 1;
		Card [4] = TCard;
		// 6
		TCard.TokenValue = 2;
		TCard.AbilityArea = 6;
		TCard.AbilityType = 1;
		Card [5] = TCard;
		// 7
		TCard.TokenValue = 1;
		TCard.AbilityArea = 7;
		TCard.AbilityType = 1;
		Card [6] = TCard;
		// 1
		TCard.TokenValue = 3;
		TCard.AbilityArea = 1;
		TCard.AbilityType = 2;
		Card [7] = TCard;
		// 2
		TCard.TokenValue = 3;
		TCard.AbilityArea = 2;
		TCard.AbilityType = 2;
		Card [8] = TCard;
		// 3
		TCard.TokenValue = 2;
		TCard.AbilityArea = 3;
		TCard.AbilityType = 2;
		Card [9] = TCard;
		// 4
		TCard.TokenValue = 2;
		TCard.AbilityArea = 4;
		TCard.AbilityType = 2;
		Card [10] = TCard;
		// 5
		TCard.TokenValue = 2;
		TCard.AbilityArea = 5;
		TCard.AbilityType = 2;
		Card [11] = TCard;
		// 6
		TCard.TokenValue = 1;
		TCard.AbilityArea = 6;
		TCard.AbilityType = 2;
		Card [12] = TCard;
		// 17
		TCard.TokenValue = 1;
		TCard.AbilityArea = 7;
		TCard.AbilityType = 2;
		Card [13] = TCard;
		// 1
		TCard.TokenValue = 3;
		TCard.AbilityArea = 1;
		TCard.AbilityType = 3;
		Card [14] = TCard;
		// 2
		TCard.TokenValue = 3;
		TCard.AbilityArea = 2;
		TCard.AbilityType = 3;
		Card [15] = TCard;
		// 3
		TCard.TokenValue = 2;
		TCard.AbilityArea = 3;
		TCard.AbilityType = 3;
		Card [16] = TCard;
		// 4
		TCard.TokenValue = 2;
		TCard.AbilityArea = 4;
		TCard.AbilityType = 3;
		Card [17] = TCard;
		// 5
		TCard.TokenValue = 2;
		TCard.AbilityArea = 5;
		TCard.AbilityType = 3;
		Card [18] = TCard;
		// 6
		TCard.TokenValue = 1;
		TCard.AbilityArea = 6;
		TCard.AbilityType = 3;
		Card [19] = TCard;
		// 17
		TCard.TokenValue = 2;
		TCard.AbilityArea = 7;
		TCard.AbilityType = 3;
		Card [20] = TCard;
	}

	static public void LoadAbilityAreaData () {
		for (int x = 0; x < 8; x++) {
			AbilityAreaTexture [x] = Resources.Load ("Textures/AbilityRange/Texture0" + x.ToString ()) as Texture2D;
		}
	}

	static public void LoadAbilityTypeData () {
		for (int x = 0; x < 4; x++) {
			AbilityTypeTexture [x] = Resources.Load ("Textures/AbilityType/Ability0" + x.ToString ()) as Texture2D;
		}
	}

	static public GameObject ShowMessage (string s) {
		GameObject Clone = Instantiate (Resources.Load ("PreMessage")) as GameObject;
		Clone.transform.parent = GameObject.Find ("Canvas").transform;
		Clone.transform.localPosition = new Vector3 (0, 0, 0);
		GameObject MessageText = Clone.transform.Find ("MessageText").gameObject;
		MessageText.GetComponent <Text> ().text = s;
		return Clone;
	}

	static public GameObject ShowMessage (string s, string option) {
		GameObject Clone = ShowMessage (s);
		Clone.transform.Find("Button").name = option;
		return Clone;
	}

	static public GameObject CreateCard () {
		GameObject Clone;
		Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Clone.GetComponent<Renderer> ().material.color = new Color (0.05f, 0.05f, 0.05f);
		Clone.name = "Card";
		GameObject Clone2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Clone2.transform.parent = Clone.transform;
		Clone2.transform.localScale = new Vector3 (0.9f, 0.9f * CardSizeX, 1);
		Clone2.transform.localPosition = new Vector3 (0, -0.12f, -0.01f);
		Clone2.name = "AbilityArea";
		Destroy (Clone2.GetComponent<Collider> ());
		Clone2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Clone2.transform.localScale = new Vector3 (0.325f, 0.325f * CardSizeX, 1);
		Clone2.transform.parent = Clone.transform;
		Clone2.transform.localPosition = new Vector3 (0.2875f, 0.339f, -0.01f);
		Clone2.name = "AbilityType";
		Destroy (Clone2.GetComponent<Collider> ());
		Clone2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Clone2.GetComponent<Renderer> ().material.color = new Color (0.5f, 0.5f, 0.5f);
		Clone2.transform.localScale = new Vector3 (0.9f, 0.325f * CardSizeX, 1);
		Clone2.transform.parent = Clone.transform;
		Clone2.transform.localPosition = new Vector3 (0, 0.34f, -0.005f);
		Clone2.name = "TextBackground";
		Destroy (Clone2.GetComponent<Collider> ());
		Clone2 = Instantiate (Resources.Load ("PreText")) as GameObject;
		Clone2.GetComponent<TextMesh> ().text = "";
		Clone2.transform.localScale = new Vector3 (0.325f, 0.325f * CardSizeX, 1);
		Clone2.transform.parent = Clone.transform;
		Clone2.transform.localPosition = new Vector3 (-0.3f, 0.35f, -0.02f);
		Clone2.name = "ValueText";
		return Clone;
	}

	static public GameObject CreateCard (int number) {
		GameObject Clone = CreateCard ();
		SetCard (Clone, number);
		return Clone;
	}

	static public void SetCard (GameObject cardObject, int number) {
		SetCardAbilityArea (cardObject, number);
		SetCardAbilityType (cardObject, number);
		SetCardValue (cardObject, number);
	}

	static public void SetCardAbilityArea (GameObject cardObject, int number) {
		cardObject.transform.Find ("AbilityArea").gameObject.GetComponent<Renderer> ().material.mainTexture =
			AbilityAreaTexture [Card [number].AbilityArea];
	}

	static public void SetCardAbilityType (GameObject cardObject, int number) {
		cardObject.transform.Find ("AbilityType").gameObject.GetComponent<Renderer> ().material.mainTexture =
			AbilityTypeTexture [Card [number].AbilityType];
	}

	static public void SetCardValue (GameObject cardObject, int number) {
		cardObject.transform.Find ("ValueText").gameObject.GetComponent<TextMesh> ().text = Card [number].TokenValue.ToString();
	}

}
