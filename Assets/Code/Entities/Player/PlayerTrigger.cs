using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour {

	public GameManager gameManager;
	public Player player;
	public BuffItem buf;
	private Direction collisionDirection;

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("[PlayerTrigger]Trigger Enter", other.gameObject);
		if (other.gameObject.CompareTag ("Item")) {
			Transform item = other.GetComponent<Transform> ();
			int index = item.GetComponentInParent<SpawnItems> ().findSpawnItem (item);
			//Debug.Log ("[ItemTrigger]Destroy item " + index.ToString());
			if (index < 0) {
				Debug.Log ("[PlayerTrigger]Invalid item index");
			}
			else {
				player.startGetItemEffect();
				player.pItemManager.addToInventory (item.GetComponent<Item> ().id);
				item.GetComponentInParent<SpawnItems> ().respawnItem (index);
			}
		}
		else if (other.gameObject.CompareTag ("Enemy")) {
			Transform enemyTransform = other.GetComponent<Transform> ();
			int index = enemyTransform.GetComponentInParent<SpawnEnemies> ().findSpawnIndex (enemyTransform);
			//Debug.Log ("[EnemyTrigger]Destroy enemy " + index.ToString());
			if (index < 0) {
				Debug.Log ("[PlayerTrigger]Invalid enemy index");
			}
			else {
				//Debug.Log ("[PlayerTrigger]Enemy hit");
				Enemy enemy = enemyTransform.GetComponent<Enemy> ();
				PlayerStatus ps = player.getPlayerStatus();

				if(ps == PlayerStatus.Special) {
					gameManager.relentGoddess (enemy, index);
				}
				else if(ps == PlayerStatus.UseItem) {
					int effect = buf.getItemEffect();
					if(effect == 0)
						gameManager.relentGoddess (enemy, index);
					else if(effect > 0)
						buf.getItemDebuff();
					else {
						if(player.pItemManager.findInInventory(enemy.enemyItem.id) >= 0) {
							gameManager.flirtWithGoddess(enemy, index);
							player.pItemManager.removeFromItemInventory(enemy.enemyItem.id);
						}
					}
				}
				else if(ps == PlayerStatus.Punish) {
					if(enemy.isGay)
						enemyTransform.GetComponentInParent<SpawnEnemies> ().respawnEnemy(index);
				}
				else {
					if(enemy.enemyItem == null) {
						Debug.Log ("[PlayerTrigger]Special enemy");
						if(enemy.isGay) {
							Debug.Log ("PlayerTrigger]Catched by gay");
							gameManager.relentGoddess(enemy, index);
							//enemyTransform.GetComponentInParent<SpawnEnemies> ().respawnEnemy(index);
							if(!player.getHurt()) {
								gameManager.lossScore();
								player.setHurt(true);
							}
						}
					}
					else if(player.pItemManager.findInInventory(enemy.enemyItem.id) < 0) {
						collisionDirection = getCollisionDirection (enemyTransform);
						player.setCollision(collisionDirection, true);
						if(!player.getHurt()) {
							gameManager.lossScore();
							player.setHurt(true);
						}
					}
					else {
						gameManager.flirtWithGoddess(enemy, index);
						player.pItemManager.removeFromItemInventory(enemy.enemyItem.id);
					}
				}
			}
		}
		else {
			collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
			player.setCollision(collisionDirection, true);
			//Debug.Log ("[TileTrigger]Tile direction" + collisionDirection.ToString());
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		PlayerStatus ps = player.getPlayerStatus ();
		switch(ps) {
		case PlayerStatus.Normal:
			if (other.gameObject.CompareTag("Tile") &&
			    Vector3.Distance (player.transform.position, other.GetComponent<Transform> ().position) < 0.5f) {
				int r = Random.Range (0, 8);
				Vector3 pp = new Vector2 (r % 3 - 1, r / 3 - 1);
				if (player.transform.position.x + pp.x < 0f || player.transform.position.x + pp.x > 63f)
					pp.x *= -1;
				if (player.transform.position.y + pp.y < 0f || player.transform.position.y + pp.y > 63f)
					pp.y *= -1;
				player.transform.position += pp;
				player.resetCollision();
			}
			else if(other.gameObject.CompareTag("Enemy")) {
				Enemy enemy = other.GetComponent<Enemy> ();
				int index = enemy.GetComponentInParent<SpawnEnemies> ().findSpawnIndex (enemy.transform);
				collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
				//Debug.Log (collisionDirection);
				player.updateCollision(collisionDirection);

				if(enemy.enemyItem == null) {
					Debug.Log ("[PlayerTrigger]Special enemy");
					if(enemy.isGay) {
						Debug.Log ("PlayerTrigger]Catched by gay");
						enemy.GetComponentInParent<SpawnEnemies> ().respawnEnemy(index);
						if(!player.getHurt()) {
							gameManager.lossScore();
							player.setHurt(true);
						}
					}
				}
				else if(player.pItemManager.findInInventory(enemy.enemyItem.id) < 0) {
					collisionDirection = getCollisionDirection (enemy.transform);
					player.setCollision(collisionDirection, true);
					if(!player.getHurt()) {
						gameManager.lossScore();
						player.setHurt(true);
					}
				}
				else {
					gameManager.flirtWithGoddess(enemy, index);
					player.pItemManager.removeFromItemInventory(enemy.enemyItem.id);
				}
			}
			else {
				collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
				//Debug.Log (collisionDirection);
				player.updateCollision(collisionDirection);
			}
			break;
		case PlayerStatus.UseItem:
			if (other.gameObject.CompareTag("Tile") &&
			    Vector3.Distance (player.transform.position, other.GetComponent<Transform> ().position) < 1f) {
				int r = Random.Range (0, 8);
				Vector3 pp = new Vector2 (r % 3 - 1, r / 3 - 1);
				if (player.transform.position.x + pp.x < 0f || player.transform.position.x + pp.x > 63f)
					pp.x *= -1;
				if (player.transform.position.y + pp.y < 0f || player.transform.position.y + pp.y > 63f)
					pp.y *= -1;
				player.transform.position += pp;
			}
			else if(other.gameObject.CompareTag("Enemy")) {
				int effect = buf.getItemEffect();
				if(effect > 0) {
					collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
					//Debug.Log (collisionDirection);
					player.updateCollision(collisionDirection);
				}
			}
			else {
				collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
				//Debug.Log (collisionDirection);
				player.updateCollision(collisionDirection);
			}
			break;
		case PlayerStatus.Special:
			if(player.transform.position.x < 0)
				player.transform.position = new Vector2(player.transform.position.x + 63, player.transform.position.y);
			if(player.transform.position.x > 63)
				player.transform.position = new Vector2(player.transform.position.x - 63, player.transform.position.y);
			if(player.transform.position.y < 0)
				player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 63);
			if(player.transform.position.y > 63)
				player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 63);
			break;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		PlayerStatus ps = player.getPlayerStatus ();
		switch(ps) {
			case PlayerStatus.Normal:
			case PlayerStatus.UseItem:
				collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
				//Debug.Log ("[PlayerTrigger]Exit " + collisionDirection);
				player.setCollision(collisionDirection, false);
				break;
		}
	}

	Direction getCollisionDirection(Transform collision) {
		float degree = Mathf.Atan2(player.transform.position.x - collision.position.x, player.transform.position.y - collision.position.y) * Mathf.Rad2Deg;
		//Debug.Log ("[PlayerTrigger]Degree " + degree.ToString());
		if (Mathf.Abs (degree) > 135)
			return Direction.Up;
		if (Mathf.Abs (degree) < 45)
			return Direction.Down;
		if (degree > 0)
			return Direction.Left;
		if (degree < 0)
			return Direction.Right;
		return player.direction;
	}
}
