using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * 玩家設定
 */
public class Player : Entity {

	private int count;

	public Text scoreText;

	public PlayerItemManager pItemManager;

	public Sprite forward;
	public Sprite backward;
	public Sprite left;
	public Sprite right;
	public SpriteRenderer spriteParent;

	public Direction direction;

	public bool isActive = true;

	void Start () {
		// 設定分數
		count = 0;
		// 將商城物品加入
	}

	void Update () {
		// 改變玩家方向與圖片
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			direction = Direction.Up;
			spriteParent.sprite = backward;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			direction = Direction.Down;
			spriteParent.sprite = forward;
		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			direction = Direction.Left;
			spriteParent.sprite = left;
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			direction = Direction.Right;
			spriteParent.sprite = right;
		}

		if (isActive) {
			// 依方向移動
			if (direction == Direction.Up) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.up * speed * Time.deltaTime;
			}
			if (direction == Direction.Down) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.down * speed * Time.deltaTime;
			}
			if (direction == Direction.Left) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.left * speed * Time.deltaTime;
			}
			if (direction == Direction.Right) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.right * speed * Time.deltaTime;
			}
		}
	}

	public void setActive(bool active) {
		isActive = active;
	}

	public void scorePoints(int points) {
		count += points;
		scoreText.text = "Score: " + count.ToString ();
	}
	
	public void lossPoints(int points) {
		if (count >= points)
			count -= points;
		else
			count = 0;
		scoreText.text = "Score: " + count.ToString ();
	}
}
