﻿using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour {

	public GameManager gameManager;
	public Player player;
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
				if(player.pItemManager.findInInventory(enemy.enemyItem.id) < 0) {
					collisionDirection = getCollisionDirection (enemyTransform);
					player.setActive (false);
					//enemy.setActive(player.direction);
				}
				else {
					gameManager.flirtWithGoddess(enemy, index);
					player.pItemManager.removeFromItemInventory(enemy.enemyItem.id);
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
		collisionDirection = getCollisionDirection (other.GetComponent<Transform> ());
		if (player.direction == collisionDirection) {
			player.setActive(false);
		} else {
			player.setActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("[PlayerTrigger]Trigger Exit");
		if(other.gameObject.CompareTag("Enemy")) {
			//Debug.Log ("[EnemyTrigger]Player exit");
			player.setActive(true);
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

	IEnumerator wait() {
		yield return new WaitForSeconds (21);
	}
}
