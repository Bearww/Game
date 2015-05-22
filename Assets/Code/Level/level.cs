using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 地圖設置
 */
public class Level : MonoBehaviour {

	private int levelWidth;
	private int levelHeight;

	public List<Tile> tiles = new List<Tile> ();

	private Color[] tileColours;
	private Color[] topTileColours;
	
	public Color spawnEnemyColour;
	public Color spawnItemColour;
	public Color spawnPathColour;
	public Color spawnPointColour;

	public Texture2D levelTexture;
	public Texture2D topTileTexture;

	public SpawnItems spawnItems;
	public SpawnEnemies spawnEnemies;

	public Entity player;

	void Start () {
		levelWidth = levelTexture.width;
		levelHeight = levelTexture.height;
		loadLevel ();
		loadTopTiles ();
		loadEnemyPath ();
	}

	void Update () {
	}

	/*
	 * 讀取底層地圖
	 */
	void loadLevel() {
		Debug.Log ("[Level]Load level");
		tileColours = new Color[levelWidth * levelHeight];
		tileColours = levelTexture.GetPixels ();

		for (int y = 0; y < levelHeight; y++) {
			for(int x = 0; x < levelWidth; x++) {
				foreach(Tile t in tiles) {
					if(tileColours[x + y * levelWidth] == t.tileColor) {
						Instantiate(t.tileTransform, new Vector3(x, y), Quaternion.identity);
					}
				}
				if(tileColours[x + y * levelWidth] == spawnPointColour) {
					Instantiate(tiles[0].tileTransform, new Vector3(x, y), Quaternion.identity);
					Vector2 pos = new Vector2(x, y);
					player.transform.position = pos;
				}
				if(tileColours[x + y * levelWidth] == spawnEnemyColour) {
					spawnEnemies.addSpawnPoint(new Vector3(x, y));
					Instantiate(tiles[0].tileTransform, new Vector3(x, y), Quaternion.identity);
				}
				if(tileColours[x + y * levelWidth] == spawnPathColour) {
					Instantiate(tiles[0].tileTransform, new Vector3(x, y), Quaternion.identity);
				}
				if(tileColours[x + y * levelWidth] == spawnItemColour) {
					spawnItems.addSpawnPoint(new Vector3(x, y));
					Instantiate(tiles[0].tileTransform, new Vector3(x, y), Quaternion.identity);
				}
			}
		}
	}

	/*
	 * 讀取地圖上層裝飾
	 */
	void loadTopTiles() {
		Debug.Log ("[Level]Load top tiles");
		topTileColours = new Color[levelWidth * levelHeight];
		topTileColours = topTileTexture.GetPixels ();

		for (int y = 0; y < levelHeight; y++) {
			for(int x = 0; x < levelWidth; x++) {
				foreach(Tile t in tiles) {
					if(topTileColours[x + y * levelWidth] == t.tileColor) {
						Instantiate(t.tileTransform, new Vector3(x, y), Quaternion.identity);
					}
				}
			}
		}
	}

	void loadEnemyPath() {
		Debug.Log ("[Level]Load enemy path");
		if (tileColours == null) {
			tileColours = new Color[levelWidth * levelHeight];
			tileColours = levelTexture.GetPixels ();
		}

		for (int i = 0; i < spawnEnemies.spawnPoints.Count; i++) {
			Vector3 start = spawnEnemies.spawnPoints[i].spawnPosition;
			foundPath (start);
		}
	}

	void foundPath(Vector3 start) {
		EnemyPath ep = new EnemyPath ();

		List<Vector3> pt = new List<Vector3> ();
		Stack<Vector3> st = new Stack<Vector3> ();

		pt.Add(start);
		st.Push (start);

		while (st.Count > 0) {
			Vector3 c = st.Peek ();
			int cx = (int) c.x, cy = (int) c.y;

			if(cy < levelHeight - 1 && tileColours[(cy + 1) * levelWidth + cx] == spawnPathColour
			   && !isVisited(c, Direction.Up, pt)) {
				addPoint (c, Direction.Up, st, pt, ep);
			}
			else if(cy > 0 && tileColours[(cy - 1) * levelWidth +cx] == spawnPathColour
			   && !isVisited(c, Direction.Down, pt)) {
				addPoint (c, Direction.Down, st, pt, ep);
			}
			else if(cx > 0 && tileColours[cy * levelWidth + cx - 1] == spawnPathColour
			        && !isVisited(c, Direction.Left, pt)) {
				addPoint (c, Direction.Left, st, pt, ep);
			}
			else if(cx < levelWidth - 1 && tileColours[cy * levelWidth + cx + 1] == spawnPathColour
			        && !isVisited(c, Direction.Right, pt)) {
				addPoint (c, Direction.Right, st, pt, ep);
			}
			else {
				st.Pop ();
				removePoint(c, st, ep);
			}
		}
		spawnEnemies.addSpawnPath (ep);
	}

	bool isVisited(Vector3 current, Direction dir, List<Vector3> pt) {
		if (dir == Direction.Up)
			current.y += 1;
		if (dir == Direction.Down)
			current.y -= 1;
		if (dir == Direction.Left)
			current.x -= 1;
		if (dir == Direction.Right)
			current.x += 1;

		for (int index = 0; index < pt.Count; index++) {
			if(current == pt[index])
				return true;
		}
		return false;
	}

	void addPoint(Vector3 current, Direction dir, Stack<Vector3> st, List<Vector3> pt, EnemyPath ep) {
		if (dir == Direction.Up)
			current.y += 1;
		if (dir == Direction.Down)
			current.y -= 1;
		if (dir == Direction.Left)
			current.x -= 1;
		if (dir == Direction.Right)
			current.x += 1;

		pt.Add (current);
		st.Push (current);

		addStep (dir, ep);
	}

	void removePoint(Vector3 current, Stack<Vector3> st, EnemyPath ep) {
		if (st.Count == 0)
			return;

		Vector3 next = st.Peek ();
		Direction dir = new Direction();

		if (next.y > current.y)
			dir = Direction.Up;
		if (next.y < current.y)
			dir = Direction.Down;
		if (next.x > current.x)
			dir = Direction.Right;
		if (next.x < current.x)
			dir = Direction.Left;

		addStep (dir, ep);
	}

	void addStep(Direction dir, EnemyPath ep) {
		int count = ep.enemyPath.Count;
		if (count > 0 && ep.enemyPath [count - 1].dir == dir)
			ep.enemyPath [count - 1].steps += 1;
		else
			ep.enemyPath.Add (new Path (dir));
	}
}

[System.Serializable]
public class Tile {
	public string tileName;
	public Color tileColor;
	public Transform tileTransform;
}