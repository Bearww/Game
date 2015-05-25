using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UseItem : MonoBehaviour {
	
	private bool isUsable = true;
	private float cd;
	private int itemAmount;

	public Item item;
	public BuffItem buf;
	public Image itemImage;
	public Image bufImage;
	public Image debImage;

	void OnMouseDown() {
		if (isUsable) {
			if(item.itemType == ItemType.Store) {
				StartCoroutine(intoCooling());
			}
			if(item.itemType == ItemType.Special) {
				itemImage.sprite = null;
				itemImage.color = new Color(255, 255, 255, 0);
			}
		}
	}

	IEnumerator launchEffect() {
		yield return new WaitForSeconds (1);
	}

	IEnumerator intoCooling() {
		isUsable = false;
		yield return new WaitForSeconds (cd);
		isUsable = true;
	}

	public void setItem() {
		if (item.itemType == ItemType.Store) {
			cd = item.GetComponent<StoreItem> ().cd;
		}
		if (item.itemType == ItemType.Special) {
			itemAmount = item.GetComponent<SpecialItem> ().collectNumber;
		}
	}
}
