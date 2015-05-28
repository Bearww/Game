using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnItems : MonoBehaviour {
	
	private List<int> spawnAmount = new List<int> ();
	private List<Transform> spawnItem = new List<Transform> ();

	public ItemManager itemManager;

	public float respawnTime;

	public List<SpawnPoint> spawnPoints;

	public List<Transform> stageProperty;

	public SpawnEnemies enemies;
	
	public void addItem(Item item) {
		spawnAmount.Add (0);
		spawnItem.Add (itemManager.getItem (item));
	}

	public void addSpawnPoint(Vector3 itemPosition) {
		//Debug.Log ("[SpawnItem]Add item " + itemId.ToString());
		SpawnPoint spawnPoint = new SpawnPoint (itemPosition);
		spawnPoints.Add (spawnPoint);
	}

	public int findSpawnItem(Item item) {
		for (int index = 0; index < spawnItem.Count; index++) {
			if(spawnItem[index].GetComponent<Item> ().id == item.id)
				return index;
		}
		return -1;
	}

	public int findSpawnItem(Transform spawnItem) {
		for (int index = 0; index < spawnPoints.Count; index++) {
			if(spawnPoints[index].spawnTransform == spawnItem)
				return index;
		}
		return -1;
	}

	public void removeItem(Item item) {
		Debug.Log ("[SpawnItem]Remove item");
		int index;
		for (index = 0; index < spawnItem.Count; index++) {
			if(spawnItem[index].GetComponent<Item> ().id == item.id)
				break;
		}

		spawnAmount.RemoveAt (index);
		spawnItem.RemoveAt (index);

		for (index = 0; index < spawnPoints.Count; index++) {
			if(spawnPoints[index].spawnTransform.GetComponent<Item> ().id == item.id) {
				respawnItem(index);
			}
		}
	}

	public void respawnItem(int index) {
		changeItemAmount (index);
		Destroy (spawnPoints [index].spawnTransform.gameObject);
		StartCoroutine(waitForRespawn(index));
	}

	public void startSpawnItems() {
		Debug.Log ("[SpawnItem]SpawnPoints " + spawnPoints.Count);
		for (int index = 0; index < spawnPoints.Count; index++) {
			//Debug.Log ("[SpawnEnemy]Spawn " + e);
			Transform item = getSpawnItem();
			spawn (index, item);
		}
	}

	void changeItemAmount(int index) {
		int i = findSpawnItem (spawnPoints [index].spawnTransform.GetComponent<Item> ());
		
		if (i < 0) {
			Debug.Log ("[SpanwItem]Invalid item");
		}
		else {
			spawnAmount [i]--;
		}
	}

	Transform getSpawnItem() {
		int items = stageProperty.Count + spawnAmount.Count;
		for (int i = stageProperty.Count; i < spawnAmount.Count; i++) {
			if(spawnAmount[i] == 0)
				items++;
		}

		int r = (int)(Random.Range (0f, 0.99f) * items);
		if (r < stageProperty.Count) {
			spawnAmount[r]++;
			return stageProperty[r];
		}

		r -= stageProperty.Count;
		if (r < spawnAmount.Count) {
			spawnAmount[r]++;
			return spawnItem[r];
		}

		r -= spawnAmount.Count;
		for (int i = 0, j = 0; i < spawnAmount.Count; i++) {
			if(spawnAmount[i] == 0) {
				if(r == j) {
					spawnAmount[j]++;
					return spawnItem[i];
				}
				else {
					j++;
				}
			}
		}
		return null;
	}

	void spawn(int index, Transform i) {
		Transform item = Instantiate (i, spawnPoints[index].spawnPosition, Quaternion.identity) as Transform;
		item.SetParent(transform);
		spawnPoints[index].spawnTransform = item;
	}

	IEnumerator waitForRespawn(int index) {
		yield return new WaitForSeconds(respawnTime);
		Transform item = getSpawnItem();
		spawn (index, item);
	}
}

[System.Serializable]
public class SpawnPoint {
	public Transform spawnTransform;
	public Vector3 spawnPosition;

	public SpawnPoint(Vector3 itemPosition) {
		this.spawnPosition = itemPosition;
	}

	public SpawnPoint(Transform itemTransform, Vector3 itemPosition) {
		this.spawnTransform = itemTransform;
		this.spawnPosition = itemPosition;
	}
}