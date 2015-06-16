using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialItem : BuffItem {

	private bool isUsable = false;
	private int itemId;
	private Sprite itemSprite;
	private Image bufImage;
	private PlayerItemManager playerItems;

	public int collectNumber;

	void launchEffect() {
		Debug.Log ("[SpecialItem]Launch Item");
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
		playerItems.removeFromItemInventory (itemId, collectNumber);
	}

	public void clearItem() {
		GetComponent<Image> ().sprite = null;
		GetComponent<Image> ().color = new Color(1, 1, 1, 0);
		GetComponentInChildren<Text> ().text = "";
		isUsable = false;
	}

	public void useItem() {
		if (isUsable) {
			launchEffect();
		}
	}

	public void setSpecialItem(Image image) {
		SpecialItem s = image.GetComponent<SpecialItem> ();
		itemId = s.itemId;
		itemDuration = s.itemDuration;
		itemEffect = s.itemEffect;
		itemPower = s.itemPower;
		collectNumber = s.collectNumber;
		itemSprite = s.itemSprite;
		isUsable = s.isUsable;
		GetComponent<Image> ().sprite = image.sprite;
		GetComponent<Image> ().color = image.color;
		GetComponentInChildren<Text> ().text = image.GetComponentInChildren<Text> ().text;
	}

	public void setSpecialItem(Item item, Sprite sprite) {
		if (item.itemType == ItemType.Special) {
			itemId = item.id;
			SpecialItem s = item.GetComponent<SpecialItem> ();
			itemDuration = s.itemDuration;
			itemEffect = s.itemEffect;
			itemPower = s.itemPower;
			collectNumber = s.collectNumber;
			itemSprite = sprite;
			GetComponent<Image> ().sprite = sprite;
		} else {
			Debug.Log ("[StoreItem]Invalid item type");
		}
		isUsable = false;
	}

	public bool setItemUsable(int amount) {
		isUsable = (amount == collectNumber);
		if (isUsable)
			GetComponent<Image> ().color = new Color (1, 1, 1, 1);
		else
			GetComponent<Image> ().color = new Color (0.5f, 0.5f, 0.5f, 1);

		if (isUsable)
			GetComponentInChildren<Text> ().text = "";
		else
			GetComponentInChildren<Text> ().text = string.Format ("{0}/{1}", amount, collectNumber);

		return amount > collectNumber;
	}
}
