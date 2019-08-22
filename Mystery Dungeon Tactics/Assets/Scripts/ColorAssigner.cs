using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAssigner : MonoBehaviour {
	private MaterialPropertyBlock block;
	private Renderer rend;
	
	void Start() {
		block = new MaterialPropertyBlock();
		rend = GetComponent<Renderer>();
		ChangeColor();
	}

	void ChangeColor() {
		// You can look up the property by ID instead of the string to be more efficient.
		block.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

// You can cache a reference to the renderer to avoid searching for it.
		rend.SetPropertyBlock(block);
	}
}
