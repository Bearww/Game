using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour {

	private List<StageEnemy> spawnEnemy = new List<StageEnemy> ();
	private List<StageEnemy> spawnSpecial = new List<StageEnemy> ();
	private List<int> spawnAmount = new List<int> ();
	private List<float> spawnRatio = new List<float> ();

	public EnemyManager enemyManager;

	public List<SpawnPoint> spawnPoints;
	public List<EnemyPath> spawnPath;

	void Start () {
		Debug.Log ("[SpawnEnemy]SpawnEnemy " + spawnEnemy.Count);
		Debug.Log ("[SpawnEnemy]SpawnPoints " + spawnPoints.Count);
		Debug.Log ("[SpawnEnemy]SpawnPath " + spawnPath.Count);

		startSpawnEnemies ();
		DontDestroyOnLoad (gameObject);
	}

	public void addSpawnPoint(Vector3 enemyPosition) {
		//Debug.Log ("[Spawnenemy]Add enemy " + enemyId.ToString());
		SpawnPoint spawnPoint = new SpawnPoint (enemyPosition);
		spawnPoints.Add (spawnPoint);
	}

	/*
	public void addSpawnPoint(int enemyId, Vector3 enemyPosition) {
		//Debug.Log ("[Spawnenemy]Add enemy " + enemyId.ToString());
		int enemyIndex = enemyManager.getEnemyIndex (enemyId);
		if (enemyIndex < 0)
			enemyIndex = 0;
		
		Transform enemy = Instantiate (enemyManager.enemies [enemyIndex].enemyTransform, enemyPosition, Quaternion.identity) as Transform;
		enemy.SetParent(transform);

		SpawnPoint spawnPoint = new SpawnPoint (enemy, enemyPosition);
		spawnPoints.Add (spawnPoint);
	}
	*/

	public void addSpawnPath(EnemyPath sp) {
		//Debug.Log ("[SpawnEnemy]SpawnPath Add" + spawnPath.Count);
		spawnPath.Add (sp);
	}

	public void addEnemy(StageEnemy enemy) {
		spawnAmount.Add (0);
		spawnEnemy.Add (enemy);
		spawnRatio.Add (enemy.enemyRatio);
		setEnemyRatio ();
	}

	public void removeEnemy(int index) {
		Debug.Log ("[SpawnEnemy]Revmoe enemy");
		spawnAmount.RemoveAt (spawnSpecial.Count + index);
		spawnEnemy.RemoveAt (index);
		spawnRatio.RemoveAt (index);
		setEnemyRatio ();
	}

	public void removeEnemy(StageEnemy enemy) {
		int index = spawnEnemy.FindIndex ((StageEnemy e) => (enemy.enemy == e.enemy));
		if (index < 0) {
			Debug.Log ("[SpawnEnemy]Remove invalid enemy");
		}
		else {
			removeEnemy (index);
		}
	}

	public void addSpecialEnemy(StageEnemy enemy) {
		spawnAmount.Insert (spawnSpecial.Count, 0);
		spawnRatio.Insert (spawnSpecial.Count, enemy.enemyRatio);
		spawnSpecial.Add (enemy);
		setEnemyRatio ();
	}

	public int findSpawnEnemy(Enemy enemy) {
		for (int index = 0; index < spawnSpecial.Count; index++) {
			if(spawnSpecial[index].getStage() == enemy.getStage())
				return index;
		}

		for (int index = 0; index < spawnEnemy.Count; index++) {
			if(spawnEnemy[index].getStage() == enemy.getStage())
				return spawnSpecial.Count + index;
		}
		return -1;
	}

	public int findSpawnIndex(Transform enemy) {
		for (int index = 0; index < spawnPoints.Count; index++) {
			if(spawnPoints[index].spawnTransform == enemy)
				return index;
		}
		return -1;
	}

	public int getEnemyScore(Enemy enemy) {
		int e = findSpawnEnemy (enemy);
		if (e < 0) {
			Debug.Log ("[SpawnEnemy]Invalid enemy");
		} else if (e < spawnSpecial.Count) {
			return spawnSpecial [e].enemyScore;
		} else {
			e -= spawnSpecial.Count;
			return spawnEnemy [e].enemyScore;
		}
		return 0;
	}
	
	public void respawnEnemy(int index) {
		changeEnemyRatio (index);
		Destroy (spawnPoints [index].spawnTransform.gameObject);
		StartCoroutine(waitForRespawn(index));
	}

	public void stopSpawnEnemy(StageEnemy enemy) {
		int index = spawnEnemy.FindIndex ((StageEnemy e) => (enemy.enemy == e.enemy));
		if (index < 0) {
			Debug.Log ("[SpawnEnemy]Invalid enemy");
		}
		else {
			spawnEnemy[index].enemyRatio = 0;
		}
	}
	
	IEnumerator waitForRespawn(int index) {
		yield return new WaitForSeconds (2);
		StageEnemy enemy = getSpawnEnemy();
		spawn (index, enemy);
	}
	
	StageEnemy getSpawnEnemy() {
		int i;
		float p = Random.Range (0.0f, 1.0f);
		for (i = 0; i < spawnRatio.Count; p -= spawnRatio[i++]) {
			if(p > 0.0f && p <= spawnRatio[i]) {
				break;
			}
		}

		if (i >= spawnAmount.Count) {
			Debug.Log ("SpawnEnemy]Reset spawn ratio");
			setEnemyRatio();
			i = spawnSpecial.Count;
			spawnAmount [i] += 1;
		}
		spawnAmount [i] += 1;

		if (i < spawnSpecial.Count) {
			return spawnSpecial [i];
		}
		return spawnEnemy [i];
	}

	void changeEnemyRatio(int index) {
		int i = findSpawnEnemy (spawnPoints [index].spawnTransform.GetComponent<Enemy> ());

		if (i < 0) {
			Debug.Log ("[SpanwEnemy]Invalid enemy");
		}
		else if (i < spawnSpecial.Count) {
			spawnAmount [i]--;
		}
		else {
			if(--spawnAmount[i] == 0) {
				if(spawnEnemy[i].getStage() != spawnEnemy[spawnEnemy.Count - 1].getStage()) {
					removeEnemy(i);
				}
			}
		}
	}

	void setEnemyRatio() {
		float total = 0.0f;
		for (int i = 0; i < spawnSpecial.Count; i++) {
			total += spawnSpecial[i].enemyRatio;
		}

		for (int i = 0; i < spawnEnemy.Count; i++) {
			total += spawnEnemy[i].enemyRatio;
		}

		for (int i = 0; i < spawnSpecial.Count; i++) {
			spawnRatio[i] = (float)spawnSpecial[i].enemyRatio / total;
		}

		for (int i = 0, special = spawnSpecial.Count; i < spawnEnemy.Count; i++) {
			spawnRatio[special + i] = (float)spawnEnemy[i].enemyRatio / total;
		}
	}

	void startSpawnEnemies() {
		for (int index = 0; index < spawnPoints.Count; index++) {
			//Debug.Log ("[SpawnEnemy]Spawn " + e);
			StageEnemy enemy = getSpawnEnemy();
			spawn (index, enemy);
		}
	}

	void spawn(int index, StageEnemy e) {
		Transform enemy = Instantiate (e.enemy, spawnPoints[index].spawnPosition, Quaternion.identity) as Transform;
		enemy.SetParent(transform);
		enemy.GetComponent<Enemy> ().setStage (e.getStage());
		enemy.GetComponent<Enemy> ().enemyPath = spawnPath[index].enemyPath;
		spawnPoints[index].spawnTransform = enemy;
	}

	/*
	bool checkCyclePath(List<Path> enemyPath)
	{
		Vector3 pos = new Vector3 (0, 0);
		for (int i = 0; i < enemyPath.Count; i++) {
			if (enemyPath [i].dir == Direction.Up) {
				pos += Vector3.up;
			}
			if (enemyPath [i].dir == Direction.Down) {
				pos += Vector3.down;
			}
			if (enemyPath [i].dir == Direction.Left) {
				pos += Vector3.left;
			}
			if (enemyPath [i].dir == Direction.Right) {
				pos += Vector3.right;
			}
		}
		
		if (pos.Equals (new Vector3 (0, 0)))
			return true;
		return false;
	}
	*/
}

[System.Serializable]
public class EnemyPath {
	public List<Path> enemyPath;
	public List<Direction> enemyPathOrder;

	public EnemyPath() {
		this.enemyPath = new List<Path> ();
		this.enemyPathOrder = new List<Direction> ();
	}

	public EnemyPath(List<Path> path, List<Direction> pathOrder) {
		this.enemyPath = path;
		this.enemyPathOrder = pathOrder;
	}
}