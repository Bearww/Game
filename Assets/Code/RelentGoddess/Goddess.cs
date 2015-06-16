using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Goddess : MonoBehaviour {

	private int index;

	private int favorability;

	private float bonus;

	private float sec;

	private float interval;

	public SpriteRenderer goddess;

	public Transform [] hearts;

	public Text timeText;
	public Text heartText;

	public Sprite heart_empty;
	public Sprite heart_red;

	public GameManager gameManager;

	public int basicHearts;
	public int hardHearts;

	public void backToGame() {
		Debug.Log (string.Format("[Goddess]GetHearts:{0} AllHearts:{1}" , favorability, (int)(20f / interval)));
		gameManager.goddessFavorability(bonus, favorability, (int)(20f / interval));
	}

	public void startFlirt(Sprite goddessSprite, float heartInterval) {
		GetComponentInChildren<SpriteRenderer> ().sprite = goddessSprite;
		interval = heartInterval;
		initGoddess ();
	}

	void countDown() {
		if (sec > 0) {
			timeText.text = string.Format ("{0:00}", --sec);
		} else {
			clearHeart();
			CancelInvoke ("showHeart");
			CancelInvoke ("countDown");

			float favor = (float)(favorability / (20f / interval)) * 100;
			bonus = 1f;
			if(favor >= hardHearts) {
				bonus += (favor - hardHearts + 1) * 0.01f;
				favor -= (favor - hardHearts + 1);
				GetComponentInChildren<Animation> ().Play ("push-hard", mode:PlayMode.StopSameLayer);
				GetComponent<AudioSource> ().Play();
			}
			else if(favor >= basicHearts) {
				bonus += (favor - basicHearts) * 0.005f;
				GetComponentInChildren<Animation> ().Play ("push-push", mode:PlayMode.StopSameLayer);
				GetComponent<AudioSource> ().Play();
			}
			else {
				bonus = 0f;
				GetComponentInChildren<Animation> ().Play ("push", mode:PlayMode.StopSameLayer);
			}
		}
	}

	void initGoddess() {
		index = 0;
		favorability = 0;
		sec = 20;
		goddess.transform.position = transform.position;
		goddess.transform.rotation = Quaternion.identity;
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
		index = Random.Range (0, hearts.Length);
		hearts [index].GetComponentInChildren<SpriteRenderer> ().sprite = heart_red;
	}

	void clearHeart() {
		for (int i = 0; i < hearts.Length; i++) {
			hearts[i].GetComponentInChildren<SpriteRenderer> ().sprite = null;
		}
	}

	public void addHeart() {
		heartText.text = string.Format("{0}", ++favorability);
	}
}
