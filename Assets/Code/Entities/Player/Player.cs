using UnityEngine;
using System.Collections;

/*
 * 玩家設定
 */
public class Player : Entity {

	public PlayerItemManager pItemManager;

	public Sprite forward;
	public Sprite backward;
	public Sprite left;
	public Sprite right;
	public SpriteRenderer spriteParent;

	public int direction;

	void Start () {
		// 將商城物品加入
	}

	void Update () {
		// 改變玩家方向與圖片
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			direction = 0;
			spriteParent.sprite = backward;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			direction = 1;
			spriteParent.sprite = forward;
		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			direction = 2;
			spriteParent.sprite = left;
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			direction = 3;
			spriteParent.sprite = right;
		}

		// 依方向移動
		if (direction == 0) {
			GetComponent<Rigidbody2D>().transform.position += Vector3.up * speed * Time.deltaTime;
		}
		if (direction == 1) {
			GetComponent<Rigidbody2D>().transform.position += Vector3.down * speed * Time.deltaTime;
		}
		if (direction == 2) {
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (direction == 3) {
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
		}
	}
}
