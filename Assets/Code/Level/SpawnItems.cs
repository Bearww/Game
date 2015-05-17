using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnItems : MonoBehaviour {

	public ItemManager itemManager;
	
	public List<SpawnPoint> spawnPoints;

	void Start () {
		Debug.Log ("[SpawnItem]SpawnPoints " + spawnPoints.Count);
		DontDestroyOnLoad (gameObject);
	}

	public void addSpawnPoint(int itemId, Vector3 itemPosition) {
		//Debug.Log ("[SpawnItem]Add item " + itemId.ToString());
		int itemIndex = itemManager.getItemIndex (itemId);
		if (itemIndex < 0)
			itemIndex = 0;

		Transform item = Instantiate (itemManager.items [itemIndex].itemTransform, itemPosition, Quaternion.identity) as Transform;
		item.SetParent(transform);
		SpawnPoint spawnPoint = new SpawnPoint (item, itemPosition);
		spawnPoints.Add (spawnPoint);
	}

	public int findSpawnItem(Transform spawnItem) {
		for (int index = 0; index < spawnPoints.Count; index++) {
			if(spawnPoints[index].spawnTransform == spawnItem)
				return index;
		}
		return -1;
	}

	public void respawnItem(int index) {
		Destroy (spawnPoints [index].spawnTransform.gameObject);
		StartCoroutine(waitForRespawn(index));
	}

	IEnumerator waitForRespawn(int index) {
		yield return new WaitForSeconds(2);
		Transform item = Instantiate (itemManager.items [0].itemTransform, spawnPoints[index].spawnPosition, Quaternion.identity) as Transform;
		item.SetParent(transform);
		spawnPoints [index].spawnTransform = item;
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