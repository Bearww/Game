using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuffItem : MonoBehaviour {
	
	private int id;

	public ItemEffect itemEffect;
	public float itemPower;
	public float itemDuration;
	public bool isDebuff;

	public void getItemDebuff() {
		GameObject g = GameObject.FindGameObjectWithTag ("Debuff");
		BuffItem deb = g.GetComponent<BuffItem> ();
		deb.setItem (20, ItemEffect.Move, 0, 5f, GetComponent<Image> ().sprite);
	}

	public int getItemEffect() {
		switch (id) {
		case 17: return -1;
		case 19: return Random.Range(1, 100) > itemPower? 1 : 0;
		}
		return -1;
	}

	public void setItem(Item item, Sprite sprite) {
		if(GetComponent<Image> ().sprite != null)
			removeEffect ();
		
		id = item.id;
		itemEffect = item.GetComponent<BuffItem> ().itemEffect;
		itemPower = item.GetComponent<BuffItem> ().itemPower;
		itemDuration = item.GetComponent<BuffItem> ().itemDuration;
		GetComponent<Image> ().sprite = sprite;
		GetComponent<Image> ().color = new Color (1, 1, 1, 1);
		StartCoroutine(launchEffect ());
	}

	public void setItem(int id, ItemEffect effect, float power, float duration, Sprite sprite) {
		if(GetComponent<Image> ().sprite != null)
			removeEffect ();

		this.id = id;
		itemEffect = effect;
		itemPower = power;
		itemDuration = duration;
		GetComponent<Image> ().sprite = sprite;
		GetComponent<Image> ().color = new Color (1, 1, 1, 1);
		StartCoroutine(launchEffect ());
	}

	IEnumerator launchEffect() {
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
		yield return new WaitForSeconds (itemDuration);
		removeEffect ();
	}

	private void effectOnMove() {
		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();

		switch (id) {
			case 13:		// Apple
				player.addSpeedEffect(itemPower);
				break;
			case 18:		// Strawberry
				player.setInverse(true);
				break;
			case 20:
				player.setActive(false);
				break;
		}
	}

	private void effectOnEntity() {
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies enemies = e.GetComponent<SpawnEnemies> ();
		switch (id) {
			case 14:		// Orange
				enemies.stopEnemies ();
				return;
		}

		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();
		switch (id) {
			case 15:		// Bell
				player.setInvincible(true);
				return;
			case 17:		// Cantaloupe
				player.setUseItem(true);
				player.GetComponentInChildren<SpriteRenderer> ().color = new Color(1, 1, 1, 0.75f);
				Physics2D.IgnoreLayerCollision (9, 9);
				return;
			case 19:
				player.setUseItem (true);
				return;
		}
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

	private void removeEffect() {
		if (itemEffect == ItemEffect.Move) {
			removeFromMove();
		}
		if (itemEffect == ItemEffect.Entity) {
			removeFromEntity();
		}
		if (itemEffect == ItemEffect.GameTime) {
			removeFromGameTime();
		}
		if (itemEffect == ItemEffect.Map) {
			removeFromMap();
		}
		GetComponent<Image> ().sprite = null;
		GetComponent<Image> ().color = new Color (1, 1, 1, 0);
	}

	private void removeFromMove() {
		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();
		
		switch (id) {
			case 13:		// Apple
				player.removeSpeedEffect(itemPower);
				break;
			case 18:		// Strawberry
				player.setInverse(false);
				break;
			case 20:
				player.setActive(true);
				break;
		}
	}
	
	private void removeFromEntity() {
		GameObject e = GameObject.FindGameObjectWithTag ("SpawnEnemy");
		SpawnEnemies enemies = e.GetComponent<SpawnEnemies> ();
		switch (id) {
			case 14:		// Orange
				enemies.startEnemies ();
				return;
		}

		GameObject p = GameObject.FindGameObjectWithTag ("Player");
		Player player = p.GetComponent<Player> ();
		switch(id) {
			case 15:		// Bell
				player.setInvincible(false);
				return;
			case 17:		// Cantaloupe
				player.setUseItem(false);
				player.GetComponentInChildren<SpriteRenderer> ().color = new Color(1, 1, 1, 1);
				Physics2D.IgnoreLayerCollision(9, 9, false);
				return;
			case 19:
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