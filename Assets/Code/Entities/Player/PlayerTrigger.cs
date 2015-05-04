using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour {

	public Player player;
	private Direction collisionDirection;

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("[PlayerTrigger]Trigger Enter");
		collisionDirection = getCollisionDirection(other.GetComponent<Transform> ());
		player.setActive (false);
	}

	void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("[PlayerTrigger]Trigger Exit" + collisionDirection.ToString());
		player.setActive (true);
	}

	void OnTriggerStay2D(Collider2D other) {
		if (collisionDirection == player.direction) {
			player.setActive (false);
		} else {
			player.setActive (true);
		}
	}

	Direction getCollisionDirection(Transform collision) {
		float degree = Mathf.Atan2(player.transform.position.x - collision.position.x, player.transform.position.y - collision.position.y) * Mathf.Rad2Deg;
		if (Mathf.Abs (degree) > 135)
			return Direction.Up;
		if (Mathf.Abs (degree) < 45)
			return Direction.Down;
		if (degree > 0)
			return Direction.Right;
		if (degree < 0)
			return Direction.Left;
		return player.direction;
	}
}
