using UnityEngine;
using System.Collections;

public class EnemyTrigger : MonoBehaviour {

	public Player player;
	public Animation playerAni;
	public GameManager gameManager;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			Enemy enemy = other.GetComponent<Enemy> ();
			if(enemy.isGay) {
				Debug.Log ("[EnemyTrigger]Chase Player");

				PlayerStatus ps = player.getPlayerStatus();
				switch(ps) {
				case PlayerStatus.UseItem:
					GameObject obj = GameObject.FindGameObjectWithTag("Buff");
					int effect = obj.GetComponent<BuffItem> ().getItemEffect();
					if(effect >= 0) {
						playerAni.GetComponent<SpriteRenderer> ().color = new Color(1, 1, 1, 1);
						playerAni.Play();
						enemy.findPath (player.transform.position);
						enemy.setChase(true);
					}
					break;
				case PlayerStatus.Normal:
					playerAni.GetComponent<SpriteRenderer> ().color = new Color(1, 1, 1, 1);
					playerAni.Play();
					enemy.findPath (player.transform.position);
					enemy.setChase(true);
					break;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			Enemy enemy = other.GetComponent<Enemy> ();
			if(enemy.isGay) {
				Debug.Log ("[EnemyTrigger]Not Chase Player");
				enemy.setChase(false);
			}
		}
	}
}
