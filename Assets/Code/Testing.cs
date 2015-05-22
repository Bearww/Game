using UnityEngine;
using System.Collections;

public class Testing : MonoBehaviour {

	public GameManager game;
	public SpawnEnemies enemy;
	public SpawnItems item;

	// Add score to player
	public bool addScore = false;
	public int score = 0;

	// Destroy enemy i
	public bool destroyEnemy = false;
	public int destroyEnemyIndex = 0;

	// Destroy all enemy
	public bool destroyAllEnemy = false;

	// Destroy item
	public bool destroyItem = false;
	public int destroyItemIndex = 0;

	// Destroy all item
	public bool destroyAllItem = false;

	void Start () {
	
	}

	void Update () {
		if (addScore) {
			game.extraScore(score);
			addScore = false;
		}
		if (destroyEnemy) {
			enemy.respawnEnemy(destroyEnemyIndex);
			destroyEnemy = false;
		}
		if (destroyAllEnemy) {
			for(int i = 0; i < enemy.spawnPoints.Count; i++)
				enemy.respawnEnemy(i);
			destroyAllEnemy = false;
		}
		if (destroyItem) {
			item.respawnItem(destroyItemIndex);
			destroyItem = false;
		}
		if (destroyAllItem) {
			for(int i = 0; i < item.spawnPoints.Count; i++)
				item.respawnItem(i);
			destroyAllItem = false;
		}
	}
}
