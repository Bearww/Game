using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private GameMode gameMode;

	private int gameStage;

	private int gameTime;

	private int gameScore;

	private int stageScore;

	private int basicScore;

	private int goddessIndex;

	private Enemy goddessInfo;

	public EnemyManager enemies;

	public int minuteTimeLimits;

	public Text timeText;
	public Text countDownText;

	public Text scoreText;

	public List<Stage> stages;

	public Player player;
	public Goddess goddess;
	public SpawnEnemies spawner;

	public int minFavorability;

	void Awake () {
		Debug.Log ("[GameManager]Game Stages " + stages.Count);
		gameMode = GameMode.Ready;
		gameStage = 0;
		gameScore = stageScore = basicScore = 0;
		gameTime = 3;
		addEnemy ();
		player.setActive (false);
		InvokeRepeating("countDown", 0.1f, 1f);
	}

	void Update () {

	}

	void countDown() {
		if (gameMode == GameMode.Ready) {
			if(gameTime > 0) {
				countDownText.text = gameTime--.ToString();
			}
			else if(gameTime == 0) {
				countDownText.text = "Go";
				gameTime--;
			}
			else {
				countDownText.gameObject.SetActive(false);
				gameMode = GameMode.Normal;
				gameTime = minuteTimeLimits * 60;
				player.setActive(true);
			}
		}
		if (gameMode == GameMode.Normal) {
			int min = gameTime / 60;
			int sec = gameTime % 60;
			timeText.text = string.Format ("{0}:{1:00}", min, sec);
			gameTime--;
		}
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

	public void flirtWithGoddess(Enemy e, int index) {
		Debug.Log ("[GameManager]Flirt with goddess");
		gameMode = GameMode.Flirt;
		goddessIndex = index;
		goddessInfo = e;
		player.gameObject.SetActive (false);
		spawner.gameObject.SetActive (false);
		goddess.gameObject.SetActive (true);
		goddess.startFlirt (enemies.getEnemySprite(e.id));
	}

	public void goddessFavorability(int favorability) {
		if (favorability < minFavorability) {
			Debug.Log ("[GameManager]Not captured heart");
		}
		else {
			int score = goddessInfo.GetComponentInParent<SpawnEnemies> ().getEnemyScore (goddessInfo);
			enemies.unlockEnemy(goddessInfo.id);
			extraScore (score);
			goddessInfo.GetComponentInParent<SpawnEnemies> ().respawnEnemy (goddessIndex);
		}
		gameMode = GameMode.Normal;
	}

	public void relentGoddess(Enemy e, int index) {
		int score = e.GetComponentInParent<SpawnEnemies> ().getEnemyScore (e);
		enemies.unlockEnemy(e.id);
		extraScore (score);
		e.GetComponentInParent<SpawnEnemies> ().respawnEnemy (index);
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

public enum GameMode {
	Ready,
	Normal,
	Flirt,
	Final
}