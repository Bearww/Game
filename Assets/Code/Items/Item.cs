using UnityEngine;
using System.Collections;

/*
 * 物品編號、名稱、類型、持續時間
 */

public class Item : MonoBehaviour {

	private int stage;

	public int id;
	public string itemName;
	public ItemType itemType;

	void Start () {
	
	}

	void Update () {
	
	}

	public int getStage() {
		return stage;
	}

	public void setStage(int stage) {
		this.stage = stage;
	}
}

public enum ItemType {
	Role,
	Buff,
	Store,
	Special
}