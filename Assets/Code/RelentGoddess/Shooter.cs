using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public Player player;
	public BuffItem debuf;

	void timeUp() {
		Debug.Log ("[Shooter]Shooting fail");
		player.setShooter (false);
		player.setShooterUsable (false);
		debuf.setItem(-1, ItemEffect.Move, 0, 5f);
	}
}
