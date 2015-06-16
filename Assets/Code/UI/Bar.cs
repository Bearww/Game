using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {

	private float currentScale = 0;

	public bool isLeftToRight;
	public Vector3 barPosition;
	public float barWidth;

	public void changeBar(float percentValue) {
		currentScale += percentValue;
		if (currentScale > 100)
			currentScale = 100;
		if (currentScale < 0)
			currentScale = 0;
		setBar (currentScale);
	}

	public void setBar(float percentValue) {
		if(!isLeftToRight)
			transform.localPosition = 
				new Vector3 (barPosition.x - barWidth * (1 - percentValue / 100), barPosition.y);
		transform.localScale = new Vector3 (percentValue / 100, 1);
	}

	void setShooter() {
		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		p.GetComponent<Player> ().setShooterUsable(true);
	}
}
