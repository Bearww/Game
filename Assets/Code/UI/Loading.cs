using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loading : MonoBehaviour {
	
	private int loadProgress = 0;
	private bool isLoadDone;

	public string levelToLoad;
	public bool isNeedClick;

	public GameObject background;
	public GameObject text;
	public GameObject progressBar;
	public GameObject progressBg;

	void Start() {
		background.SetActive (false);
		progressBg.SetActive (false);
		text.SetActive (false);
		progressBar.SetActive (false);
	}

	public void onClick() {
	}
	
	public void loading(string level) {
		levelToLoad = level;
		StartCoroutine (displayLoadingScreen ());
	}

	IEnumerator displayLoadingScreen() {
		Debug.Log ("[Loading]Load:" + levelToLoad);
		background.SetActive (true);
		progressBg.SetActive (true);
		text.SetActive (true);
		progressBar.SetActive (true);

		progressBar.transform.localScale = new Vector3 (loadProgress,
		                                               progressBar.transform.localScale.y,
		                                               progressBar.transform.localScale.z);

		text.GetComponent<Text> ().text = loadProgress.ToString() + "%";

		AsyncOperation async = Application.LoadLevelAsync (levelToLoad);
		while (!async.isDone) {
			loadProgress = (int)(async.progress * 100);
			text.GetComponent<Text> ().text = loadProgress.ToString() + "%";

			progressBar.transform.localScale = new Vector3 (async.progress,
			                                                progressBar.transform.localScale.y,
			                                                progressBar.transform.localScale.z);
			yield return null;
		}
	}
}
