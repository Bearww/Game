using UnityEngine;
using System.Collections;

/*
 * 物品生成
 */

public class SpawnItem : MonoBehaviour {
	private GameObject playerObject;

	private GameObject itemManagerObject;

	private Player player;

	public ItemManager itemManager;

	private int currentItemId;

	private Transform currentItemTransform;

	private bool isActive = true;

	public Transform test;

	void Start () {
		if (playerObject == null) {
			playerObject = GameObject.FindGameObjectWithTag("Player");
			player = playerObject.GetComponent<Player> ();
		}

		if (itemManagerObject == null) {
			itemManagerObject = GameObject.FindGameObjectWithTag("Item");
			itemManager = itemManagerObject.GetComponent<ItemManager> ();
			currentItemTransform = itemManager.items [0].itemTransform;
			currentItemId = currentItemTransform.GetComponent<Item> ().id;
		}
	}

	void Update () {
		if (isActive) {
			if (Vector2.Distance (GetComponentInChildren<Rigidbody2D> ().transform.position, player.GetComponent<Rigidbody2D> ().transform.position) < 1) {
				player.pItemManager.addToItemInventory (currentItemId, 1);
				StartCoroutine(waitForRespawn());
			}
			if(Input.GetKey (KeyCode.F)) {
				Destroy (GetComponent<Transform> ().GetChild(0).gameObject);
				Transform obj = Instantiate(itemManager.items[0].itemTransform, GetComponent<Transform> ().position, Quaternion.identity) as Transform;
				obj.parent = transform;
			}
		}
	}

	IEnumerator waitForRespawn()
	{
		isActive = false;
		GetComponentInChildren<SpriteRenderer> ().enabled = isActive;
		yield return new WaitForSeconds(2);
		isActive = true;
		GetComponentInChildren<SpriteRenderer> ().enabled = isActive;
	}
}
