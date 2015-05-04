using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 女生生成、移動路徑
 */

public class SpawnEnemy : MonoBehaviour {
	private GameObject playerObject;
	private GameObject enemyManagerObject;
	
	private Player player;
	
	private EnemyManager enemyManager;
	
	private Enemy currentEnemy;
	private int currentEnemyItemId;
	private Transform currentEnemyTransform;

	private Vector3 startPos;
	private Vector3 lastPos;
	
	private int currentStep = 0;

	private float speed;

	private bool isActive = true;
	private bool isEscape = false;
	private bool isBack = false;

	public bool isCycle = false;

	public List<Path> enemyPath = new List<Path> ();

	void Start () {
		if (playerObject == null) {
			playerObject = GameObject.FindGameObjectWithTag("Player");
			player = playerObject.GetComponent<Player> ();
		}
		
		if (enemyManagerObject == null) {
			enemyManagerObject = GameObject.FindGameObjectWithTag("Enemy");
			enemyManager = enemyManagerObject.GetComponent<EnemyManager> ();
			getEnemyInfo(1);
		}
		startPos = GetComponent<Transform> ().position;
		lastPos = GetComponent<Transform> ().position;
	}
	
	void Update () {
		if (isActive) {
			if (!isEscape && player.pItemManager.findInInventory(currentEnemyItemId) != -1) {
				isEscape = true;
			}
			if (isEscape && Vector2.Distance (GetComponent<Rigidbody2D> ().transform.position, player.GetComponent<Rigidbody2D> ().transform.position) < 1) {
				StartCoroutine(waitForRespawn());
			}

			if (!isBack) {
				if (enemyPath [currentStep].dir == Direction.Up) {
					GetComponent<Transform> ().position += Vector3.up * speed * Time.deltaTime;
				}
				if (enemyPath [currentStep].dir == Direction.Down) {
					GetComponent<Transform> ().position += Vector3.down * speed * Time.deltaTime;
				}
				if (enemyPath [currentStep].dir == Direction.Left) {
					GetComponent<Transform> ().position += Vector3.left * speed * Time.deltaTime;
				}
				if (enemyPath [currentStep].dir == Direction.Right) {
					GetComponent<Transform> ().position += Vector3.right * speed * Time.deltaTime;
				}
			} else {
				if (enemyPath [currentStep].dir == Direction.Up) {
					GetComponent<Transform> ().position += Vector3.down * speed * Time.deltaTime;
				}
				if (enemyPath [currentStep].dir == Direction.Down) {
					GetComponent<Transform> ().position += Vector3.up * speed * Time.deltaTime;
				}
				if (enemyPath [currentStep].dir == Direction.Left) {
					GetComponent<Transform> ().position += Vector3.right * speed * Time.deltaTime;
				}
				if (enemyPath [currentStep].dir == Direction.Right) {
					GetComponent<Transform> ().position += Vector3.left * speed * Time.deltaTime;
				}
			}
			
			if (enemyPath [currentStep].steps <= Vector2.Distance (GetComponent<Transform> ().position, lastPos)) {
				lastPos = GetComponent<Transform> ().position;
				if(isBack)
					currentStep--;
				else
					currentStep++;
				
				if(currentStep == enemyPath.Count) {
					if(isCycle)
						currentStep = 0;
					else {
						isBack = true;
						currentStep--;
					}
				}
				else if(currentStep < 0) {
					isBack = false;
					currentStep++;
				}
			}
		}
	}
	
	IEnumerator waitForRespawn()
	{
		isActive = false;
		GetComponentInChildren<SpriteRenderer> ().enabled = isActive;
		yield return new WaitForSeconds(2);
		isActive = true;
		GetComponentInChildren<SpriteRenderer> ().enabled = isActive;
	}

	void getEnemyInfo(int id)
	{
		int index = enemyManager.getEnemyIndex (id);
		if (index > 0) {
			currentEnemyTransform = enemyManager.enemies [index].enemyTransform;
			currentEnemyItemId = id;
			currentEnemy = currentEnemyTransform.GetComponent<Enemy> ();

			speed = currentEnemy.speed;
		} else {
			currentEnemyTransform = enemyManager.enemies [0].enemyTransform;
			currentEnemyItemId = 0;
			currentEnemy = currentEnemyTransform.GetComponent<Enemy> ();
			speed = currentEnemy.speed;
		}
	}

	void resetEnemy()
	{
		isActive = true;
		isEscape = false;
		isBack = false;

		GetComponent<Transform> ().position = startPos;
	}

	void checkCyclePath()
	{
		Vector3 pos = new Vector3 ();
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

		if(pos.Equals (new Vector3()))
		   isCycle = true;
		else
		   isCycle = false;
	}
}

public enum Direction {
	Up,
	Down,
	Left,
	Right
}

[System.Serializable]
public class Path {
	public Direction dir;
	public int steps;
}