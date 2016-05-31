using UnityEngine;
using System.Collections;

public class AttachToFieldScript : MonoBehaviour {

	public int x;
	public int y;

	public void Initialize (int x2, int y2) {
		x = x2;
		y = y2;
	}


	// Use this for initialization
	void Start () {
		CScript CScript = GameObject.Find ("CObject").GetComponent<CScript> ();
		CScript.Token [x, y] = gameObject;
		transform.parent = CScript.Field [x, y].transform;
		transform.localPosition = new Vector3 (0, 0, -7.5f);
		transform.localScale = new Vector3 (0.9f, 0.9f, 1);
		gameObject.AddComponent<FallingScript> ();
	}
}
