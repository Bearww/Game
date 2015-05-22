using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * 玩家設定
 */
public class Player : Entity {

	private bool isOneClick = false;
	private float timeForOneClick;

	private Vector3 cameraDist;
	private float posX;
	private float posY;

	public PlayerItemManager pItemManager;

	public Sprite forward;
	public Sprite backward;
	public Sprite left;
	public Sprite right;
	public SpriteRenderer playerSprite;
	public Transform spriteTransform;	

	public Direction direction;

	public float delay;
	public float doubleClickTime;
	public float dragLength;

	public bool isActive = true;

	void Start () {
		// 將商城物品加入
		pItemManager.addToItemInventory (14, 2);
	}

	void Update () {
		// 改變玩家方向與圖片
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			direction = Direction.Up;
			playerSprite.sprite = backward;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			direction = Direction.Down;
			playerSprite.sprite = forward;
		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			direction = Direction.Left;
			playerSprite.sprite = left;
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			direction = Direction.Right;
			playerSprite.sprite = right;
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

	void OnMouseDown() {
		if (!isOneClick) {
			cameraDist = Camera.main.WorldToScreenPoint(transform.position);
			posX = Input.mousePosition.x - cameraDist.x;
			posY = Input.mousePosition.y - cameraDist.y;
			spriteTransform.position = new Vector3(0, 0);
			isOneClick = true;
			timeForOneClick = Time.time;
		}
		else {
			float d = Time.time - timeForOneClick;
			Debug.Log ("[Player]Double Click" + d);
			if(d <= doubleClickTime) {
				setActive(false);
				isOneClick = false;
			}
			else {
				posX = Input.mousePosition.x - cameraDist.x;
				posY = Input.mousePosition.y - cameraDist.y;
				timeForOneClick = Time.time;
			}
		}
	}

	void OnMouseDrag() {
		float d = Time.time - timeForOneClick;
		if (d >= delay) {
			setActive(false);
			Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, cameraDist.z);
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos) - transform.position;
			Vector3 dir;
			float degree = Mathf.Atan2 (worldPos.x, worldPos.y) * Mathf.Rad2Deg;
			float absDeg = Mathf.Abs (degree);
			Debug.Log ("[Player]worldPos" + degree);
			if(absDeg >= 0f && absDeg <= 22.5f)
				dir = new Vector2(0, 1);
			else if(absDeg > 22.5f && absDeg <= 67.5f)
				dir = new Vector2(1, 1);
			else if(absDeg > 67.5f && absDeg <= 112.5f)
				dir = new Vector2(1, 0);
			else if(absDeg > 112.5f && absDeg <= 157.5f)
				dir = new Vector2(1, -1);
			else
				dir = new Vector2(0, -1);

			if(degree < 0)
				dir.x *= -1;

			dir /= Mathf.Sqrt (dir.x * dir.x + dir.y * dir.y);

			float ml = Mathf.Max(Mathf.Abs(worldPos.x), Mathf.Abs(worldPos.y));

			if(ml > dragLength)
				ml = dragLength;

			spriteTransform.position = dir * ml + transform.position;
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
