using UnityEngine;
using System.Collections;

public class CoverScript : MonoBehaviour {

	public bool Enabled;
	public float timer;
	float MaxTime = 0.25f;
	float Col = 0.4f;
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer / MaxTime < 1) {
			if (Enabled) {
				GetComponent<Renderer> ().material.color = new Color (Col, Col, Col, timer / MaxTime);
			} else {
				GetComponent<Renderer> ().material.color = new Color (Col, Col, Col, 1 - timer / MaxTime);
			}
		} else {
			if (Enabled) {
				GetComponent<Renderer> ().material.color = new Color (Col, Col, Col, 1);
			} else {
				GetComponent<Renderer> ().material.color = new Color (Col, Col, Col, 0);
			}
			Destroy (GetComponent<CoverScript> ());
		}
		
	}
}
