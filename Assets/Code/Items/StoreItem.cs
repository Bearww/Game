using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreItem : BuffItem {

	public bool isUsable = false;
	private int itemId;
	private Sprite itemSprite;
	public Image bufImage;
	private PlayerItemManager playerItems;

	public float cd;
	public int price;
	public string description;

	void Start() {
		if (bufImage == null) {
			GameObject buf = GameObject.FindGameObjectWithTag ("Buff");
			bufImage = buf.GetComponent<Image> ();
		}
		if (playerItems == null) {
			GameObject p = GameObject.FindGameObjectWithTag ("Player");
			playerItems = p.GetComponent<PlayerItemManager> ();
		}
	}

	IEnumerator launchEffect() {
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
		bufImage.sprite = itemSprite;
		bufImage.color = new Color (1, 1, 1, 1);
		playerItems.removeFromItemInventory (itemId, 1);
		yield return new WaitForSeconds (itemDuration);
		bufImage.sprite = null;
		bufImage.color = new Color (1, 1, 1, 0);
		StartCoroutine(intoCooling ());
	}
	
	IEnumerator intoCooling() {
		yield return new WaitForSeconds (cd);
		if(GetComponent<Image> ().color.r == 1)
			isUsable = true;
	}

	public void useItem() {
		if (isUsable) {
			StartCoroutine(launchEffect());
		}
	}

	public void setItem(Transform item, Sprite sprite) {
		if (item.GetComponent<Item> ().itemType == ItemType.Store) {
			itemId = item.GetComponent<Item> ().id;
			StoreItem s = item.GetComponent<StoreItem> ();
			itemDuration = s.itemDuration;
			itemEffect = s.itemEffect;
			cd = s.cd;
			itemSprite = sprite;
			isUsable = true;
		} else {
			Debug.Log ("[StoreItem]Invalid item type");
			isUsable = false;
		}
	}

	public void setItemNotUsable() {
		isUsable = false;
	}
}
