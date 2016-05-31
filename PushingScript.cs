using UnityEngine;
using System.Collections;

public class PushingScript : MonoBehaviour {

	public Vector3 StartingPosition;
	public Vector3 EndingPosition;
	float timer;

	void Start () {
		EndingPosition.z = -0.02f;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= 0.5f) {
			transform.position = EndingPosition;
			Destroy (GetComponent<PushingScript> ());
		} else {
			transform.position = new Vector3 (
				(EndingPosition.x - transform.position.x) * 0.1f + transform.position.x,
				(EndingPosition.y - transform.position.y) * 0.1f + transform.position.y,
				-0.02f);
		}
	}
}
