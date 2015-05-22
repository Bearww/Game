using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Goddess : MonoBehaviour {

	private int index;

	private int favorability;

	private float sec;

	public float interval;

	public SpriteRenderer goddess;

	public Transform [] hearts;

	public Text timeText;
	public Text heartText;

	public Sprite heart_empty;
	public Sprite heart_red;

	public GameManager gameManager;
	public GameObject [] gameObjs;

	public void startFlirt(Sprite goddessSprite) {
		GetComponentInChildren<SpriteRenderer> ().sprite = goddessSprite;
		initGoddess ();
	}

	void countDown() {
		if (sec > 0) {
			timeText.text = string.Format ("{0:00}", --sec);
		} else {
			CancelInvoke ("showHeart");
			CancelInvoke ("countDown");
			if (gameObjs != null) {
				gameObject.SetActive (false);
				for (int i = 0; i < gameObjs.Length; i++) {
					gameObjs [i].SetActive (true);
				}
			}
			gameManager.goddessFavorability(favorability);
		}
	}

	void initGoddess() {
		index = 0;
		favorability = 0;
		sec = 20;
		timeText.text = string.Format ("{0:00}", sec);
		heartText.text = string.Format ("{0}", favorability);
		for (int i = 0; i < hearts.Length; i++) {
			hearts[i].GetComponentInChildren<SpriteRenderer> ().sprite = heart_empty;
		}
		InvokeRepeating ("countDown", 1f, 1f);
		InvokeRepeating ("showHeart", 1f, interval);
	}

	void showHeart() {
		hearts [index].GetComponentInChildren<SpriteRenderer> ().sprite = heart_empty;
		float r = Random.Range (0f, (float)hearts.Length - 0.01f);
		index = (int)r;
		hearts [index].GetComponentInChildren<SpriteRenderer> ().sprite = heart_red;
	}

	public void addHeart() {
		heartText.text = string.Format("{0}", ++favorability);
	}
}
