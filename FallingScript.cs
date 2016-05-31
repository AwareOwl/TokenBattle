using UnityEngine;
using System.Collections;

public class FallingScript : MonoBehaviour {
	
	void Update () {
		if (transform.localPosition.z < -0.01f) {
			transform.Translate (0, 0, Time.deltaTime * GameData.FallingDiestance / GameData.FallingDuration);
			//transform.localPosition += Vector3.forward * Time.deltaTime * GameData.FallingDiestance / GameData.FallingDuration;
		}
		if (transform.localPosition.z >= -0.01f) {
			transform.localPosition = new Vector3 (0, 0, -0.01f);
			Destroy (GetComponent<FallingScript> ());
		}
	}
}
