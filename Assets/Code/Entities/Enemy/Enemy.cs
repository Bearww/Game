using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 女生編號、名稱、所需物品
 */
public class Enemy : Entity {

	private const float dev = 0.05f;
	
	private Vector3 lastPos;
	
	private int currentStep = 0;
	
	private bool isActive = true;
	private bool isEscape = false;

	private int stage;

	public int id;
	public string enemyName;
	public Item enemyItem;
	public List<Path> enemyPath;

	void Start () {
		lastPos = transform.position;
	}

	void Update () {
		if (isActive && !isEscape) {
			if(enemyPath.Count > 0) {
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
		
				if (enemyPath [currentStep].steps <= Vector2.Distance (GetComponent<Transform> ().position, lastPos)) {
					Vector3 current = GetComponent<Transform> ().position;

					if(enemyPath[currentStep].dir == Direction.Up) {
						current.y = Mathf.Round (current.y);
						current.y += dev;
					}
					if(enemyPath[currentStep].dir == Direction.Down) {
						current.y = Mathf.Round (current.y);
						current.y -= dev;
					}
					if(enemyPath[currentStep].dir == Direction.Left) {
						current.x = Mathf.Round (current.x);
						current.x -= dev;
					}
					if(enemyPath[currentStep].dir == Direction.Right) {
						current.x = Mathf.Round (current.x);
						current.x += dev;
					}

					GetComponent<Transform> ().position = current;
					lastPos = current;
				
					if (++currentStep == enemyPath.Count)
						currentStep = 0;
				}
			}
		}
	}

	public int getStage() {
		return stage;
	}

	public void setActive(Direction dir) {
		if (dir == Direction.Up)
			isActive =  !(enemyPath [currentStep].dir == Direction.Down);
		if (dir == Direction.Down)
			isActive =  !(enemyPath [currentStep].dir == Direction.Up);
		if (dir == Direction.Left)
			isActive =  !(enemyPath [currentStep].dir == Direction.Right);
		if (dir == Direction.Right)
			isActive =  !(enemyPath [currentStep].dir == Direction.Left);
	}

	public void setActive(bool active) {
		isActive = active;
	}

	public void setStage(int stage) {
		this.stage = stage;
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

	public Path(Direction dir) {
		this.dir = dir;
		this.steps = 1;
	}
}