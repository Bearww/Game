using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour {

	public EnemyManager enemyManager;

	public List<SpawnPoint> spawnPoints;
	public List<EnemyPath> spawnPath;

	void Start () {
		Debug.Log ("[SpawnEnemy]SpawnPoints " + spawnPoints.Count);
		DontDestroyOnLoad (gameObject);
		for (int index = 0; index < spawnPath.Count; index++)
			checkCyclePath (index);
	}

	public void addSpawnPoint(int enemyId, Vector3 enemyPosition) {
		//Debug.Log ("[Spawnenemy]Add enemy " + enemyId.ToString());
		int spawnIndex = spawnPoints.Count;
		int enemyIndex = enemyManager.getEnemyIndex (enemyId);
		if (enemyIndex < 0)
			enemyIndex = 0;
		
		Transform enemy = Instantiate (enemyManager.enemies [enemyIndex].enemyTransform, enemyPosition, Quaternion.identity) as Transform;
		enemy.SetParent(transform);

		if (spawnIndex < spawnPath.Count) {
			enemy.GetComponent<Enemy> ().enemyPath = spawnPath [spawnIndex].enemyPath;
			enemy.GetComponent<Enemy> ().isCycle = spawnPath [spawnIndex].isCyclePath;
		}

		SpawnPoint spawnPoint = new SpawnPoint (enemy, enemyPosition);
		spawnPoints.Add (spawnPoint);
	}
	
	public int findSpawnEnemy(Transform spawnenemy) {
		for (int index = 0; index < spawnPoints.Count; index++) {
			if(spawnPoints[index].spawnTransform == spawnenemy)
				return index;
		}
		return -1;
	}
	
	public void respawnEnemy(int index) {
		Destroy (spawnPoints [index].spawnTransform.gameObject);
		StartCoroutine(waitForRespawn(index));
	}
	
	IEnumerator waitForRespawn(int index) {
		yield return new WaitForSeconds(2);
		Transform enemy = Instantiate (enemyManager.enemies [0].enemyTransform, spawnPoints[index].spawnPosition, Quaternion.identity) as Transform;
		enemy.SetParent(transform);
		if (index < spawnPath.Count) {
			enemy.GetComponent<Enemy> ().enemyPath = spawnPath [index].enemyPath;
			enemy.GetComponent<Enemy> ().isCycle = spawnPath [index].isCyclePath;
		}
		spawnPoints [index].spawnTransform = enemy;
	}

	void checkCyclePath(int index)
	{
		Vector3 pos = new Vector3 (0, 0);
		for (int i = 0; i < spawnPath [index].enemyPath.Count; i++) {
			if (spawnPath [index].enemyPath [i].dir == Direction.Up) {
				pos += Vector3.up;
			}
			if (spawnPath [index].enemyPath [i].dir == Direction.Down) {
				pos += Vector3.down;
			}
			if (spawnPath [index].enemyPath [i].dir == Direction.Left) {
				pos += Vector3.left;
			}
			if (spawnPath [index].enemyPath [i].dir == Direction.Right) {
				pos += Vector3.right;
			}
		}
		
		if(pos.Equals (new Vector3(0, 0)))
			spawnPath [index].isCyclePath = true;
		else
			spawnPath [index].isCyclePath = false;
	}
}

[System.Serializable]
public class EnemyPath {
	public List<Path> enemyPath;
	public bool isCyclePath;
}