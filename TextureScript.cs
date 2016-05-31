using UnityEngine;
using System.Collections;
using System.IO;

public class TextureScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Generate ();
	}

	void Generate () {
		int n = 512;
		Texture2D tex = new Texture2D (n, n, TextureFormat.ARGB32, false);
		int [,] t = new int [5, 5];
		t [1, 1] = 1;
		t [3, 3] = 1;
		t [1, 3] = 1;
		t [3, 1] = 1;
		t [1, 2] = 1;
		t [3, 2] = 1;
		t [2, 3] = 1;
		t [2, 1] = 1;
		for (int x = 1; x < n; x++) {
			for (int y = 1; y < n; y++) {
				int tx = (x - 2) / 102;
				int ty = (y - 2) / 102;
				if (tx == 2 && ty == 2) {
					if (y - ty * 102 > x - tx * 102) {
						tex.SetPixel (x, y, new Color (0.3f, 0.3f, 0.3f, 1));
					} else {
						tex.SetPixel (x, y, new Color (0.25f, 0.25f, 0.25f, 1));
					}

				} else if (t [tx, ty] == 1) {
					if (y - ty * 102 > x - tx * 102) {
						tex.SetPixel (x, y, new Color (0.85f, 0.85f, 0.2f, 1));
					} else {
						tex.SetPixel (x, y, new Color (0.825f, 0.775f, 0.2f, 1));
					}
				} else {
					if (y - ty * 102 > x - tx * 102) {
						tex.SetPixel (x, y, new Color (0.8f, 0.8f, 0.8f, 1));
					} else {
						tex.SetPixel (x, y, new Color (0.75f, 0.75f, 0.75f, 1));
					}
				}
			}
		}
		tex.Apply ();
		GetComponent<Renderer> ().material.mainTexture = tex;
		byte [] bytes = tex.EncodeToPNG ();
		File.WriteAllBytes (Application.dataPath + "/../Texture07.png", bytes);
	}
	
	// Update is called once per frame
	void Update () {

		for (int x = 0; x < 1; x++) {
		}
	}
}
