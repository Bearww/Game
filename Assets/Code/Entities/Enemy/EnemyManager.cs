using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 女生管理
 */
public class EnemyManager : MonoBehaviour {

	public List<Enemies> enemies;

	void Start () {
		Debug.Log ("[EnemyManager]enemies: " + enemies.Count.ToString());
		DontDestroyOnLoad (gameObject);
	}

	void Update () {
	
	}

	public int getEnemyIndex(int id) {
		for (int i = 0; i < enemies.Count; i++) {
			if(enemies[i].enemyTransform.GetComponent<Enemy> ().id == id)
				return i;
		}
		return -1;
	}

	public Sprite getEnemySprite(int id) {
		for (int i = 0; i < enemies.Count; i++) {
			if(enemies[i].enemyTransform.GetComponent<Enemy> ().id == id)
				return enemies[i].enemySprite;
		}
		return null;
	}

	public void unlockEnemy(int id, float favorability) {
		for (int i = 0; i < enemies.Count; i++) {
			if(enemies[i].enemyTransform.GetComponent<Enemy> ().id == id) {
				int level = -1;
				if(favorability == 0f) level = 0;
				if(favorability >= 0.98f && favorability <= 1.02f) level = 1;
				if(favorability > 1f) level = 2;
				if(favorability > 1.2f) level = 3;

				if(enemies[i].enemyLevel < level)
					enemies[i].enemyLevel = level;
			}
		}
	}
}

[System.Serializable]
public class Enemies {
	public Transform enemyTransform;
	public Sprite enemySprite;
	public int enemyLevel;

	public Enemies() {
		enemyLevel = -1;
	}
}