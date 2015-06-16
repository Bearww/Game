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
	private int stageBasicScore;

	private PlayerStatus previousStatus;

	private int goddessAmount;
	private int goddessIndex;
	private int allHeartAmount;
	private int getHeartAmount;

	private Enemy goddessInfo;

	public EnemyManager enemies;

	public int minuteTimeLimits;

	public Text timeText;
	public Text countDownText;

	public Text stageText;
	public Bar stageScoreBar;
	public Bar basicScoreBar;
	public Animation stageMessage;
	public Animation warningMessage;

	public Canvas ending;
	public Text goddessAmountText;
	public Text getHeartAmountText;
	public Text getHeartRatioText;
	public Text goddessFavorabilityText;

	public List<Stage> stages;
	
	public Player player;
	public Goddess goddess;
	public SpawnEnemies enemySpawner;
	public SpawnItems itemSpawner;

	public AudioClip countDownAudio;
	public AudioClip gameBgAudio;

	void Awake () {
		Debug.Log ("[GameManager]Game Stages " + stages.Count);
		init ();
		addEnemy ();
		InvokeRepeating("countDown", 0.1f, 1f);
	}

	void Update () {

	}

	void init() {
		gameMode = GameMode.Ready;
		gameStage = 0;
		gameScore = 0;
		stageScore = 0;
		basicScore = 0;
		goddessAmount = 0;
		allHeartAmount = 0;
		getHeartAmount = 0;
		stageScoreBar.setBar (0);
		basicScoreBar.setBar (0);
		player.shooterBar.setBar (0);
		gameTime = 3;
		player.playerStatus = PlayerStatus.Init;
	}

	void countDown() {
		if (gameMode == GameMode.Ready) {
			if(gameTime == 3) {
				GetComponent<AudioSource> ().clip = countDownAudio;
				GetComponent<AudioSource> ().Play();
			}
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
				GetComponent<AudioSource> ().clip = gameBgAudio;
				GetComponent<AudioSource> ().Play();
				GetComponent<AudioSource> ().loop = true;
				player.playerStatus = PlayerStatus.Normal;
				player.setShooterUsable(false);
				player.setActive(true);
			}
		}
		if (gameMode == GameMode.Normal) {
			if(gameTime == 0) {
				gameEnding();
			}

			int min = gameTime / 60;
			int sec = gameTime % 60;
			timeText.text = string.Format ("{0}:{1:00}", min, sec);
			gameTime--;
		}
	}

	void addEnemy() {
		if (gameStage < stages.Count) {
			removePreviousEnemy();
			enemySpawner.changeSpecialEnemy(stages[gameStage].stageSpeed);

			if(stages[gameStage].specialEnemy.enemy != null) {
				Debug.Log ("[GameManager]Add special enemy");
				stages[gameStage].specialEnemy.setStage(gameStage);
				stages[gameStage].specialEnemy.enemy.GetComponent<Enemy> ().speed = stages[gameStage].stageSpeed;
				enemySpawner.addSpecialEnemy(stages[gameStage].specialEnemy);
			}
			Debug.Log ("[GameManager]Add enemy " + stages[gameStage].stageEnemies.Count);
			for(int i = 0; i < stages[gameStage].stageEnemies.Count; i++) {
				stages[gameStage].stageEnemies[i].setStage(gameStage);
				stages[gameStage].stageEnemies[i].enemy.GetComponent<Enemy> ().speed = stages[gameStage].stageSpeed;
				enemySpawner.addEnemy(stages[gameStage].stageEnemies[i]);
			}
		}
	}

	void removePreviousEnemy() {
		if (gameStage > 0) {
			Debug.Log ("[GameManager]Remove previous enemy " + stages [gameStage - 1].stageEnemies.Count);
			for (int i = 0; i < stages[gameStage - 1].stageEnemies.Count; i++) {
				enemySpawner.stopSpawnEnemy (stages [gameStage - 1].stageEnemies [i]);
			}
		}
	}

	void gameEnding() {
		ending.gameObject.SetActive(true);
		gameMode = GameMode.Final;
		player.setActive (false);
		player.playerStatus = PlayerStatus.Ending;
		CancelInvoke("countDown");
		goddessAmountText.text = goddessAmount.ToString ();
		getHeartAmountText.text = getHeartAmount.ToString ();
		//Debug.Log (getHeartAmount);
		if (allHeartAmount > 0)
			getHeartAmountText.text = ((float)(getHeartAmount / allHeartAmount) * 100).ToString ();
		else
			getHeartAmountText.text = "0";
		goddessFavorabilityText.text = gameScore.ToString ();
	}
	
	void updateScore() {
		if (gameStage < stages.Count) {
			stageText.text = (gameStage + 1).ToString();
			stageScoreBar.setBar ((stageScore * 100) / stages [gameStage].stageScore);
			if(stageBasicScore > 0)
				basicScoreBar.setBar ((basicScore * 100) / stageBasicScore);
		}
		else {
			stageText.text = (gameStage + 1).ToString();
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
				stageBasicScore = basicScore = gameScore / 10;
				stageScoreBar.setBar(0f);
				basicScoreBar.setBar(100f);
				stageMessage.GetComponentInChildren<Text> ().text = (gameStage + 1).ToString();
				stageMessage.Play();
				addEnemy ();
				player.setHurt(true);
				if(gameStage == 1) {
					Sprite sprite = Resources.Load<Sprite> ("UI/warning-gay");
					warningMessage.GetComponent<Image> ().sprite = sprite;
					warningMessage.Play();
				}
				if(gameStage == 2) {
					Sprite sprite = Resources.Load<Sprite> ("UI/warning-oldmama");
					warningMessage.GetComponent<Image> ().sprite = sprite;
					warningMessage.Play();
				}
			}
		}
		updateScore ();
	}

	public void lossScore() {
		int score = stages [gameStage].lossScore;
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
				gameEnding();
			}
		}
		updateScore ();
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
				gameEnding();
			}
		}
		updateScore ();
	}

	public void flirtWithGoddess(Enemy e, int index) {
		Debug.Log ("[GameManager]Flirt with goddess");
		gameMode = GameMode.Flirt;
		goddessIndex = index;
		goddessInfo = e;
		previousStatus = player.getPlayerStatus ();
		player.stopEffect ();
		GetComponent<AudioSource> ().Stop ();
		player.gameObject.SetActive (false);
		enemySpawner.gameObject.SetActive (false);
		itemSpawner.gameObject.SetActive (false);
		goddess.gameObject.SetActive (true);
		goddess.startFlirt (enemies.getEnemySprite(e.id), stages[e.getStage()].stageHeartInterval);
	}

	public void goddessFavorability(float favorability, int getHearts, int allHearts) {
		Debug.Log (string.Format ("[GameManager]Bonus: {0} Hearts: {1} AllHearts: {2}", favorability, getHearts, allHearts));
		gameMode = GameMode.Normal;
		getHeartAmount += getHearts;
		allHeartAmount += allHearts;
		player.gameObject.SetActive (true);
		enemySpawner.gameObject.SetActive (true);
		itemSpawner.gameObject.SetActive (true);
		goddess.gameObject.SetActive (false);
		player.playerStatus = previousStatus;
		player.startEffect ();
		GetComponent<AudioSource> ().Play ();

		if (favorability == 0) {
			Debug.Log ("[GameManager]Not captured heart");
		}
		else {
			int score = goddessInfo.GetComponentInParent<SpawnEnemies> ().getEnemyScore (goddessInfo);
			enemies.unlockEnemy(goddessInfo.id, favorability);
			extraScore (Mathf.CeilToInt(score * favorability));
			goddessAmount += 1;
			goddessInfo.GetComponentInParent<SpawnEnemies> ().respawnEnemy (goddessIndex);
		}
	}

	public void relentGoddess(Enemy e, int index) {
		int score = e.GetComponentInParent<SpawnEnemies> ().getEnemyScore (e);
		enemies.unlockEnemy(e.id, 1f);
		extraScore (score);
		goddessAmount += 1;
		e.GetComponentInParent<SpawnEnemies> ().respawnEnemy (index);
	}
}

[System.Serializable]
public class Stage {
	public int stageScore;
	public int lossScore;
	public float stageSpeed;
	public float stageHeartInterval;
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