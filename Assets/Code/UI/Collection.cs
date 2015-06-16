using UnityEngine;
using System.Collections;

public class Collection : MonoBehaviour {

	private int page = 0;

	public Portrait[] portraits;
	public EnemyManager enemyManager;
	
	void Start () {
		for (int i = 0, length = portraits.Length; i < enemyManager.enemies.Count; i++) {
			portraits[i % length].setPortrait(enemyManager.enemies[i]);
		}

		for (int i = 0; i < portraits.Length; i++) {
			portraits[i].switchPortrait(page);
		}
	}

	void Update () {
	
	}
}
