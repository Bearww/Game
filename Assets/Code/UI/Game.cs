using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public Loading load;

	void Awake () {
		load.loading ("hall");
	}

	void Update () {
	
	}
}
