using UnityEngine;
using System.Collections;

public class EvadingScript : MonoBehaviour {

	public bool AutoDestroy;
	float Timer;
    float TimeScale = 0.5f;

	void Start () {
		transform.localPosition = new Vector3 (0, 0, 1);
	}

	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if (Timer < TimeScale) {
			transform.localPosition = new Vector3 (0, 0, 2 * (TimeScale - Timer));
		} else if (AutoDestroy) {
			if (Timer < 2 * TimeScale) {
				transform.localPosition = new Vector3 (0, 0, 2 * (Timer - TimeScale));
			} else {
				Destroy (gameObject);
			}
		} else {
			transform.localPosition = new Vector3 (0, 0, 0);
			Destroy (GetComponent<EvadingScript> ());
		}
	}
}
