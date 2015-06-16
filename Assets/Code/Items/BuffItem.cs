using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuffItem : MonoBehaviour {
	
	private int id;
	private float startTime;

	public ItemEffect itemEffect;
	public float itemPower;
	public float itemDuration;
	public bool isDebuff;

	public void getItemDebuff() {
		GameObject g = GameObject.FindGameObjectWithTag ("Debuff");
		BuffItem deb = g.GetComponent<BuffItem> ();
		deb.setItem (-1, ItemEffect.Move, 0, 5f);
	}

	public int getItemEffect() {
		switch (id) {
		case 17:		// 隱形效水
			return -1;
		case 18:		// 百變物
			removeEffect(true);
			return Random.Range(0, 99) > itemPower? 1 : 0;
		}
		return -1;
	}

	public void setItem(Item item, Sprite sprite) {
		BuffItem buf = item.GetComponent<BuffItem> ();
		setItem (item.id, buf.itemEffect, buf.itemPower, buf.itemDuration, sprite);
	}

	public void setItem(int id, ItemEffect effect, float power, float duration) {
		Sprite bufSprite = new Sprite();
		switch (id) {
		case -1:
			bufSprite = Resources.Load<Sprite> ("Props/frozen");
			break;
		}
		setItem (id, effect, power, duration, bufSprite);
	}

	public void setItem(int id, ItemEffect effect, float power, float duration, Sprite sprite) {
		if (GetComponent<Image> ().sprite != null)
			removeEffect (true);

		this.id = id;
		itemEffect = effect;
		itemPower = power;
		itemDuration = duration;
		GetComponent<Image> ().sprite = sprite;
		GetComponent<Image> ().color = new Color (1, 1, 1, 1);
		launchEffect ();
	}

	public void startEffect() {
		StartCoroutine (launch ());
	}

	public void stopEffect() {
		StopCoroutine (launch ());
		itemDuration -= Time.time - startTime;
		if (itemDuration < 0)
			itemDuration = 0;
		Debug.Log ("[BuffItem]Remain Time:" + itemDuration);
	}

	void launchEffect() {
		//Debug.Log ("Launch" + Time.time);
		if (itemEffect == ItemEffect.Move) {
			effectOnMove();
		}
		if (itemEffect == ItemEffect.Entity) {
			effectOnEntity();
		}
		if (itemEffect == ItemEffect.GameTime) {
			effectOnGameTime();
		}
		if (itemEffect == ItemEffect.Map) {
			effectOnMap();
		}
		StartCoroutine(launch ());
	}

	IEnumerator launch() {
		startTime = Time.time;
		yield return new WaitForSeconds (itemDuration);
		removeEffect (false);
	}

	private void effectOnMove() {
		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();
		Sprite hand = Resources.Load<Sprite> ("Role/hand");

		switch (id) {
			case 13:		// 潤滑液
			case 19:
				hand = Resources.Load<Sprite> ("Effects/hand_speedup");
				player.addSpeedEffect(itemPower);
				break;
			case 16:		// 迷幻藥
				hand = Resources.Load<Sprite> ("Effects/hand_ecstasy");
				player.setInverse(true);
				break;

			// Item secondary action
			case -1:		// 停止移動
				hand = Resources.Load<Sprite> ("Effects/hand_frozen");
				player.playerStatus = PlayerStatus.Punish;
				player.setActive(false);
				break;
		}

		player.GetComponentInChildren<SpriteRenderer> ().sprite = hand;
	}

	private void effectOnEntity() {
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies enemies = e.GetComponent<SpawnEnemies> ();
		Sprite hand = Resources.Load<Sprite> ("Role/hand");

		switch (id) {
			case 14:		// 時間停止器
			case 20:
				hand = Resources.Load<Sprite> ("Effects/hand_timestop");
				enemies.stopEnemies ();
				break;
		}

		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();
		switch (id) {
			case 15:		// 藍色小藥丸
				hand = Resources.Load<Sprite> ("Effects/hand_littlebluepill");
				player.setInvincible(true);
				break;
			case 17:		// 隱型藥水
				hand = Resources.Load<Sprite> ("Effects/hand_invisibility");
				player.setUseItem(true);
				player.GetComponentInChildren<SpriteRenderer> ().color = new Color(1, 1, 1, 0.75f);
				Physics2D.IgnoreLayerCollision (9, 9);
				break;
			case 18:		// 百變物
				hand = Resources.Load<Sprite> ("Effects/hand_variety");
				player.setUseItem (true);
				break;
		}

		player.GetComponentInChildren<SpriteRenderer> ().sprite = hand;
	}
	
	private void effectOnGameTime() {
		/*
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies player = e.GetComponent<SpawnEnemies> ();
		
		switch (id) {
			case 14:		// Orange
				break;
		}
		*/
	}

	private void effectOnMap() {
		/*
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies player = e.GetComponent<SpawnEnemies> ();
		
		switch (id) {
			case 14:		// Orange
				break;
		}
		*/
	}

	private void removeEffect(bool isForce) {
		if (isForce || Time.time - startTime >= itemDuration) {
			//Debug.Log ("Remove" + Time.time);
			if (itemEffect == ItemEffect.Move) {
				removeFromMove ();
			}
			if (itemEffect == ItemEffect.Entity) {
				removeFromEntity ();
			}
			if (itemEffect == ItemEffect.GameTime) {
				removeFromGameTime ();
			}
			if (itemEffect == ItemEffect.Map) {
				removeFromMap ();
			}
			GetComponent<Image> ().sprite = null;
			GetComponent<Image> ().color = new Color (1, 1, 1, 0);

			GameObject p = GameObject.FindGameObjectWithTag ("Player");
			Player player = p.GetComponent<Player> ();
			player.GetComponentInChildren<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Role/hand");
		}
		StopCoroutine (launch ());
	}

	private void removeFromMove() {
		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();
		
		switch (id) {
			case 13:		// 潤滑液
			case 19:
				player.removeSpeedEffect(itemPower);
				break;
			case 16:		// 迷幻藥
				player.setInverse(false);
				break;

			// Item secondary action
			case -1:		// 停止移動
				player.playerStatus = PlayerStatus.Normal;
				player.setActive(true);
				break;
		}
	}
	
	private void removeFromEntity() {
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies enemies = e.GetComponent<SpawnEnemies> ();
		switch (id) {
			case 14:		// 時間停止器
			case 20:
				enemies.startEnemies ();
				return;
		}

		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();
		switch(id) {
			case 15:		// 藍色小藥丸
				player.setInvincible(false);
				return;
			case 17:		// 隱型藥水
				player.setUseItem(false);
				player.GetComponentInChildren<SpriteRenderer> ().color = new Color(1, 1, 1, 1);
				Physics2D.IgnoreLayerCollision(9, 9, false);
				return;
			case 18:		// 百變物
				player.setUseItem(false);
				return;
		}
	}
	
	private void removeFromGameTime() {
		/*
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies player = e.GetComponent<SpawnEnemies> ();
		
		switch (id) {
			case 14:		// Orange
				break;
		}
		*/
	}
	
	private void removeFromMap() {
		/*
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies player = e.GetComponent<SpawnEnemies> ();
		
		switch (id) {
			case 14:		// Orange
				break;
		}
		*/
	}
}

public enum ItemEffect
{
	Move,
	Entity,
	GameTime,
	Map
}