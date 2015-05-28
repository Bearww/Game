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
				player.pItemManager.addToItemInventory (item.GetComponent<Item> ().id);
				item.GetComponentInParent<SpawnItems> ().respawnItem (index);
				gameManager.extraScore (5);
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
				Debug.Log ("[PlayerTrigger]Enemy hit");
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
				}
				else {
					if(player.pItemManager.findInInventory(enemy.enemyItem.id) < 0) {
						collisionDirection = getCollisionDirection (enemyTransform);
						player.setActive (false);
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
			player.setActive (false);
			//Debug.Log ("[TileTrigger]Tile direction" + collisionDirection.ToString());
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		PlayerStatus ps = player.getPlayerStatus ();
		if(ps == PlayerStatus.Normal) {
			if (Vector3.Distance (player.transform.position, other.GetComponent<Transform> ().position) < 0.5f) {
				int r = (int)Random.Range (0f, 8.9f);
				Vector3 pp = new Vector2 (r % 3 - 1, r / 3 - 1);
				if (player.transform.position.x + pp.x < 0f || player.transform.position.x + pp.x > 63f)
					pp.x *= -1;
				if (player.transform.position.y + pp.y < 0f || player.transform.position.y + pp.y > 63f)
					pp.y *= -1;
				player.transform.position += pp;
				player.setActive (false);
			}
			else {
				collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
				if (player.direction == collisionDirection) {
					player.tryActive ();
					Vector3 tryDir = new Vector3 ();
					if (player.direction == Direction.Up)
						tryDir = Vector3.up;
					if (player.direction == Direction.Down)
						tryDir = Vector3.down;
					if (player.direction == Direction.Left)
						tryDir = Vector3.left;
					if (player.direction == Direction.Right)
						tryDir = Vector3.right;
					tryDir *= 0.001f;
					player.transform.position -= tryDir;
				} else if (other.gameObject.CompareTag ("Enemy") != true) {
					player.setActive (true);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		PlayerStatus ps = player.getPlayerStatus ();
		if (ps == PlayerStatus.Normal) {
			if (other.gameObject.CompareTag ("Enemy")) {
				//Debug.Log ("[EnemyTrigger]Player exit");
				player.setActive (true);
			}
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
