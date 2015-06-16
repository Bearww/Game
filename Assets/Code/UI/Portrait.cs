using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portrait : MonoBehaviour {
	
	private List<int> levels = new List<int> ();
	private List<Sprite> sprites = new List<Sprite> ();
	private List<Transform> portraits = new List<Transform> ();
	private int currentPortrait = 0;
	private float clickTime;
	private bool isEnlarge = false;

	public SpriteRenderer basicHeart;
	public SpriteRenderer normalHeart;
	public SpriteRenderer hardHeart;
	public SpriteRenderer goddess;

	void Update() {
		if (isEnlarge && Input.GetMouseButtonDown (0)) {
			isEnlarge = false;
			goddess.color = new Color(1, 1, 1, 0);
		}
	}

	void OnMouseDown() {
		if (!isEnlarge)
			clickTime = Time.time;
		else {
			isEnlarge = false;
			goddess.color = new Color(1, 1, 1, 0);
			clickTime = -1f;
		}
	}

	void OnMouseDrag() {
		if (!isEnlarge && clickTime > 0f && Time.time - clickTime >= 0.5f) {
			if(portraits[currentPortrait].GetComponentInChildren<SpriteRenderer> ().color.r != 0) {
				goddess.sprite = sprites[currentPortrait];
				goddess.color = new Color(1, 1, 1, 1);
				isEnlarge = true;
			}
		}
	}

	public void switchPortrait(int page) {
		if (page < levels.Count) {
			portraits[currentPortrait].GetComponentInChildren<SpriteRenderer> ().color = new Color(1, 1, 1, 0);
			basicHeart.color = new Color(0.6f, 0.6f, 0.6f, 1);
			normalHeart.color = new Color(0.6f, 0.6f, 0.6f, 1);
			hardHeart.color = new Color(0.6f, 0.6f, 0.6f, 1);

			currentPortrait = page;
			if(levels[currentPortrait] < 0)
				portraits[currentPortrait].GetComponentInChildren<SpriteRenderer> ().color = new Color(0, 0, 0, 1);
			else if(levels[currentPortrait] == 0)
				portraits[currentPortrait].GetComponentInChildren<SpriteRenderer> ().color = new Color(0.6f, 0.6f, 0.6f, 1);
			else {
				portraits[currentPortrait].GetComponentInChildren<SpriteRenderer> ().color = new Color(1, 1, 1, 1);
				if(levels[currentPortrait] > 0)
					basicHeart.color = new Color(1, 1, 1, 1);
				if(levels[currentPortrait] > 1)
					normalHeart.color = new Color(1, 1, 1, 1);
				if(levels[currentPortrait] > 2)
					hardHeart.color = new Color(1, 1, 1, 1);
			}
		}
	}

	public void setPortrait(Enemies goddess) {
		Transform portrait = Instantiate (goddess.enemyTransform, transform.position, Quaternion.identity) as Transform;
		portrait.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		portrait.GetComponent<CircleCollider2D> ().enabled = false;
		portrait.SetParent (transform);
		portraits.Add (portrait);
		sprites.Add (goddess.enemySprite);
		levels.Add (goddess.enemyLevel);
	}
}
