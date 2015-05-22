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
			Debug.Log (string.Format("[PlayerItemManager]Add itemId:{0}", itemId));
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
			Debug.Log (string.Format("[PlayerItemManager]Add itemId:{0} amount:{1}", itemId, amount));
			int invIndex = findInInventory(itemId);
			if(invIndex >= 0)
				items[invIndex].amount += amount;
			else {
				Inventory inv = new Inventory(itemManager.items[itemIndex].itemTransform.GetComponent<Item> (), amount);
				items.Add(inv);
				invIndex = 0;
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
			else {
				Debug.Log (string.Format("[PlayerItemManager]Remove item id:{0}", itemId));
				updateItemUI(invIndex);
				items.RemoveAt(invIndex);
			}
		}
	}

	public void removeFromItemInventory(int itemId, int amount) {
		int itemIndex = findInItems (itemId);
		if (itemIndex < 0) {
			Debug.Log ("[PlayerItemManager | Remove]Invalid item loc");
		}
		else {
			int invIndex = findInInventory(itemId);
			if(invIndex < 0)
				Debug.Log ("[PlayerItemManager | Remove]Invalid item inv");
			else {
				Debug.Log (string.Format("[PlayerItemManager]Remove itemId:{0} amount:{1}", itemId, amount));
				items[invIndex].amount -= amount;
			}
		}
	}

	public void removeFromItemInventory(ItemType itemType) {
		int invIndex = findInInventory(itemType);
		if (invIndex < 0)
			Debug.Log ("[PlayerItemManager | Remove]Invalid item type");
		else {
			Debug.Log (string.Format("[PlayerItemManager]Remove item type:{0}", itemType));
			updateItemUI(invIndex);
			items.RemoveAt (invIndex);
		}
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

	private void updateItemUI(int invIndex) {
		Item item = items [invIndex].item;
		Sprite itemSprite = itemManager.getItemSprite (item.id);
		int amountOfItem, index;

		if (item.itemType == ItemType.Role) {
			amountOfItem = roleItem.Length;
			for(index = 0; index < amountOfItem; index++) {
				if(roleItem[index].GetComponent<Image> ().sprite == itemSprite) {
					break;
				}
			}

			for(; index < amountOfItem - 1; index++) {
				roleItem[index].GetComponent<Image> ().sprite = roleItem[index + 1].GetComponent<Image> ().sprite;
			}
			roleItem[amountOfItem - 1].sprite = null;
		}
	}

	private void updateItemUI(int itemIndex, int invIndex) {
		Item item = items [invIndex].item;
		Sprite itemSprite = itemManager.getItemSprite (item.id);
		int amountOfItem;

		if (item.itemType == ItemType.Role) {
			amountOfItem = roleItem.Length;
			for(int index = 0; index < amountOfItem; index++) {
				if(roleItem[index].GetComponent<Image> ().sprite == null) {
					roleItem[index].sprite = itemSprite;
					return;
				}
			}
			removeFromItemInventory(ItemType.Role);
			roleItem[amountOfItem - 1].sprite = itemSprite;
		}

		if (item.itemType == ItemType.Store) {
			amountOfItem = storeItem.Length;
			for(int index = 0; index < amountOfItem; index++) {
				if(storeItem[index].GetComponent<Image> ().sprite == null ||
				   storeItem[index].GetComponent<Image> ().sprite == itemSprite) {
					storeItem[index].sprite = itemSprite;
					storeItem[index].GetComponentInChildren<Text> ().text = items[invIndex].amount.ToString ();
					return;
				}
			}
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