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
	private int distance = 0;

	private bool enableChase = true;
	private bool isChase = false;
	private bool isForceChase = false;
	private bool isActive = true;

	private float refindPathTime = 5f;
	private float startTraceTime;

	private Player player;

	public bool isGay;

	private int stage;

	public int id;
	public string enemyName;
	public Item enemyItem;
	public float chaseTime;
	public List<Path> enemyPath;
	public List<Path> chasePath;

	void Start () {
		lastPos = transform.position;
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		if(obj != null)
			player = obj.GetComponent<Player> ();
	}

	void Update () {
		if (isActive) {
			if(isChase) {
				move (chasePath);
				if(Vector3.Distance(player.transform.position, transform.position) < 3f) {
					isChase = false;
					isForceChase = true;
				}
			}
			else if(isForceChase) {
				if(player.GetComponent<Rigidbody2D>().transform.position.y > (GetComponent<Rigidbody2D>().transform.position.y + distance))
				{
					GetComponent<Rigidbody2D>().transform.position += Vector3.up * speed * Time.deltaTime;
				}
				if(player.GetComponent<Rigidbody2D>().transform.position.y < (GetComponent<Rigidbody2D>().transform.position.y - distance))
				{
					GetComponent<Rigidbody2D>().transform.position += Vector3.down * speed * Time.deltaTime;
				}
				if(player.GetComponent<Rigidbody2D>().transform.position.x > (GetComponent<Rigidbody2D>().transform.position.x + distance))
				{
					GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
				}
				if(player.GetComponent<Rigidbody2D>().transform.position.x < (GetComponent<Rigidbody2D>().transform.position.x - distance))
				{
					GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
				}
			}
			else {
				move (enemyPath);
			}
		}
	}

	void move(List<Path> path) {
		if (path.Count > 0 && currentStep < path.Count) {
			if (path [currentStep].dir == Direction.Up) {
				GetComponent<Transform> ().position += Vector3.up * speed * Time.deltaTime;
			}
			if (path [currentStep].dir == Direction.Down) {
				GetComponent<Transform> ().position += Vector3.down * speed * Time.deltaTime;
			}
			if (path [currentStep].dir == Direction.Left) {
				GetComponent<Transform> ().position += Vector3.left * speed * Time.deltaTime;
			}
			if (path [currentStep].dir == Direction.Right) {
				GetComponent<Transform> ().position += Vector3.right * speed * Time.deltaTime;
			}
		
			if (path [currentStep].steps <= Vector2.Distance (GetComponent<Transform> ().position, lastPos)) {
				Vector3 current = GetComponent<Transform> ().position;
			
				if (path [currentStep].dir == Direction.Up) {
					current.x = Mathf.Round (current.x);
					current.y = Mathf.Round (current.y);
				}
				if (path [currentStep].dir == Direction.Down) {
					current.x = Mathf.Round (current.x);
					current.y = Mathf.Round (current.y);
				}
				if (path [currentStep].dir == Direction.Left) {
					current.x = Mathf.Round (current.x);
					current.y = Mathf.Round (current.y);
				}
				if (path [currentStep].dir == Direction.Right) {
					current.x = Mathf.Round (current.x);
					current.y = Mathf.Round (current.y);
				}
			
				GetComponent<Transform> ().position = current;
				lastPos = current;
				
				if (++currentStep == enemyPath.Count) {
					if(isChase) {
						isChase = false;
					}
					else {
						currentStep = 0;
					}
				}
			}
		}
	}

	void backToStart() {
		setActive (false);
		List<Path> backPath = new List<Path> ();

		if (currentStep < chasePath.Count) {
			float d = Vector3.Distance (transform.position, lastPos);
			chasePath [currentStep].steps = Mathf.RoundToInt (d);

			for (int cnt = currentStep; cnt >= 0; cnt++) {
				if (chasePath [currentStep].dir == Direction.Up)
					backPath.Add (new Path (Direction.Down, chasePath [currentStep].steps));
				if (chasePath [currentStep].dir == Direction.Down)
					backPath.Add (new Path (Direction.Up, chasePath [currentStep].steps));
				if (chasePath [currentStep].dir == Direction.Left)
					backPath.Add (new Path (Direction.Right, chasePath [currentStep].steps));
				if (chasePath [currentStep].dir == Direction.Right)
					backPath.Add (new Path (Direction.Left, chasePath [currentStep].steps));
			}
			chasePath = backPath;
		}
		setActive (true);
	}

	void nodeToPath(ArrayList pathArray) {
		List<Path> path = new List<Path> ();
		for (int cnt = 0; cnt < pathArray.Count - 1; cnt++) {
			Vector3 v = new Vector3(((Node)pathArray[cnt + 1]).position.x
			                        - ((Node)pathArray[cnt]).position.x,
			                        ((Node)pathArray[cnt + 1]).position.y
			                        - ((Node)pathArray[cnt]).position.y);
			//Debug.Log ("[Enemy]Add path " + v);
			int dist = Mathf.RoundToInt(Mathf.Abs (v.x));
			if(dist > 0) {
				if(v.x > 0)
					path.Add(new Path(Direction.Right, dist));
				else
					path.Add(new Path(Direction.Left, dist));
			}

			dist = Mathf.RoundToInt(Mathf.Abs (v.y));
			if(dist > 0) {
				if(v.y > 0)
					path.Add(new Path(Direction.Up, dist));
				else
					path.Add(new Path(Direction.Down, dist));
			}
		}
		chasePath = path;
	}

	public void findPath(Vector3 goal) {
		//Debug.Log ("[Enemy]Find player " + goal);
		//Debug.Log ("[Enemy]Start position " + transform.position);
		// Get start node position
		Node startNode = new Node (NodeManager.instance.GetNodeCenter (transform.position));
		// Get end node position
		Node endNode = new Node (NodeManager.instance.GetNodeCenter (goal));

		ArrayList pathArray = AStar.FindPath (startNode, endNode);
		//Debug.Log (pathArray.Count);
		nodeToPath (pathArray);
		isChase = true;
		StartCoroutine (stopChase());
	}

	public int getStage() {
		return stage;
	}
	
	public void setActive(bool active) {
		isActive = active;
	}

	public void setStage(int stage) {
		this.stage = stage;
	}

	public void setChase(bool chase) {
		if (enableChase) {
			if (chase) {
				Debug.Log ("[Enemy]Start chase player");
				currentStep = 0;
				lastPos = transform.position;
				StartCoroutine(waitCd (refindPathTime, isChase));
			} else {
				Debug.Log ("[Enemy]Stop chase player");
				//backToStart ();
			}
		}
	}

	IEnumerator waitCd(float sec, bool setting) {
		setting = !setting;
		yield return new WaitForSeconds (sec);
		setting = !setting;
	}

	IEnumerator stopChase() {
		yield return new WaitForSeconds (chaseTime);
		isChase = false;
		isForceChase = false;
		int index = GetComponentInParent<SpawnEnemies> ().findSpawnIndex (transform);
		GetComponentInParent<SpawnEnemies> ().respawnEnemy (index);
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

	public Path(Direction dir, int step) {
		this.dir = dir;
		this.steps = step;
	}
}