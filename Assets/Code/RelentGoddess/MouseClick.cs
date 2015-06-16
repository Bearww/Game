using UnityEngine;
using System.Collections;

public class MouseClick : MonoBehaviour {

	public Goddess goddess;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		if (GetComponentInChildren<SpriteRenderer> ().sprite == goddess.heart_red) {
			//Debug.Log ("[Goddess]Mouse down");
			GetComponentInChildren<SpriteRenderer> ().sprite = goddess.heart_empty;
			goddess.addHeart();
		}
	}
}
