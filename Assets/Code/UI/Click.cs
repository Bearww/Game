using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

	private bool isEnable;
	
	public string levelToLoad;
	public float clickEnableSecond;

	public Loading load;
	
	void Start() {
		if (clickEnableSecond > 0) {
			isEnable = false;
			StartCoroutine (waitForTime ());
		} else
			isEnable = true;
	}
	
	public void onClick() {
		if(isEnable)
			load.loading (levelToLoad);
	}
	
	void OnMouseDown() {
		if(isEnable)
			load.loading (levelToLoad);
	}

	IEnumerator waitForTime() {
		yield return new WaitForSeconds(clickEnableSecond);
		isEnable = true;
	}
}
