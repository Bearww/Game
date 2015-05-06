using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 玩家物品管理
 */
public class PlayerItemManager : MonoBehaviour {

	public ItemManager iManager;

	public List<Inventory> items = new List<Inventory> ();

	void Start () {
	
	}

	void Update () {
	
	}

	public void addToItemInventory(int itemId, int amount) {
		int locItem = findInItems (itemId);
		if (locItem < 0) {
			Debug.Log ("[PlayerItemManager]Invalid item loc");
		}
		else {
			int locInv = findInInventory(itemId);
			if(locInv >= 0)
				items[locInv].amount += amount;
			else {
				Inventory inv = new Inventory(iManager.items[locItem].itemTransform.GetComponent<Item> (), amount);
				items.Add(inv);
			}
		}
	}

	private int findInItems(int itemId) {
		for (int i = 0; i < iManager.items.Count; i++) {
			if(iManager.items[i].itemTransform.GetComponent<Item> ().id == itemId) {
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