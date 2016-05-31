using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ControlScript : MonoBehaviour {

	public int x;
	public int y;
	public int CardNumber;
	public CScript CScript;
	public PlayerScript PScript;
	public DeckEditorScript EScript;

	void OnMouseEnter () {
		if (name == "Card") {
			transform.localScale *= 1.1f;
		}
		if (tag == "Field") {
			CScript.MouseOverFieldX = x;
			CScript.MouseOverFieldY = y;
			PScript.ShowRange ();
		}
	}
	void OnMouseExit () {
		if (name == "Card") {
			transform.localScale /= 1.1f;

		}
		if (tag == "Field") {
			CScript.MouseOverFieldX = -1;
			CScript.MouseOverFieldY = -1;
			PScript.HideRange ();
		}
	}

	void OnMouseOver () {
		if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject ()) {
			if (tag == "Field") {
				StartCoroutine (PScript.UseToken (x, y));
				CScript.MouseOverFieldX = -1;
				CScript.MouseOverFieldY = -1;
				PScript.HideRange ();
			} else if (name == "Card") {
				if (CScript.ActiveQueue != CardNumber) {
					CScript.ActiveQueue = CardNumber;
					DestroySelection ();
					CreateSelection ();
				} else {
					//CScript.ZoomCard (CardNumber);
				}
			} else if (name == "MessageButton") {
				Destroy (gameObject);
			} else if (name == "CardCollection") {
				EScript.SelectedCardInCollection = CardNumber;
				DestroySelection ();
				CreateSelection ();
			} else if (name == "CardInDeck") {
				PScript.PickCardToSet (x, y);
				EScript.SelectedCardInCollection = -1;
				DestroySelection ();
			} else if (name == "SaveDeck") {
				PScript.CmdSaveHandset (PScript.HandsetToString ());
			}
		}
	}

	// Use this for initialization
	void Awake () {
		if (GameObject.Find ("CObject") != null) {
			CScript = GameObject.Find ("CObject").GetComponent<CScript> ();
		}
		if (GameObject.Find ("MyPlayer") != null) {
			PScript = GameObject.Find ("MyPlayer").GetComponent<PlayerScript> ();
		}
		if (GameObject.Find ("EObject") != null) {
			EScript = GameObject.Find ("EObject").GetComponent<DeckEditorScript> ();
		}
	}

	void Start () {
		if (name == "Card" && CardNumber == 0) {
			CreateSelection ();
		}
	}

	void CreateSelection () {
		PScript.Selection = Instantiate (Resources.Load ("PreSelection")) as GameObject;
		PScript.Selection.transform.position = transform.position;
		PScript.Selection.transform.localScale = transform.lossyScale;
		PScript.Selection.transform.parent = transform;
	}

	void DestroySelection () {
		if (PScript.Selection != null) {
			Destroy (PScript.Selection);
		}
	}
}
