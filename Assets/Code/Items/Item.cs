using UnityEngine;
using System.Collections;

/*
 * 物品編號、名稱、類型、持續時間
 */

public class Item : MonoBehaviour {

	public int id;
	public string itemName;
	public ItemType itemType;

	void Start () {
	
	}

	void Update () {
	
	}
}

public enum ItemType {
	Role,
	Buff,
	Store,
	Special
}