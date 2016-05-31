using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	public bool AutoDestroy;
	float Timer;
	float Scale = 0.5f;

	void Start () {
		GetComponent<Renderer> ().material.color = new Color (1, 1, 0.25f, 0);
	}

	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if (Timer < Scale) {
			GetComponent<Renderer> ().material.color = new Color (1, 1, 0, 0.75f * Timer / Scale);
		} else if (AutoDestroy) {
			if (Timer < 2 * Scale) {
				GetComponent<Renderer> ().material.color = new Color (1, 1, 0, 1.5f - 0.75f * Timer / Scale);
			} else {
				Destroy (gameObject);
			}
		}
	}
}
