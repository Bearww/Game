using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour {

	public Player player;
	private Direction collisionDirection;

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("[PlayerTrigger]Trigger Enter", other.gameObject);
		if (other.gameObject.CompareTag ("Item")) {
			Transform item = other.GetComponent<Transform> ();
			int index = item.GetComponentInParent<SpawnItems> ().findSpawnItem (item);
			//Debug.Log ("[ItemTrigger]Destroy item " + index.ToString());
			if (index < 0) {
				Debug.Log ("[ItemTrigger]Invalid item index");
			}
			else {
				player.pItemManager.addToItemInventory (item.GetComponent<Item> ().id, 1);
				item.GetComponentInParent<SpawnItems> ().respawnItem (index);
				player.scorePoints (5);
			}
		} else if (other.gameObject.CompareTag ("Enemy")) {
			Transform enemy = other.GetComponent<Transform> ();
			int index = enemy.GetComponentInParent<SpawnEnemies> ().findSpawnEnemy (enemy);
			//Debug.Log ("[EnemyTrigger]Destroy enemy " + index.ToString());
			if (index < 0) {
				Debug.Log ("[EnemyTrigger]Invalid enemy index");
			}
			else {
				Item enemyItem = enemy.GetComponent<Enemy> ().enemyItem;
				if(player.pItemManager.findInInventory(enemyItem.id) < 0) {
					collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
					player.setActive (false);
				}
				else {
					enemy.GetComponentInParent<SpawnEnemies> ().respawnEnemy (index);
					player.scorePoints (10);
				}
			}
		}
		else {
			collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
			player.setActive (false);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("[PlayerTrigger]Trigger Exit");
		player.setActive (true);
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Item") == false) {
			if (collisionDirection == player.direction) {
				player.setActive (false);
			} else {
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
