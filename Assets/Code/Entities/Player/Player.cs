using UnityEngine;
using System.Collections;

/*
 * 玩家設定
 */
public class Player : Entity {

	private Direction lastDir;
	private bool isDrag = false;
	private bool isInverse = false;
	private bool isOneClick = false;
	private float timeForOneClick;
	private float flyingDuration;

	private Vector3 cameraDist;
	private Vector3 dragDir;
	private float posX;
	private float posY;

	private int ratePrecision = 1;
	public int rate;

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
	public float dragDeviation;
	public float dragRadius;
	public float magnitude;

	// Player Status
	public PlayerStatus playerStatus;

	// Status : Normal
	public bool isActive;

	// Status : Use Item
	private bool isUseItem;

	// Status : Special
	private bool isFlying;
	private bool isInvincible;

	public float touchDeviation;

	void Start () {
		// 將商城物品加入
		pItemManager.addToItemInventory (13, 1);
		pItemManager.addToItemInventory (14, 2);
		pItemManager.addToItemInventory (15, 5);
		pItemManager.addToItemInventory (16, 1);
		pItemManager.addToItemInventory (17);
	}

	void Update () {
		// 改變玩家方向與圖片
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			if(!isInverse)
				direction = Direction.Up;
			else
				direction = Direction.Down;
			playerSprite.sprite = backward;
		}
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			if(!isInverse)
				direction = Direction.Down;
			else
				direction = Direction.Up;
			playerSprite.sprite = forward;
		}
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
			if(!isInverse)
				direction = Direction.Left;
			else
				direction = Direction.Right;
			playerSprite.sprite = left;
		}
		if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
			if(!isInverse)
				direction = Direction.Right;
			else
				direction = Direction.Left;
			playerSprite.sprite = right;
		}

		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			float degree = Mathf.Atan2 (touchDeltaPosition.x, touchDeltaPosition.y) * Mathf.Rad2Deg;
			float absDeg = Mathf.Abs (degree);

			if(absDeg >= 0f && absDeg <= touchDeviation)
				direction = Direction.Up;
			else if(absDeg >= 180f - touchDeviation)
				direction = Direction.Down;
			else if(absDeg > 90f - touchDeviation && absDeg <= 90f + touchDeviation) {
				if(degree > 0)
					direction = Direction.Right;
				else
					direction = Direction.Left;
			}
		}

		if (isActive) {
			// 依方向移動
			if (direction == Direction.Up) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.up * speed * getRate() * Time.deltaTime;
			}
			if (direction == Direction.Down) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.down * speed * getRate() * Time.deltaTime;
			}
			if (direction == Direction.Left) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.left * speed * getRate() * Time.deltaTime;
			}
			if (direction == Direction.Right) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.right * speed * getRate() * Time.deltaTime;
			}
		}
		else if(!GetComponent<Rigidbody2D> ().isKinematic) {
			tryActive();
		}
	}

	float getRate() {
		return rate / ratePrecision;
	}

	void OnMouseDown() {
		if (!isOneClick) {
			cameraDist = Camera.main.WorldToScreenPoint(transform.position);
			posX = Input.mousePosition.x - cameraDist.x;
			posY = Input.mousePosition.y - cameraDist.y;
			isOneClick = true;
			timeForOneClick = Time.time;
		}
		else {
			float d = Time.time - timeForOneClick;
			//Debug.Log ("[Player]Double Click" + d);
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
			Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, cameraDist.z);
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos) - transform.position;

			if(Vector3.Distance(worldPos, new Vector3(0, 0, 0)) > dragDeviation) {
				isDrag = true;
				setActive(false);
				float degree = Mathf.Atan2 (worldPos.x, worldPos.y) * Mathf.Rad2Deg;
				float absDeg = Mathf.Abs (degree);

				if(absDeg >= 0f && absDeg <= 22.5f)
					dragDir = new Vector2(0, 1);
				else if(absDeg > 22.5f && absDeg <= 67.5f)
					dragDir = new Vector2(1, 1);
				else if(absDeg > 67.5f && absDeg <= 112.5f)
					dragDir = new Vector2(1, 0);
				else if(absDeg > 112.5f && absDeg <= 157.5f)
					dragDir = new Vector2(1, -1);
				else
					dragDir = new Vector2(0, -1);

				if(degree < 0)
					dragDir.x *= -1;

				dragDir /= Mathf.Sqrt (dragDir.x * dragDir.x + dragDir.y * dragDir.y);

				float ml = Mathf.Max(Mathf.Abs(worldPos.x), Mathf.Abs(worldPos.y));

				if(ml > dragRadius)
					ml = dragRadius;

				spriteTransform.position = dragDir * ml + transform.position;
			}
		}
	}

	void OnMouseUp() {
		if (isDrag) {
			isDrag = false;
			setActive(false);
			spriteTransform.position = transform.position;
			Physics2D.IgnoreLayerCollision(9, 9);
			Physics2D.IgnoreLayerCollision(9, 11);
			isFlying = true;
			playerStatus = PlayerStatus.Special;
			GetComponent<Rigidbody2D> ().isKinematic = true;
			GetComponent<Rigidbody2D> ().velocity = -dragDir * magnitude * 20;
			flyingDuration = 1f;
			StartCoroutine(stopFlying());
		}
	}

	IEnumerator stopFlying() {
		yield return new WaitForSeconds (flyingDuration);
		//Debug.Log ("[Player]Stop Fly");
		isFlying = false;
		playerStatus = PlayerStatus.Normal;
		GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<Rigidbody2D> ().velocity = new Vector2(0, 0);
		Physics2D.IgnoreLayerCollision(9, 9, false);
		Physics2D.IgnoreLayerCollision(9, 11, false);

		Collider2D [] cols = Physics2D.OverlapCircleAll (GetComponent<Rigidbody2D> ().position, 0.5f);
		if(cols.Length > 1) {
			GetComponent<Rigidbody2D> ().AddRelativeForce(new Vector2(dragDir.x, dragDir.y));
		}
		setActive (false);
	}

	public void addSpeedEffect(float r) {
		rate *= (int) r;
		ratePrecision *= 100;
	}

	public PlayerStatus getPlayerStatus() {
		if (isFlying)
			return PlayerStatus.Special;
		if (isInvincible)
			return PlayerStatus.Special;
		if (isUseItem)
			return PlayerStatus.UseItem;
		return PlayerStatus.Normal;
	}

	public void removeSpeedEffect(float r) {
		rate /= (int) r;
		ratePrecision /= 100;
	}

	public void setActive(bool active) {
		isActive = active;
		if (!active) {
			lastDir = direction;
		}
	}

	public void setInverse(bool inverse) {
		isInverse = inverse;
		if (direction == Direction.Up)
			direction = Direction.Down;
		else if (direction == Direction.Down)
			direction = Direction.Up;
		else if (direction == Direction.Left)
			direction = Direction.Right;
		else
			direction = Direction.Left;
	}

	public void setInvincible(bool invincible) {
		isInvincible = invincible;
		if (isInvincible)
			playerStatus = PlayerStatus.UseItem;
		else
			playerStatus = PlayerStatus.Normal;
	}

	public void setUseItem(bool use) {
		isUseItem = use;
		if (isUseItem)
			playerStatus = PlayerStatus.UseItem;
		else
			playerStatus = PlayerStatus.Normal;
	}

	public void tryActive() {
		if (lastDir != direction) {
			setActive (true);
		}
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

public enum PlayerStatus {
	Normal,
	UseItem,
	Special
}