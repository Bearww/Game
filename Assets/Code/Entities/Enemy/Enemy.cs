using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 女生編號、名稱、所需物品
 */
public class Enemy : Entity {

	public int id;
	public string enemyName;
	public Item enemyItem;

	private Vector3 lastPos;
	
	private int currentStep = 0;
	
	private bool isEscape = false;
	private bool isBack = false;
	
	public bool isCycle = false;

	public List<Path> enemyPath;

	void Start () {
		lastPos = transform.position;
	}

	void Update () {
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