using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private int gameStage;

	private int gameTime;

	private int gameScore;

	private int stageScore;

	private int basicScore;

	public int minuteTimeLimits;

	public Text timeText;

	public Text scoreText;

	public List<Stage> stages;

	public SpawnEnemies spawner;

	void Awake () {
		Debug.Log ("[GameManager]Game Stages " + stages.Count);
		gameStage = 0;
		gameScore = stageScore = basicScore = 0;
		gameTime = minuteTimeLimits * 60;
		addEnemy ();
		InvokeRepeating("countDown", 1f, 1f);
	}

	void Update () {
	
	}

	void countDown() {
		int min = gameTime / 60;
		int sec = gameTime % 60;
		timeText.text = string.Format ("{0}:{1:00}", min, sec);
		gameTime--;
	}

	void addEnemy() {
		if (gameStage < stages.Count) {
			removePreviousEnemy();
			if(stages[gameStage].specialEnemy.enemy != null) {
				Debug.Log ("[GameManager]Add special enemy");
				stages[gameStage].specialEnemy.setStage(gameStage);
				spawner.addSpecialEnemy(stages[gameStage].specialEnemy);
			}
			Debug.Log ("[GameManager]Add enemy " + stages[gameStage].stageEnemies.Count);
			for(int i = 0; i < stages[gameStage].stageEnemies.Count; i++) {
				stages[gameStage].stageEnemies[i].setStage(gameStage);
				spawner.addEnemy(stages[gameStage].stageEnemies[i]);
			}
		}
	}

	void removePreviousEnemy() {
		if (gameStage > 0) {
			Debug.Log ("[GameManager]Remove previous enemy " + stages [gameStage - 1].stageEnemies.Count);
			for (int i = 0; i < stages[gameStage - 1].stageEnemies.Count; i++) {
				spawner.stopSpawnEnemy (stages [gameStage - 1].stageEnemies [i]);
			}
		}
	}

	public void extraScore(int score) {
		stageScore += score;
		gameScore += score;

		if (gameStage < stages.Count) {
			if (stageScore >= stages [gameStage].stageScore) {
				Debug.Log ("[GameManager]Stage Change");
				gameStage++;
				stageScore = 0;
				basicScore = gameScore / 10;
				addEnemy ();
			}
		}

		if (gameStage < stages.Count) {
			scoreText.text = string.Format ("Stage:{0}\nScore:{1}/{2}/{3}", gameStage, basicScore, stageScore, stages [gameStage].stageScore);
		}
		else {
			scoreText.text = string.Format ("Stage:{0}\nScore:{1}", gameStage, gameScore);
		}
	}
	
	public void lossScore(int score) {
		if (stageScore >= score) {
			gameScore -= score;
			stageScore -= score;
		} else {
			score -= stageScore;
			stageScore = 0;
			if (basicScore >= score) {
				basicScore -= score;
			} else {
				Debug.Log ("[GameManager]Gameover");
			}
		}
		if (gameStage < stages.Count) {
			scoreText.text = string.Format ("Stage:{0}\nScore:{1}/{2}/{3}", gameStage, basicScore, stageScore, stages [gameStage].stageScore);	
		}
		else {
			scoreText.text = string.Format ("Stage:{0}\nScore:{1}", gameStage, gameScore);
		}
	}
}

[System.Serializable]
public class Stage {
	public int stageScore;
	public int lossScore;
	public List<StageEnemy> stageEnemies; 
	public StageEnemy specialEnemy;
}

[System.Serializable]
public class StageEnemy {
	private int stage;
	public Transform enemy;
	public int enemyScore;
	public float enemyRatio;

	public int getStage() {
		return stage;
	}

	public void setStage(int stage) {
		this.stage = stage;
	}
}