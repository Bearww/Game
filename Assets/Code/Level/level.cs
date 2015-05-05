using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 地圖設置
 */
public class level : MonoBehaviour {

	private int levelWidth;
	private int levelHeight;

	public List<Tile> tiles = new List<Tile> ();

	private Color[] tileColours;
	private Color[] topTileColours;

	public Color spawnPointColour;
	public Color spawnEnemyColour;
	public Color spawnItemColour;

	public Texture2D levelTexture;
	public Texture2D topTileTexture;

	public SpawnItems spawnItems;
	public SpawnEnemies spawnEnemies;

	public Entity player;

	void Start () {
		levelWidth = levelTexture.width;
		levelHeight = levelTexture.height;
		loadLevel ();
		//loadTopTiles ();
	}

	void Update () {
	
	}

	/*
	 * 讀取底層地圖
	 */
	void loadLevel() {
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
					spawnEnemies.addSpawnPoint(0, new Vector3(x, y));
					Instantiate(tiles[0].tileTransform, new Vector3(x, y), Quaternion.identity);
				}
				if(tileColours[x + y * levelWidth] == spawnItemColour) {
					spawnItems.addSpawnPoint(0, new Vector3(x, y));
					Instantiate(tiles[0].tileTransform, new Vector3(x, y), Quaternion.identity);
				}
			}
		}
	}

	/*
	 * 讀取地圖上層裝飾
	 */
	void loadTopTiles() {
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
}

[System.Serializable]
public class Tile {
	public string tileName;
	public Color tileColor;
	public Transform tileTransform;
}