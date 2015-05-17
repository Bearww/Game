using UnityEngine;
using UnityEngine.UI;
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

	public Direction direction;

	public bool isActive = true;

	void Start () {
		// 將商城物品加入
		pItemManager.addToItemInventory (4, 2);
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

	/*
	public void tryMoving() {
		Vector2 testDir = new Vector2 ();
		if (direction == Direction.Up) {
			testDir = Vector2.up;
		}
		if (direction == Direction.Down) {
			testDir = Vector2.up * -1;
		}
		if (direction == Direction.Left) {
			testDir = Vector2.right * -1;
		}
		if (direction == Direction.Right) {
			testDir = Vector2.right;
		}

		Vector2 current = new Vector2 (transform.position.x, transform.position.y);
		Collider2D [] col = Physics2D.OverlapCircleAll (current + testDir * 0.01f, 0.5f);

		if (col.Length > 1) {
			Debug.Log ("[Player]Collider amount " + col.Length);
		}
		else {
			setActive (true);
		}
	}
	*/
}
