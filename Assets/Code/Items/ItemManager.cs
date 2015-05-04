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
}

[System.Serializable]
public class Items {
	public Transform itemTransform;
}