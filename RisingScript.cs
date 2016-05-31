using UnityEngine;
using System.Collections;

public class RisingScript : MonoBehaviour {

	void Update () {
		if (GetComponent<PushingScript> () == null) {
			if (transform.localPosition.z > -10) {
				transform.localPosition += Vector3.back * Time.deltaTime * 30f;
			} else {
				Destroy (gameObject);
			}
		}
	}
}
