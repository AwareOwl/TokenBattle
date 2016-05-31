using UnityEngine;
using System.Collections;

public class SelectionScript : MonoBehaviour {

	GameObject [] Quads = new GameObject [8];
	Color col = new Color (0.3f, 1, 0.3f);
	float Size = 0.2f;

	// Use this for initialization
	void Start () {
		name = "wat";
		for (int x = 0; x < 8; x++) {
			if (x < 4) {
				Quads [x] = Instantiate (Resources.Load ("PreSelectionEdge")) as GameObject;
			} else {
				Quads [x] = Instantiate (Resources.Load ("PreSelectionCorner")) as GameObject;
			}
		}
		Quads [0].transform.localPosition = new Vector3 (
			transform.position.x,
			transform.position.y + transform.lossyScale.y / 2 + Size / 2,
			transform.position.z);
		Quads [1].transform.localPosition = new Vector3 (
			transform.position.x + transform.lossyScale.x / 2 + Size / 2,
			transform.position.y,
			transform.position.z);
		Quads [2].transform.localPosition = new Vector3 (
			transform.position.x,
			transform.position.y - transform.lossyScale.y / 2 - Size / 2,
			transform.position.z);
		Quads [3].transform.localPosition = new Vector3 (
			transform.position.x - transform.lossyScale.x / 2 - Size / 2,
			transform.position.y,
			transform.position.z);
		Quads [4].transform.localPosition = new Vector3 (
			transform.position.x + transform.lossyScale.x / 2 + Size / 2,
			transform.position.y + transform.lossyScale.y / 2 + Size / 2,
			transform.position.z);
		Quads [5].transform.localPosition = new Vector3 (
			transform.position.x + transform.lossyScale.x / 2 + Size / 2,
			transform.position.y - transform.lossyScale.y / 2 - Size / 2,
			transform.position.z);
		Quads [6].transform.localPosition = new Vector3 (
			transform.position.x - transform.lossyScale.x / 2 - Size / 2,
			transform.position.y - transform.lossyScale.y / 2 - Size / 2,
			transform.position.z);
		Quads [7].transform.localPosition = new Vector3 (
			transform.position.x - transform.lossyScale.x / 2 - Size / 2,
			transform.position.y + transform.lossyScale.y / 2 + Size / 2,
			transform.position.z);

		Quads [0].transform.localScale = new Vector3 (
			transform.lossyScale.x,
			Size,
			1);
		Quads [1].transform.localScale = new Vector3 (
			transform.lossyScale.y,
			Size,
			1);
		Quads [2].transform.localScale = new Vector3 (
			transform.lossyScale.x,
			Size,
			1);
		Quads [3].transform.localScale = new Vector3 (
			transform.lossyScale.y,
			Size,
			1);
		for (int x = 4; x < 8; x++) {
			Quads [x].transform.localScale = new Vector3 (
				Size,
				Size,
				1);
		}
		//Quads [0].transform.Rotate (Vector3.back, 0);
		Quads [1].transform.Rotate (Vector3.back, 90);
		Quads [2].transform.Rotate (Vector3.back, 180);
		Quads [3].transform.Rotate (Vector3.back, 270);
		//Quads [4].transform.Rotate (Vector3.back, 0);
		Quads [5].transform.Rotate (Vector3.back, 90);
		Quads [6].transform.Rotate (Vector3.back, 180);
		Quads [7].transform.Rotate (Vector3.back, 270);
		for (int x = 0; x < 8; x++) {
			Quads [x].transform.parent = transform;
			Quads [x].GetComponent<Renderer> ().material.color = col;
		}
	}
}
