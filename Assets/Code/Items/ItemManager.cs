using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 物品管理
 */

public class ItemManager : MonoBehaviour {

	public List<Items> items = new List<Items> ();

	void Start () {
		Debug.Log ("[ItemManager]items: " + items.Count.ToString());
		DontDestroyOnLoad (gameObject);
	}
	
	void Update () {
	
	}

	public Transform getItem(Item item) {
		for (int i = 0; i < items.Count; i++) {
			if(items[i].itemTransform.GetComponent<Item> ().id == item.id)
				return items[i].itemTransform;
		}
		return null;
	}

	public int getItemIndex(int id) {
		for (int i = 0; i < items.Count; i++) {
			if(items[i].itemTransform.GetComponent<Item> ().id == id)
				return i;
		}
		return -1;
	}

	public Sprite getItemSprite(int id) {
		for (int i = 0; i < items.Count; i++) {
			if(items[i].itemTransform.GetComponent<Item> ().id == id)
				return items[i].itemSprite;
		}
		return null;
	}
}

[System.Serializable]
public class Items {
	public Transform itemTransform;
	public Sprite itemSprite;
}