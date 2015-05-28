using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreItem : BuffItem {

	private bool isUsable = false;
	private int itemId;
	private Sprite itemSprite;
	private Image bufImage;
	private PlayerItemManager playerItems;

	public float cd;
	public int price;
	public string description;

	void launchEffect() {
		Debug.Log ("[StoreItem]Launch Item");
		isUsable = false;
		if (bufImage == null) {
			GameObject buf = GameObject.FindGameObjectWithTag ("Buff");
			bufImage = buf.GetComponent<Image> ();
		}
		if (playerItems == null) {
			GameObject p = GameObject.FindGameObjectWithTag ("Player");
			playerItems = p.GetComponent<PlayerItemManager> ();
		}
		bufImage.GetComponent<BuffItem> ().setItem (itemId, itemEffect, itemPower, itemDuration, itemSprite);
		playerItems.removeFromItemInventory (itemId, 1);
		StartCoroutine(intoCooling ());
	}
	
	IEnumerator intoCooling() {
		yield return new WaitForSeconds (cd);
		if(GetComponent<Image> ().color.r == 1)
			isUsable = true;
	}

	public void useItem() {
		if (isUsable) {
			launchEffect();
		}
	}

	public void setStoreItem(Item item, Sprite sprite) {
		if (item.itemType == ItemType.Store) {
			itemId = item.id;
			StoreItem s = item.GetComponent<StoreItem> ();
			itemDuration = s.itemDuration;
			itemEffect = s.itemEffect;
			itemPower = s.itemPower;
			cd = s.cd;
			itemSprite = sprite;
			isUsable = true;
		} else {
			Debug.Log ("[StoreItem]Invalid item type");
			isUsable = false;
		}
	}
}
