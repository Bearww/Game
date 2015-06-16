using UnityEngine;
using System.Collections;

/*
 * 玩家設定
 */
public class Player : Entity {

	private Direction lastDir;
	private bool isDrag = false;
	private bool isOneClick = false;
	private bool isHurt = true;
	private bool isLoaded = false;
	private float timeForOneClick;
	private float animationTime;
	private float dragTime;
	private float flyingDuration;
	private Direction inputDirection;

	private Vector3 cameraDist;
	private Vector3 dragDir;
	private float posX;
	private float posY;

	public bool[] collisions = new bool[4];
	private int[] collisionTimes = new int[5];

	private int ratePrecision = 1;
	public int rate;

	public PlayerItemManager pItemManager;
	
	public Transform spriteTransform;
	public SpriteRenderer effectSprite;
	public SpriteRenderer shooterCompass;
	public SpriteRenderer shooterCharge;
	public SpriteRenderer shooterTimer;

	public Direction direction;

	public int collisionNumber;
	public float hurtCd;

	public float delay;
	public float doubleClickTime;
	public float dragDeviation;
	public float dragRadius;
	public float magnitude;

	public Bar shooterBar;

	// Player Status
	public PlayerStatus playerStatus;

	// Status : Normal
	public bool isActive;
	private bool isInverse = false;

	// Status : Use Item
	private bool isUseItem;

	// Status : Special
	private bool isFlying;
	private bool isInvincible;

	public float touchDeviation;

	void Start () {
		// 將商城物品加入
	}

	void Update () {
		switch (playerStatus) {
		case PlayerStatus.Init:
		case PlayerStatus.Punish:
		case PlayerStatus.Special:
		case PlayerStatus.Ending:
			break;
		case PlayerStatus.Normal:
		case PlayerStatus.UseItem:
			playerMove();
			break;
		}
	}

	float getRate() {
		if (ratePrecision == 0)
			ratePrecision = 1;
		return rate / ratePrecision;
	}

	void OnMouseDown() {
		if (playerStatus == PlayerStatus.Normal || playerStatus == PlayerStatus.UseItem) {
			if (!isOneClick) {
				cameraDist = Camera.main.WorldToScreenPoint (transform.position);
				posX = Input.mousePosition.x - cameraDist.x;
				posY = Input.mousePosition.y - cameraDist.y;
				isOneClick = true;
				timeForOneClick = Time.time;
			} else {
				float d = Time.time - timeForOneClick;
				//Debug.Log ("[Player]Double Click" + d);
				if (d <= doubleClickTime) {
					setActive (false);
					isOneClick = false;
				} else {
					posX = Input.mousePosition.x - cameraDist.x;
					posY = Input.mousePosition.y - cameraDist.y;
					timeForOneClick = Time.time;
				}
			}
		}
	}

	void OnMouseDrag() {
		float d = Time.time - timeForOneClick;
		Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, cameraDist.z);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos) - transform.position;
		float dist = Vector3.Distance (worldPos, new Vector3 (0, 0, 0));

		if (isLoaded && dist <= dragDeviation) {
			if(d >= delay) {
				isDrag = true;
				isLoaded = false;
			}
		}

		if (d >= delay && isDrag) {
			if(shooterCompass.color.a == 0)
				setShooter(true);

			float degree = Mathf.Atan2 (worldPos.x, worldPos.y) * Mathf.Rad2Deg;
			float absDeg = Mathf.Abs (degree);
			Quaternion q;

			if(absDeg >= 0f && absDeg <= 22.5f) {
				dragDir = new Vector2(0, 1);
				q = new Quaternion(0, 0, 1, 0);
			}
			else if(absDeg > 22.5f && absDeg <= 67.5f) {
				dragDir = new Vector2(1, 1);
				q = new Quaternion(0, 0, 2, 1);
			}
			else if(absDeg > 67.5f && absDeg <= 112.5f) {
				dragDir = new Vector2(1, 0);
				q = new Quaternion(0, 0, 1, 1);
			}
			else if(absDeg > 112.5f && absDeg <= 157.5f) {
				dragDir = new Vector2(1, -1);
				q = new Quaternion(0, 0, 1, 2);
			}
			else {
				dragDir = new Vector2(0, -1);
				q = new Quaternion(0, 0, 0, 0);
			}

			if(degree < 0) {
				dragDir.x *= -1;
				q.z *= -1;
			}

			dragDir /= Mathf.Sqrt (dragDir.x * dragDir.x + dragDir.y * dragDir.y);

			float ml = Mathf.Max(Mathf.Abs(worldPos.x), Mathf.Abs(worldPos.y));

			if(ml > dragRadius)
				ml = dragRadius;

			spriteTransform.localPosition = dragDir * ml;
			spriteTransform.rotation = q;
		}
	}

	void OnMouseUp() {
		float d = Time.time - timeForOneClick;
		if (isDrag && d >= delay) {
			setShooter(false);
			Physics2D.IgnoreLayerCollision(9, 9);
			Physics2D.IgnoreLayerCollision(9, 11);
			isFlying = true;
			playerStatus = PlayerStatus.Special;
			GetComponent<Rigidbody2D> ().isKinematic = true;
			GetComponent<Rigidbody2D> ().velocity = -dragDir * magnitude * 20;
			GetComponent<AudioSource> ().Play ();
			StartCoroutine(stopFlying());
		}
	}

	void rotateSprite() {
		if (direction == Direction.Up)
			spriteTransform.rotation = new Quaternion (0, 0, 0, 0);
		if (direction == Direction.Down)
			spriteTransform.rotation = new Quaternion (0, 0, 1, 0);
		if (direction == Direction.Left)
			spriteTransform.rotation = new Quaternion (0, 0, 1, 1);
		if (direction == Direction.Right)
			spriteTransform.rotation = new Quaternion (0, 0, -1, 1);
	}

	void playerMove() {
		// 改變玩家方向與圖片
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
			inputDirection = Direction.Up;
			setActive (true);
		}
		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
			inputDirection = Direction.Down;
			setActive (true);
		}
		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
			inputDirection = Direction.Left;
			setActive (true);
		}
		if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) {
			inputDirection = Direction.Right;
			setActive (true);
		}
		
		if (!isDrag && Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			float degree = Mathf.Atan2 (touchDeltaPosition.x, touchDeltaPosition.y) * Mathf.Rad2Deg;
			float absDeg = Mathf.Abs (degree);
			
			if(absDeg >= 0f && absDeg <= touchDeviation)
				inputDirection = Direction.Up;
			else if(absDeg >= 180f - touchDeviation)
				inputDirection = Direction.Down;
			else if(absDeg > 90f - touchDeviation && absDeg <= 90f + touchDeviation) {
				if(degree > 0)
					inputDirection = Direction.Right;
				else
					inputDirection = Direction.Left;
			}
			setActive(true);
		}

		if (isInverse) {
			if (inputDirection == Direction.Up)
				direction = Direction.Down;
			if (inputDirection == Direction.Down)
				direction = Direction.Up;
			if (inputDirection == Direction.Left)
				direction = Direction.Right;
			if (inputDirection == Direction.Right)
				direction = Direction.Left;
		} else
			direction = inputDirection;

		if(!isDrag)
			rotateSprite ();

		if (isActive) {
			// 依方向移動
			if (!collisions[0] && direction == Direction.Up) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.up * speed * getRate() * Time.deltaTime;
				collisions[1] = false;
			}
			if (!collisions[1] && direction == Direction.Down) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.down * speed * getRate() * Time.deltaTime;
				collisions[0] = false;
			}
			if (!collisions[2] && direction == Direction.Left) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.left * speed * getRate() * Time.deltaTime;
				collisions[3] = false;
			}
			if (!collisions[3] && direction == Direction.Right) {
				GetComponent<Rigidbody2D> ().transform.position += Vector3.right * speed * getRate() * Time.deltaTime;
				collisions[2] = false;
			}
		}
	}

	IEnumerator waitHurtCd() {
		yield return new WaitForSeconds (hurtCd);
		isHurt = false;
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

		rotateSprite ();
		setActive (true);
		setShooterUsable (false);
		resetCollision ();
	}

	public void addSpeedEffect(float r) {
		rate *= (int) r;
		ratePrecision *= 100;
	}

	public bool getHurt() {
		return isHurt;
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

	public void resetCollision() {
		for (int i = 0; i < collisions.Length; i++)
			collisions [i] = false;
		for (int i = 0; i < collisionTimes.Length; i++)
			collisionTimes [i] = 0;
	}

	public void setActive(bool active) {
		isActive = active;
		if (!active) {
			lastDir = direction;
		}
	}

	public void setCollision(Direction dir, bool collision) {
		for (int i = 0; i < 4; i++) {
			collisions[i] = false;
			collisionTimes [i] = 0;
		}

		if (dir == Direction.Up)
			collisions[0] = collision;
		else if (dir == Direction.Down)
			collisions[1] = collision;
		else if (dir == Direction.Left)
			collisions[2] = collision;
		else
			collisions[3] = collision;
	}

	public void setHurt(bool hurt) {
		isHurt = hurt;
		if (isHurt)
			StartCoroutine (waitHurtCd());
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

	public void setShooter(bool sight) {
		isDrag = sight;
		setActive(false);
		float a = sight ? 1 : 0;
		shooterCompass.color = new Color(1, 1, 1, a);
		shooterCharge.color = new Color(133f / 255f, 0, 96f/ 255f, a);
		shooterTimer.color = new Color(218f / 255f, 45f / 255f, 191f / 255f, a);
		if (sight) {
			dragTime = Time.time;
			shooterCharge.GetComponent<Animation> ().Play ();
			shooterTimer.GetComponent<Animation> ().Play ();
		} else {
			shooterCharge.GetComponent<Animation> ().Stop ();
			shooterTimer.GetComponent<Animation> ().Stop ();
			spriteTransform.position = transform.position;

			float aniLen = shooterTimer.GetComponent<Animation> ().clip.length;
			dragTime = Time.time - dragTime;
			float power = dragTime / aniLen;
			//Debug.Log ("[Player]Clip Length" + aniLen);

			if(power < 1) {
				power = Mathf.Abs(power - 0.5f);
				float dragPower = 0;

				for(int i = 0; i < 10; i++) {
					dragPower += 0.05f;
					if(power < dragPower) {
						flyingDuration = 1f - 0.1f * i;
						break;
					}
				}
			}
		}
	}

	public void setShooterUsable(bool usable) {
		isLoaded = usable;
		if (usable) {
			//shooterBar.GetComponent<Animation> ().Play ("flickerbar");
			//shooterBar.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
		} else {
			shooterBar.GetComponent<Animation> ().Play ("10scdbar");
			shooterBar.GetComponent<Animation> ().wrapMode = WrapMode.Default;
		}
	}

	public void startGetItemEffect() {
		effectSprite.enabled = true;
		StartCoroutine(stopGetItemEffect ());
	}

	public void startEffect() {
		if (!isLoaded) {
			foreach(AnimationState animState in shooterBar.GetComponent<Animation> ()) {
				animState.time = animationTime;
			}
			shooterBar.GetComponent<Animation> ().Play ();
		}
		pItemManager.bufItem.GetComponent<BuffItem> ().startEffect ();
		pItemManager.debufItem.GetComponent<BuffItem> ().startEffect ();
	}

	IEnumerator stopGetItemEffect() {
		yield return new WaitForSeconds (1);
		effectSprite.enabled = false;
	}

	public void stopEffect() {
		if (!isLoaded) {
			foreach(AnimationState animState in shooterBar.GetComponent<Animation> ()) {
				animationTime = animState.time;
			}
			shooterBar.GetComponent<Animation> ().Stop ();
		}
		pItemManager.bufItem.GetComponent<BuffItem> ().stopEffect ();
		pItemManager.debufItem.GetComponent<BuffItem> ().stopEffect ();
	}

	public void tryActive() {
		if (lastDir != direction) {
			setActive (true);
		}
	}

	public void updateCollision(Direction dir) {
		if (dir == Direction.Up)
			collisionTimes [0]++;
		else if (dir == Direction.Down)
			collisionTimes [1]++;
		else if (dir == Direction.Left)
			collisionTimes [2]++;
		else
			collisionTimes [3]++;

		if (++collisionTimes [4] == collisionNumber) {
			for(int i = 0; i < 4; i++) {
				if(collisionTimes[i] != 0)
					collisions[i] = true;
				else
					collisions[i] = false;
				collisionTimes[i] = 0;
			}
			collisionTimes[4] = 0;
		}
	}
}

public enum PlayerStatus {
	Init,
	Normal,
	UseItem,
	Punish,
	Special,
	Ending
}