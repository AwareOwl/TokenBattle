using UnityEngine;
using System.Collections;

public class DeckEditorScript: MonoBehaviour {


	public int SelectedCardInCollection = -1;
	float CardScale = 1.6f;

	public BasicCardQueue [,] CardQueue = new BasicCardQueue [4, 5];
	public BasicCardInCollection [] CardInCollection = new BasicCardInCollection [30];

	public struct BasicCardQueue {
		public int Number;
		public bool Empty;
		public GameObject Card;
		public GameObject Cover;
	}

	public struct BasicCardInCollection {
		public bool Empty;
		public GameObject Card;
		public GameObject Cover;
	}

	// Use this for initialization
	void Start () {
		GameObject Clone;
		Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
		Clone.transform.parent = transform;
		Clone.transform.localScale = new Vector3 (30, 10, 1);
		Clone.GetComponent<Renderer> ().material.color = new Color (0.6f, 0.6f, 0.6f);
		// Back to menu
		Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
		Clone.AddComponent<ControlScript> ();
		Clone.GetComponent<Renderer> ().material.mainTexture = Resources.Load ("OkButton") as Texture;
		Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
		Clone.transform.parent = transform;
		Clone.transform.localScale = new Vector3 (1f, 1f, 1);
		Clone.transform.localPosition = new Vector3 (0, 0, -0.001f);
		Clone.name = "SaveDeck";
		//
		for (int x = 0; x < 4; x ++) {
			Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
			Clone.transform.parent = transform;
			Clone.transform.localScale = new Vector3 (CardScale * GameData.CardSizeX, 8, 1);
			Clone.transform.localPosition = new Vector3 (-5.4f + (CardScale * GameData.CardSizeX + 0.1f) * x, 0, -0.001f);
			Clone.GetComponent<Renderer> ().material.color = new Color (0.25f, 0.25f, 0.25f);
			for (int y = 0; y < 5; y ++) {
				Clone = GameData.CreateCard ();
				Clone.transform.parent = transform;
				Clone.transform.localPosition = new Vector3 (
					-5.4f + (CardScale * GameData.CardSizeX + 0.1f) * x,
					3.4f - (CardScale * GameData.CardSizeY + 0.1f) * y,
					-0.01f);
				Clone.transform.localScale = new Vector3 (CardScale * GameData.CardSizeX, CardScale * GameData.CardSizeY, 1);
				Clone.AddComponent<ControlScript> ();
				Clone.GetComponent<ControlScript> ().x = x;
				Clone.GetComponent<ControlScript> ().y = y;
				Clone.name = "CardInDeck";
				CardQueue [x, y].Card = Clone;
				Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
				Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
				Clone.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 0.4f, 1);
				Clone.transform.parent = CardQueue [x, y].Card.transform;
				Clone.transform.localScale = new Vector3 (1, 1, 1);
				Clone.transform.localPosition = new Vector3 (0, 0, -0.03f);
				Destroy (Clone.GetComponent<Collider> ());
				Clone.name = "Cover";
				CardQueue [x, y].Cover = Clone;
				CardQueue [x, y].Empty = true;
			}
		}
		int MaxX = 4;
		for (int x = 0; x < MaxX; x++) {
			for (int y = 0; y < 5; y++) {
				Clone = GameData.CreateCard (x+y* MaxX);
				Clone.transform.parent = transform;
				Clone.transform.localPosition = new Vector3 (
					1.5f + (CardScale * GameData.CardSizeX + 0.1f) * x,
					3.4f - (CardScale * GameData.CardSizeY + 0.1f) * y,
					-0.01f);
				Clone.transform.localScale = new Vector3 (CardScale * GameData.CardSizeX, CardScale * GameData.CardSizeY, 1);
				Clone.AddComponent<ControlScript> ();
				Clone.GetComponent<ControlScript> ().CardNumber = x + y * MaxX;
				Clone.name = "CardCollection";
				CardInCollection [x + y * MaxX].Card = Clone;
				Clone = GameObject.CreatePrimitive (PrimitiveType.Quad);
				Clone.GetComponent<Renderer> ().material.shader = Shader.Find ("Sprites/Default");
				Clone.GetComponent<Renderer> ().material.color = new Color (0.4f, 0.4f, 0.4f, 0);
				Clone.transform.parent = CardInCollection [x + y * MaxX].Card.transform;
				Clone.transform.localScale = new Vector3 (1, 1, 1);
				Clone.transform.localPosition = new Vector3 (0, 0, -0.03f);
				Destroy (Clone.GetComponent<Collider> ());
				Clone.name = "Cover";
				CardInCollection [x + y * MaxX].Cover = Clone;
			}
		}
	}
}
