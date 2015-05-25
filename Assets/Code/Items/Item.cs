using UnityEngine;
using System.Collections;

/*
 * 物品編號、名稱、類型
 */

public class Item : MonoBehaviour {
	public int id;
	public string itemName;
	public ItemType itemType;
}

public enum ItemType {
	Role,
	Buff,
	Store,
	Special
}