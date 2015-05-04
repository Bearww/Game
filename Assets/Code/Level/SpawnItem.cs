using UnityEngine;
using System.Collections;

/*
 * 物品生成
 */

public class SpawnItem : MonoBehaviour {
	private GameObject playerObject;
	private GameObject itemManagerObject;

	private Player player;

	private int currentItemId;
	private Transform currentItemTransform;

	private bool isActive = true;

	public ItemManager itemManager;

	void Start () {
		if (playerObject == null) {
			playerObject = GameObject.FindGameObjectWithTag("Player");
			player = playerObject.GetComponent<Player> ();
		}

		if (itemManagerObject == null) {
			itemManagerObject = GameObject.FindGameObjectWithTag("Item");
			itemManager = itemManagerObject.GetComponent<ItemManager> ();
		}
		setItemInfo (0);
	}

	void Update () {
		if (isActive) {
			if (Vector2.Distance (GetComponentInChildren<Rigidbody2D> ().transform.position, player.GetComponent<Rigidbody2D> ().transform.position) < 1) {
				player.pItemManager.addToItemInventory (currentItemId, 1);
				StartCoroutine(waitForRespawn());
			}
		}
	}

	IEnumerator waitForRespawn() {
		setItemActive (false);
		yield return new WaitForSeconds(2);
		setItemActive (true);
	}

	void spawnItem(int id) {
		if(GetComponentInChildren<Item> ().id != id) {
			int index = itemManager.getItemIndex (id);
			if(index < 0) index = 0;
			Destroy (GetComponent<Transform> ().GetChild(0).gameObject);
			Transform obj = Instantiate(itemManager.items[index].itemTransform, GetComponent<Transform> ().position, Quaternion.identity) as Transform;
			obj.parent = transform;
			setItemInfo(index);
		}
		setItemActive (true);
	}

	void setItemInfo(int index) {
		currentItemTransform = itemManager.items [index].itemTransform;
		currentItemId = currentItemTransform.GetComponent<Item> ().id;
	}

	void setItemActive(bool active) {
		isActive = active;
		GetComponentInChildren<SpriteRenderer> ().enabled = isActive;
	}
}
