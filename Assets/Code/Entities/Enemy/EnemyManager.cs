using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 女生管理
 */
public class EnemyManager : MonoBehaviour {

	public List<Enemies> enemies = new List<Enemies> ();

	void Start () {
		Debug.Log ("[EnemyManager]enemies: " + enemies.Count.ToString());
		DontDestroyOnLoad (gameObject);
	}

	void Update () {
	
	}

	public int getEnemyIndex(int id)
	{
		for (int i = 0; i < enemies.Count; i++) {
			if(enemies[i].enemyTransform.GetComponent<Enemy> ().id == id)
				return i;
		}
		return -1;
	}
}

[System.Serializable]
public class Enemies {
	public Transform enemyTransform;
	public int enemyAmount;
}