using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickPage : MonoBehaviour {

	public int currentPage;
	public Loading load;
	public Sprite[] pages;

	public void onClick(int dir) {
		if (dir == 0) {
			if(currentPage == 0) {
				GetComponent<Image> ().color = new Color(1, 1, 1, 0);
				load.loading("hall");
			}
			else {
				currentPage--;
				loadPage();
			}
		}
		if (dir == 1) {
			if(currentPage == pages.Length - 1) {
				GetComponent<Image> ().color = new Color(1, 1, 1, 0);
				load.loading("hall");
			}
			else {
				currentPage++;
				loadPage();
			}
		}
	}

	void loadPage() {
		GetComponent<Image> ().sprite = pages[currentPage];
	}
}
