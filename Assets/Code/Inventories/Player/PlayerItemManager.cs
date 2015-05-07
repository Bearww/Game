using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * 玩家物品管理
 */
public class PlayerItemManager : MonoBehaviour {

	public Image[] roleItem;
	public Image[] storeItem, specialItem;

	public ItemManager itemManager;

	public List<Inventory> items = new List<Inventory> ();

	void Start () {
	
	}

	void Update () {
	
	}

	public void addToItemInventory(int itemId) {
		int itemIndex = findInItems (itemId);
		if (itemIndex < 0) {
			Debug.Log ("[PlayerItemManager]Invalid item loc");
		}
		else {
			Inventory inv = new Inventory(itemManager.items[itemIndex].itemTransform.GetComponent<Item> (), 1);
			items.Add(inv);
			updateItemUI(itemIndex, items.Count - 1);
		}
	}

	public void addToItemInventory(int itemId, int amount) {
		int itemIndex = findInItems (itemId);
		if (itemIndex < 0) {
			Debug.Log ("[PlayerItemManager | Add]Invalid item loc");
		}
		else {
			int invIndex = findInInventory(itemId);
			if(invIndex >= 0)
				items[invIndex].amount += amount;
			else {
				Inventory inv = new Inventory(itemManager.items[itemIndex].itemTransform.GetComponent<Item> (), amount);
				items.Add(inv);
			}
			updateItemUI(itemIndex, invIndex);
		}
	}

	public void removeFromItemInventory(int itemId) {
		int itemIndex = findInItems (itemId);
		if (itemIndex < 0) {
			Debug.Log ("[PlayerItemManager | Remove]Invalid item loc");
		}
		else {
			int invIndex = findInInventory(itemId);
			if(invIndex < 0)
				Debug.Log ("[PlayerItemManager | Remove]Invalid item inv");
			else
				items.RemoveAt(invIndex);
		}
	}

	public void removeFromItemInventory(int itemId, int amount) {
		int itemIndex = findInItems (itemId);
		if (itemIndex < 0) {
			Debug.Log ("[PlayerItemManager | Add]Invalid item loc");
		}
		else {
			int invIndex = findInInventory(itemId);
			if(invIndex < 0)
				Debug.Log ("[PlayerItemManager | Remove]Invalid item inv");
			else
				items[invIndex].amount -= amount;
		}
	}

	public void removeFromItemInventory(ItemType itemType) {
		int invIndex = findInInventory(itemType);
		if(invIndex < 0)
			Debug.Log ("[PlayerItemManager | Remove]Invalid item type");
		else
			items.RemoveAt(invIndex);
	}

	private int findInItems(int itemId) {
		for (int i = 0; i < itemManager.items.Count; i++) {
			if(itemManager.items[i].itemTransform.GetComponent<Item> ().id == itemId) {
				return i;
			}
		}
		return -1;
	}

	public int findInInventory(int itemId) {
		for (int i = 0; i < items.Count; i++) {
			if (items [i].item.id == itemId) {
				return i;
			}
		}
		return -1;
	}

	public int findInInventory(ItemType itemType) {
		for (int i = 0; i < items.Count; i++) {
			if (items [i].item.itemType == itemType) {
				return i;
			}
		}
		return -1;
	}

	private void updateItemUI(int itemIndex, int invIndex) {
		Item item = items [invIndex].item;
		int amountOfItem;
		Sprite itemSprite = itemManager.items [itemIndex].itemTransform.GetComponent<Item> ().itemSprite;

		if (item.itemType == ItemType.Role) {
			amountOfItem = roleItem.Length;
			for(int index = 0; index < amountOfItem; index++) {
				if(roleItem[index].GetComponent<Image> ().sprite == null) {
					roleItem[index].sprite = itemSprite;
					return;
				}
			}
			removeFromItemInventory(ItemType.Role);
			for(int index = 0; index < amountOfItem - 1; index++) {
				roleItem[index].GetComponent<Image> ().sprite = roleItem[index + 1].GetComponent<Image> ().sprite;
			}
			roleItem[amountOfItem - 1].sprite = itemSprite;
		}
	}
}

[System.Serializable]
public class Inventory {
	public Item item;
	public int amount;

	public Inventory(Item item, int amountOfItem) {
		this.item = item;
		this.amount = amountOfItem;
	}
}