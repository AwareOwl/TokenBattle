using UnityEngine;
using System.Collections;

public class CScript : MonoBehaviour {

	// Skrypt przechowujące dane klienta

	public int PNumber;

	public int ActiveQueue;
	public int MouseOverFieldX;
	public int MouseOverFieldY;

	public int NumberOfPlayers;

	// Gameobjects
	GameObject Center;
	public GameObject [,] Field;
	public GameObject TokenRange;
	public GameObject [,] AbilityRange;
	public GameObject [,] Token;
	public GameObject [] PlayerScoreText = new GameObject [2];

	// Materials
	Material FieldMaterial;
	
	public struct BasicCardQueue {
		public int CardNumber;
		public bool Zoomed;
		public Vector3 Position;
		public GameObject Background;
		public GameObject TokenType;
		public GameObject TokenValue;
		public GameObject AbilityType;
		public GameObject AbilityArea;
	}

	public BasicCardQueue [] CardQueue = new BasicCardQueue [4];
	
	void LoadMapSettings () {
		Field = new GameObject [GameData.MapSizeX, GameData.MapSizeY];
		AbilityRange = new GameObject [GameData.MapSizeX, GameData.MapSizeY];
		Token = new GameObject [GameData.MapSizeX, GameData.MapSizeY];
	}
	
	public void GenerateBoard () {

		Center = new GameObject ();
		Center.transform.localPosition = new Vector3 (0, 0, 0);
		Center.transform.parent = transform;
		Center.name = "Center";

		GameObject Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Clone.transform.parent = Center.transform;
		Clone.transform.localPosition = new Vector3 (0, 0, 0);
		Clone.transform.localScale = new Vector3 (20, 10, 1);
		for (int x = 0; x < GameData.MapSizeX; x++) {
			for (int y = 0; y < GameData.MapSizeY; y++) {
				Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
				Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
				Clone.transform.parent = Center.transform;
				Clone.transform.localPosition = new Vector3 (
					-2.5f + GameData.DiestanceBetweenFields * x, 
					-1.5f + GameData.DiestanceBetweenFields * y, 
					-0.01f);
				Clone.transform.localScale = new Vector3 (0.9f, 0.9f, 1);
				Clone.GetComponent<Renderer> ().material = FieldMaterial;
				Clone.AddComponent<ControlScript> ();
				Clone.GetComponent<ControlScript> ().x = x;
				Clone.GetComponent<ControlScript> ().y = y;
				Clone.name = "Field" + x.ToString () + y.ToString ();
				Clone.tag = "Field";
				Field [x, y] = Clone;
			}
		}
		for (int x = 0; x < 4; x++) {
			Clone = GameData.CreateCard ();
			Clone.transform.parent = Center.transform;
			CardQueue [x].Position = new Vector3 (-2.3f + x * 1.525f, -3.4f, -0.01f);
			Clone.transform.localPosition = CardQueue [x].Position;
			Clone.transform.localScale = new Vector3 (1.35f, 1.8f, 1);
			Clone.AddComponent<ControlScript> ();
			Clone.GetComponent<ControlScript> ().CardNumber = x;
			Clone.name = "Card";
			CardQueue [x].Background = Clone;
		}
		for (int x = 0; x < NumberOfPlayers; x++) {
			Clone = Instantiate (Resources.Load ("PreText")) as GameObject;
			Clone.transform.parent = Center.transform;
			Clone.transform.localPosition = new Vector3 (3.5f, 4.05f - 1.5f * x, -0.01f);
			Clone.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
			Clone.GetComponent<TextMesh> ().text = "";
			Clone.GetComponent<TextMesh> ().anchor = TextAnchor.UpperLeft;
			Clone.GetComponent<TextMesh> ().alignment = TextAlignment.Left;
			PlayerScoreText [x] = Clone;
		}
	}
	
	// Use this for initialization
	void Awake () {
		name = "CObject";
		LoadMapSettings ();
		NumberOfPlayers = 2;
		FieldMaterial = new Material (Shader.Find ("Standard"));
		FieldMaterial.color = new Color (0.3f, 0.3f, 0.3f, 1.0f);
		GenerateBoard ();
		Time.timeScale = 1f;
	}

	public void ZoomCard (int number) {
		if (CardQueue [number].Zoomed == false) {
			CardQueue [number].Background.transform.parent = GameObject.Find ("Main Camera").transform;
			CardQueue [number].Background.transform.localEulerAngles = new Vector3 (0, 0, 0);
			CardQueue [number].Background.transform.localPosition = new Vector3 (0, 0, 1.7f);
			CardQueue [number].Zoomed = true;
		} else {
			CardQueue [number].Background.transform.parent = Center.transform;
			CardQueue [number].Background.transform.localEulerAngles = new Vector3 (0, 0, 0);
			CardQueue [number].Background.transform.localPosition = CardQueue [number].Position;
			CardQueue [number].Zoomed = false;
		}
	}
}
